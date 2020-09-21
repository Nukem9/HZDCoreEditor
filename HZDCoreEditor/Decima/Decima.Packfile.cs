using System;
using System.IO;
using System.Numerics;
using System.Text;
using Utility;

namespace Decima
{
    public class Packfile
    {
        public PackfileHeader Header { get; private set; }
        public FileEntry[] FileEntries { get; private set; }
        public BlockEntry[] BlockEntries { get; private set; }
        private readonly FileStream ArchiveFileHandle;

        public class PackfileHeader
        {
            public const uint HardcodedMagic = 0x20304050;
            public const uint HardcodedMagicEncrypted = 0x21304050;

            public uint Magic;
            public uint XorKey;
            public uint FileEntryCount;
            public uint BlockEntryCount;

            public bool IsEncrypted => Magic == HardcodedMagicEncrypted;

            public static PackfileHeader FromData(BinaryReader reader)
            {
                var x = new PackfileHeader();
                Span<byte> xorData = reader.ReadBytes(40);

                x.Magic = BitConverter.ToUInt32(xorData.Slice(0));  // 0x0
                x.XorKey = BitConverter.ToUInt32(xorData.Slice(4)); // 0x4

                switch (x.Magic)
                {
                    case HardcodedMagic:
                    case HardcodedMagicEncrypted:
                        break;

                    default:
                        throw new InvalidDataException("Unknown header magic");
                }

                if (x.IsEncrypted)
                {
                    var key1 = x.BuildXorKey(BitConverter.GetBytes(x.XorKey));
                    var key2 = x.BuildXorKey(BitConverter.GetBytes(x.XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x18] ^= key2[i];// XOR bytes 24 to 40
                }

                //_ = BitConverter.ToUInt32(xorData.Slice(8));                  // 0x8 - 0x18 Not used?
                x.FileEntryCount = BitConverter.ToUInt32(xorData.Slice(24));    // 0x18
                x.BlockEntryCount = BitConverter.ToUInt32(xorData.Slice(32));   // 0x20
                //_ = BitConverter.ToUInt32(xorData.Slice(36));                 // 0x24 Not used?

                return x;
            }

            public byte[] BuildXorKey(ReadOnlySpan<byte> mixData)
            {
                // Seed value was pulled from ds.exe
                Span<byte> key = new byte[] { 0x43, 0x94, 0x3A, 0xFA, 0x62, 0xAB, 0x1C, 0xF4, 0x1C, 0x81, 0x76, 0xF3, 0x3E, 0x9E, 0xA8, 0xD2 };

                for (int i = 0; i < mixData.Length; i++)
                    key[i] = mixData[i];

                SMHasher.MurmurHash3_x64_128(key, 42, out ulong[] hash);
                BitConverter.TryWriteBytes(key.Slice(0), hash[0]);
                BitConverter.TryWriteBytes(key.Slice(8), hash[1]);

                return key.ToArray();
            }
        }

        /// <summary>
        /// Data structure that comes directly after the packfile header.
        /// </summary>
        /// <remarks>
        /// <see cref="PathHash"/> is calculated from the MurmurHash3 of the file path with
        /// the null terminator included. Entries must be sorted from smallest to largest based
        /// on <see cref="PathHash"/>.
        /// </remarks>
        public class FileEntry
        {
            public uint XorKey1;
            public ulong PathHash;
            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public uint XorKey2;

            public static FileEntry FromData(BinaryReader reader, PackfileHeader header)
            {
                var x = new FileEntry();
                Span<byte> xorData = reader.ReadBytes(32);

                if (header.IsEncrypted)
                {
                    x.XorKey1 = BitConverter.ToUInt32(xorData.Slice(4));    // 0x4
                    x.XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));   // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(x.XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(x.XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x10] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0x4 and 0x1C in xorData are trashed now. They seem to ignore it.
                }

                //_ = BitConverter.ToUInt32(xorData.Slice(0));                      // 0x0 Not used?
                x.PathHash = BitConverter.ToUInt64(xorData.Slice(8));               // 0x8
                x.DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(16));    // 0x10
                x.DecompressedSize = BitConverter.ToUInt32(xorData.Slice(24));      // 0x18

                return x;
            }
        }

        /// <summary>
        /// Data structure that comes directly after the <see cref="FileEntry"/> array. This describes
        /// chunks of compressed data.
        /// </summary>
        /// <remarks>
        /// Entries must be sorted from smallest to largest based on <see cref="DecompressedOffset"/>.
        /// </remarks>
        public class BlockEntry
        {
            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public uint XorKey1;
            public ulong Offset;
            public uint Size;
            public uint XorKey2;

            public static BlockEntry FromData(BinaryReader reader, PackfileHeader header)
            {
                var x = new BlockEntry();
                Span<byte> xorData = reader.ReadBytes(32);

                if (header.IsEncrypted)
                {
                    x.XorKey1 = BitConverter.ToUInt32(xorData.Slice(12));   // 0xC
                    x.XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));   // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(x.XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(x.XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0x10] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0xC and 0x1C in xorData are trashed now. They are manually restored in game code.
                }

                x.DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(0)); // 0x0
                x.DecompressedSize = BitConverter.ToUInt32(xorData.Slice(8));   // 0x8
                x.Offset = BitConverter.ToUInt64(xorData.Slice(16));            // 0x10
                x.Size = BitConverter.ToUInt32(xorData.Slice(24));              // 0x18

                return x;
            }

            public byte[] BuildXorKey()
            {
                // Murmurhash3 of the first 16 bytes of the BlockEntry header as stored in the file. No type casting = very ugly in C#.
                Span<byte> seed = stackalloc byte[16];
                BitConverter.TryWriteBytes(seed.Slice(0), DecompressedOffset);
                BitConverter.TryWriteBytes(seed.Slice(8), DecompressedSize);
                BitConverter.TryWriteBytes(seed.Slice(12), XorKey1);

                SMHasher.MurmurHash3_x64_128(seed, 42, out ulong[] hash);
                BitConverter.TryWriteBytes(seed.Slice(0), hash[0]);
                BitConverter.TryWriteBytes(seed.Slice(8), hash[1]);

                // XOR the seed with the data key
                var key = new byte[] { 0x37, 0x4A, 0x08, 0x6C, 0x95, 0x9D, 0x15, 0x7E, 0xE8, 0xF7, 0x5A, 0x3D, 0x3F, 0x7D, 0xAA, 0x18 };

                for (int i = 0; i < key.Length; i++)
                    key[i] ^= seed[i];

                return key;
            }
        }

        public Packfile(string archivePath, FileMode mode = FileMode.Open)
        {
            if (mode == FileMode.Open)
            {
                ArchiveFileHandle = File.Open(archivePath, mode, FileAccess.Read, FileShare.Read);

                using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
                {
                    Header = PackfileHeader.FromData(reader);
                    FileEntries = new FileEntry[Header.FileEntryCount];
                    BlockEntries = new BlockEntry[Header.BlockEntryCount];

                    for (uint i = 0; i < FileEntries.Length; i++)
                        FileEntries[i] = FileEntry.FromData(reader, Header);

                    for (uint i = 0; i < BlockEntries.Length; i++)
                        BlockEntries[i] = BlockEntry.FromData(reader, Header);
                }
            }
            else if (mode == FileMode.Create || mode == FileMode.CreateNew)
            {
                throw new NotImplementedException("Writing archives is not supported at the moment");
            }
            else
            {
                throw new NotImplementedException("Archive file mode must be Open, Create, or CreateNew");
            }
        }

        ~Packfile()
        {
            ArchiveFileHandle.Close();
        }

        public void ExtractFile(string path, string destinationPath, bool allowOverwrite = false)
        {
            ExtractFile(GetHashForPath(path), destinationPath, allowOverwrite);
        }

        public void ExtractFile(ulong pathId, string destinationPath, bool allowOverwrite = false)
        {
            // Hashed path -> file entry -> block entries
            uint fileIndex = GetFileEntryIndex(pathId);
            var fileEntry = FileEntries[fileIndex];

            uint firstBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset);
            uint lastBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset + fileEntry.DecompressedSize - 1);

            using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(File.Open(destinationPath, allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write)))
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                // Keep a small cache sitting around to avoid excessive allocations
                Span<byte> decompressedData = new byte[512 * 1024];

                ulong fileDataOffset = fileEntry.DecompressedOffset; // 
                ulong fileDataLength = fileEntry.DecompressedSize;   // Remainder

                // Files can be split across multiple sequential blocks
                for (uint blockIndex = firstBlock; blockIndex <= lastBlock; blockIndex++)
                {
                    var block = BlockEntries[blockIndex];

                    if (block.DecompressedSize > decompressedData.Length)
                        throw new Exception("Increase cache buffer size");

                    // Read from the bin, decrypt, and decompress
                    reader.BaseStream.Position = (long)block.Offset;
                    var data = reader.ReadBytesStrict(block.Size);

                    if (Header.IsEncrypted)
                    {
                        // Raw data is XOR'd with a 16-byte MD5 of the block entry key
                        var key = md5.ComputeHash(block.BuildXorKey());

                        // If possible, do the XOR in a vectorized loop
                        int vectorRegisterSize = Vector<byte>.Count;
                        int vectorizedLoopCount = data.Length / vectorRegisterSize;
                        int byteIndex = 0;

                        if (key.Length > vectorRegisterSize)
                            vectorizedLoopCount = 0;

                        if (vectorizedLoopCount > 0)
                        {
                            // Key length has to match register size, so replicate necessary values
                            Span<byte> temp = stackalloc byte[vectorRegisterSize];

                            for (int i = 0; i < vectorRegisterSize; i++)
                                temp[i] = key[i & 15];

                            var keyVector = new Vector<byte>(temp);

                            // XOR operation
                            for (int i = 0; i < vectorizedLoopCount; i++)
                            {
                                var inputVector = new Vector<byte>(data, byteIndex);
                                var outputVector = inputVector ^ keyVector;

                                outputVector.CopyTo(data, byteIndex);
                                byteIndex += vectorRegisterSize;
                            }
                        }

                        // Remainder
                        for (; byteIndex < data.Length; byteIndex++)
                            data[byteIndex] ^= key[byteIndex & 15];
                    }

                    if (!OodleLZ.Decompress(data, decompressedData))
                        throw new InvalidDataException("OodleLZ block decompression failed");

                    // Copy data from the adjusted offset within the decompressed buffer. If the file requires another block,
                    // truncate the copy and loop again.
                    ulong copyOffset = fileDataOffset - block.DecompressedOffset;
                    ulong copySize = Math.Min(fileDataLength, block.DecompressedSize - copyOffset);

                    writer.Write(decompressedData.Slice((int)copyOffset, (int)copySize));

                    fileDataOffset += copySize;
                    fileDataLength -= copySize;
                }
            }
        }

        public void AddFile(string path, string sourcePath)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            return GetFileEntryIndex(path) != uint.MaxValue;
        }

        private ulong GetHashForPath(string path)
        {
            SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(path + char.MinValue), 42, out ulong[] hash);
            return hash[0];
        }

        private uint GetFileEntryIndex(string path)
        {
            return GetFileEntryIndex(GetHashForPath(path));
        }

        private uint GetFileEntryIndex(ulong pathId)
        {
            int l = 0;
            int r = FileEntries.Length - 1;

            // Binary search
            while (l <= r)
            {
                int mid = l + (r - l) / 2;

                if (FileEntries[mid].PathHash == pathId)
                    return (uint)mid;
                else if (FileEntries[mid].PathHash < pathId)
                    l = mid + 1;
                else
                    r = mid - 1;
            }

            return uint.MaxValue;
        }

        private uint GetBlockEntryIndex(ulong offset)
        {
            // TODO: Binary search
            for (uint i = 0; i < BlockEntries.Length; i++)
            {
                if (offset >= BlockEntries[i].DecompressedOffset &&
                    offset < (BlockEntries[i].DecompressedOffset + BlockEntries[i].DecompressedSize))
                    return i;
            }

            return uint.MaxValue;
        }
    }
}
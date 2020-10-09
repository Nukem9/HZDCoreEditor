using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Utility;

namespace Decima
{
    public class Packfile : IDisposable
    {
        private readonly PackfileHeader Header;
        public readonly List<FileEntry> FileEntries;
        private readonly List<BlockEntry> BlockEntries;
        private readonly FileStream ArchiveFileHandle;

        private uint WriterBlockSizeThreshold;
        private ulong WriterDecompressedBlockOffset;

        /// <summary>
        /// Main header for *.bin files
        /// </summary>
        public class PackfileHeader
        {
            public const int DataHeaderSize = 40;// Bytes
            private const uint HardcodedMagic = 0x20304050;
            private const uint HardcodedMagicEncrypted = 0x21304050;

            public uint Magic;
            public uint XorKey;
            public uint FileEntryCount;
            public uint BlockEntryCount;

            public bool IsEncrypted
            {
                set { Magic = value ? HardcodedMagicEncrypted : HardcodedMagic; }
                get { return Magic == HardcodedMagicEncrypted; }
            }

            public void ToData(BinaryWriter writer)
            {
                Span<byte> data = stackalloc byte[DataHeaderSize];
                BitConverter.TryWriteBytes(data.Slice(0), Magic);           // 0x0
                BitConverter.TryWriteBytes(data.Slice(4), XorKey);          // 0x4
                BitConverter.TryWriteBytes(data.Slice(24), FileEntryCount); // 0x18
                BitConverter.TryWriteBytes(data.Slice(32), BlockEntryCount);// 0x20

                if (IsEncrypted)
                {
                    var key1 = BuildXorKey(BitConverter.GetBytes(XorKey));
                    var key2 = BuildXorKey(BitConverter.GetBytes(XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        data[i + 8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        data[i + 24] ^= key2[i];// XOR bytes 24 to 40
                }

                writer.Write(data);
            }

            public PackfileHeader FromData(BinaryReader reader)
            {
                Span<byte> xorData = reader.ReadBytes(DataHeaderSize);
                Magic = BitConverter.ToUInt32(xorData.Slice(0));            // 0x0
                XorKey = BitConverter.ToUInt32(xorData.Slice(4));           // 0x4

                switch (Magic)
                {
                    case HardcodedMagic:
                    case HardcodedMagicEncrypted:
                        break;

                    default:
                        throw new InvalidDataException("Unknown header magic");
                }

                if (IsEncrypted)
                {
                    var key1 = BuildXorKey(BitConverter.GetBytes(XorKey));
                    var key2 = BuildXorKey(BitConverter.GetBytes(XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        xorData[i + 24] ^= key2[i];// XOR bytes 24 to 40
                }

                //_ = BitConverter.ToUInt32(xorData.Slice(8));              // 0x8 - 0x18 Not used?
                FileEntryCount = BitConverter.ToUInt32(xorData.Slice(24));  // 0x18
                BlockEntryCount = BitConverter.ToUInt32(xorData.Slice(32)); // 0x20
                //_ = BitConverter.ToUInt32(xorData.Slice(36));             // 0x24 Not used?

                return this;
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
        /// Data structure that comes directly after the <see cref="PackfileHeader"/> header.
        /// </summary>
        /// <remarks>
        /// <see cref="PathHash"/> is calculated from the MurmurHash3 of the file path with
        /// the null terminator included. Entries must be sorted from smallest to largest based
        /// on <see cref="PathHash"/>.
        /// </remarks>
        public class FileEntry
        {
            public const int DataHeaderSize = 32;// Bytes

            public ulong PathHash;
            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public uint XorKey1;
            public uint XorKey2;

            public void ToData(BinaryWriter writer, PackfileHeader header)
            {
                Span<byte> data = stackalloc byte[DataHeaderSize];
                BitConverter.TryWriteBytes(data.Slice(8), PathHash);            // 0x8
                BitConverter.TryWriteBytes(data.Slice(16), DecompressedOffset); // 0x10
                BitConverter.TryWriteBytes(data.Slice(24), DecompressedSize);   // 0x18

                if (header.IsEncrypted)
                {
                    var key1 = header.BuildXorKey(BitConverter.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        data[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        data[i + 16] ^= key2[i];// XOR bytes 16 to 32
                }

                BitConverter.TryWriteBytes(data.Slice(4), XorKey1);             // 0x4
                BitConverter.TryWriteBytes(data.Slice(28), XorKey2);            // 0x1C

                writer.Write(data);
            }

            public FileEntry FromData(BinaryReader reader, PackfileHeader header)
            {
                Span<byte> xorData = reader.ReadBytes(DataHeaderSize);

                if (header.IsEncrypted)
                {
                    XorKey1 = BitConverter.ToUInt32(xorData.Slice(4));          // 0x4
                    XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));         // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0x4 and 0x1C in xorData are trashed now. They seem to ignore it.
                }

                //_ = BitConverter.ToUInt32(xorData.Slice(0));                  // 0x0 Not used?
                PathHash = BitConverter.ToUInt64(xorData.Slice(8));             // 0x8
                DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(16));  // 0x10
                DecompressedSize = BitConverter.ToUInt32(xorData.Slice(24));    // 0x18

                return this;
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
            public const int DataHeaderSize = 32;// Bytes

            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public ulong Offset;
            public uint Size;
            public uint XorKey1;
            public uint XorKey2;

            // There's no guarantee MD5 will be thread safe, so use a per-thread instance instead
            private static readonly ThreadLocal<MD5> MD5Context = new ThreadLocal<MD5>(() =>
            {
                return MD5.Create();
            });

            public void ToData(BinaryWriter writer, PackfileHeader header)
            {
                Span<byte> data = stackalloc byte[DataHeaderSize];
                BitConverter.TryWriteBytes(data.Slice(0), DecompressedOffset);  // 0x0
                BitConverter.TryWriteBytes(data.Slice(8), DecompressedSize);    // 0x8
                BitConverter.TryWriteBytes(data.Slice(16), Offset);             // 0x10
                BitConverter.TryWriteBytes(data.Slice(24), Size);               // 0x18

                if (header.IsEncrypted)
                {
                    var key1 = header.BuildXorKey(BitConverter.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        data[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        data[i + 16] ^= key2[i];// XOR bytes 16 to 32
                }

                BitConverter.TryWriteBytes(data.Slice(12), XorKey1);            // 0xC
                BitConverter.TryWriteBytes(data.Slice(28), XorKey2);            // 0x1C

                writer.Write(data);
            }

            public BlockEntry FromData(BinaryReader reader, PackfileHeader header)
            {
                Span<byte> xorData = reader.ReadBytes(DataHeaderSize);

                if (header.IsEncrypted)
                {
                    XorKey1 = BitConverter.ToUInt32(xorData.Slice(12));         // 0xC
                    XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));         // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0xC and 0x1C in xorData are trashed now. They are manually restored in game code.
                }

                DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(0));   // 0x0
                DecompressedSize = BitConverter.ToUInt32(xorData.Slice(8));     // 0x8
                Offset = BitConverter.ToUInt64(xorData.Slice(16));              // 0x10
                Size = BitConverter.ToUInt32(xorData.Slice(24));                // 0x18

                return this;
            }

            public void XorDataBuffer(byte[] data)
            {
                // Raw data is XOR'd with a 16-byte MD5 of the block entry key
                var key = BuildXorKey();

                // If possible, do the XOR in a vectorized loop
                int vectorRegisterSize = Vector<byte>.Count;
                int vectorizedLoopCount = data.Length / vectorRegisterSize;
                int byteIndex = 0;

                if (vectorRegisterSize % key.Length != 0)
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
                        var encData = new Vector<byte>(data, byteIndex);
                        var decData = encData ^ keyVector;

                        decData.CopyTo(data, byteIndex);
                        byteIndex += vectorRegisterSize;
                    }
                }

                // Remainder
                for (; byteIndex < data.Length; byteIndex++)
                    data[byteIndex] ^= key[byteIndex & 15];
            }

            private byte[] BuildXorKey()
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

                return MD5Context.Value.ComputeHash(key);
            }
        }

        public Packfile(string archivePath, FileMode mode = FileMode.Open, bool encrypted = false)
        {
            if (mode == FileMode.Open)
            {
                ArchiveFileHandle = File.Open(archivePath, mode, FileAccess.Read, FileShare.Read);

                using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
                {
                    Header = new PackfileHeader().FromData(reader);
                    FileEntries = new List<FileEntry>((int)Header.FileEntryCount);
                    BlockEntries = new List<BlockEntry>((int)Header.BlockEntryCount);

                    for (int i = 0; i < Header.FileEntryCount; i++)
                        FileEntries.Add(new FileEntry().FromData(reader, Header));

                    for (int i = 0; i < Header.BlockEntryCount; i++)
                        BlockEntries.Add(new BlockEntry().FromData(reader, Header));
                }
            }
            else if (mode == FileMode.Create || mode == FileMode.CreateNew)
            {
                ArchiveFileHandle = File.Open(archivePath, mode, FileAccess.ReadWrite, FileShare.None);

                Header = new PackfileHeader();
                FileEntries = new List<FileEntry>();
                BlockEntries = new List<BlockEntry>();

                Header.IsEncrypted = encrypted;
            }
            else
            {
                throw new NotImplementedException("Archive file mode must be Open, Create, or CreateNew");
            }
        }

        ~Packfile()
        {
            Dispose();
        }

        public void Dispose()
        {
            ArchiveFileHandle.Close();
        }

        public bool ContainsFile(string path)
        {
            return GetFileEntryIndex(path) != int.MaxValue;
        }

        public void AddFile(string path, string sourcePath)
        {
            throw new NotImplementedException();
        }

        public void ExtractFile(string path, string destinationPath, bool allowOverwrite = false)
        {
            ExtractFile(GetHashForPath(path), destinationPath, allowOverwrite);
        }

        public void ExtractFile(ulong pathId, string destinationPath, bool allowOverwrite = false)
        {
            // Hashed path -> file entry -> block entries
            int fileIndex = GetFileEntryIndex(pathId);
            var fileEntry = FileEntries[fileIndex];

            int firstBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset);
            int lastBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset + fileEntry.DecompressedSize - 1);

            using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(File.Open(destinationPath, allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write)))
            {
                // Keep a small cache sitting around to avoid excessive allocations
                Span<byte> decompressedData = new byte[512 * 1024];

                ulong fileDataOffset = fileEntry.DecompressedOffset; // 
                ulong fileDataLength = fileEntry.DecompressedSize;   // Remainder

                // Files can be split across multiple sequential blocks
                for (int blockIndex = firstBlock; blockIndex <= lastBlock; blockIndex++)
                {
                    var block = BlockEntries[blockIndex];

                    if (block.DecompressedSize > decompressedData.Length)
                        throw new Exception("Increase cache buffer size");

                    // Read from the bin, decrypt, and decompress
                    reader.BaseStream.Position = (long)block.Offset;
                    var data = reader.ReadBytesStrict(block.Size);

                    if (Header.IsEncrypted)
                        block.XorDataBuffer(data);

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

        public void BuildFromFileList(string physicalPathRoot, string[] sourceFiles)
        {
            WriterBlockSizeThreshold = 256 * 1024;
            WriterDecompressedBlockOffset = 0;
            byte[] tempCompressedBuffer = new byte[WriterBlockSizeThreshold * 2];

            long totalBlockSize = sourceFiles.Sum(file => new FileInfo(Path.Combine(physicalPathRoot, file)).Length);
            int blockCount = (int)((totalBlockSize + WriterBlockSizeThreshold) / WriterBlockSizeThreshold);
            int fileCount = sourceFiles.Length;

            using (var archiveWriter = new BinaryWriter(ArchiveFileHandle, Encoding.UTF8, true))
            using (var blockStream = new MemoryStream())
            {
                // Reserve space for the header
                archiveWriter.BaseStream.Position = CalculateArchiveHeaderLength(fileCount, blockCount);

                // Write file data sequentially
                ulong decompressedFileOffset = 0;

                foreach (string filePath in sourceFiles)
                {
                    using var reader = new BinaryReader(File.OpenRead(Path.Combine(physicalPathRoot, filePath)));

                    var fileEntry = new FileEntry()
                    {
                        PathHash = GetHashForPath(filePath),
                        DecompressedOffset = decompressedFileOffset,
                        DecompressedSize = (uint)reader.BaseStream.Length,
                    };

                    // This appends data until a 256KB block write/flush is triggered - combining multiple files into single block entries
                    reader.BaseStream.CopyTo(blockStream);
                    decompressedFileOffset += fileEntry.DecompressedSize;

                    WriteBlockEntries(archiveWriter, blockStream, false, tempCompressedBuffer);
                    FileEntries.Add(fileEntry);
                }

                WriteBlockEntries(archiveWriter, blockStream, true, tempCompressedBuffer);

                // Rewind & insert headers before the compressed data
                archiveWriter.BaseStream.Position = 0;
                WriteArchiveHeaders(archiveWriter);
            }
        }

        public void Validate()
        {
            //
            // Simple validation:
            //
            // - FileEntries must be sorted in order based on PathHash (asc)
            // - FileEntries must resolve to a valid block entry
            // - BlockEntries must be sorted in order based on DecompressedOffset (asc)
            // - BlockEntries should not exceed the file size (Offset + Size)
            //
            ulong previousPathHash = 0;
            ulong previousOffset = 0;

            foreach (var entry in FileEntries)
            {
                if (entry.PathHash < previousPathHash)
                    throw new InvalidDataException("Archive file entry array isn't sorted properly.");

                int firstBlock = GetBlockEntryIndex(entry.DecompressedOffset);
                int lastBlock = GetBlockEntryIndex(entry.DecompressedOffset + entry.DecompressedSize - 1);

                if (firstBlock == int.MaxValue || lastBlock == int.MaxValue)
                    throw new InvalidDataException("Unable to resolve a file to one or more block entries.");

                previousPathHash = entry.PathHash;
            }

            foreach (var entry in BlockEntries)
            {
                if (entry.DecompressedOffset < previousOffset)
                    throw new InvalidDataException("Archive block entry array isn't sorted properly.");

                if ((entry.Offset + entry.Size) > (ulong)ArchiveFileHandle.Length)
                    throw new InvalidDataException("Archive data truncated. A block entry header exceeds the file size.");

                previousOffset = entry.DecompressedOffset;
            }
        }

        private void WriteBlockEntries(BinaryWriter writer, MemoryStream blockStream, bool flushAllData, byte[] compressorBufferCache)
        {
            int dataRemainder = 0;
            int readPosition = 0;

            while (true)
            {
                dataRemainder = (int)Math.Min(blockStream.Length - readPosition, WriterBlockSizeThreshold);

                if (dataRemainder == 0 || (dataRemainder < WriterBlockSizeThreshold && !flushAllData))
                    break;

                var blockEntry = new BlockEntry()
                {
                    DecompressedOffset = WriterDecompressedBlockOffset,
                    DecompressedSize = (uint)dataRemainder,
                    Offset = (ulong)writer.BaseStream.Position,
                };

                // Compress
                long compressedSize = OodleLZ.Compress(blockStream.GetBuffer().AsSpan().Slice(readPosition, dataRemainder), compressorBufferCache);

                if (compressedSize == -1)
                    throw new Exception("Buffer compression failed");

                blockEntry.Size = (uint)compressedSize;

                // Encrypt
                if (Header.IsEncrypted)
                    blockEntry.XorDataBuffer(compressorBufferCache);

                // Write to disk
                writer.Write(compressorBufferCache, 0, (int)blockEntry.Size);

                WriterDecompressedBlockOffset += blockEntry.DecompressedSize;
                readPosition += dataRemainder;

                BlockEntries.Add(blockEntry);
            }

            // Free MemoryStream data that was already written to prevent excessive memory consumption
            if (readPosition > 0 && dataRemainder > 0)
            {
                Buffer.BlockCopy(blockStream.GetBuffer(), readPosition, blockStream.GetBuffer(), 0, dataRemainder);
                blockStream.SetLength(blockStream.Length - readPosition);
            }
        }

        private void WriteArchiveHeaders(BinaryWriter writer)
        {
            FileEntries.Sort((x, y) => x.PathHash.CompareTo(y.PathHash));
            Header.FileEntryCount = (uint)FileEntries.Count;

            BlockEntries.Sort((x, y) => x.DecompressedOffset.CompareTo(y.DecompressedOffset));
            Header.BlockEntryCount = (uint)BlockEntries.Count;

            Header.ToData(writer);

            foreach (var entry in FileEntries)
                entry.ToData(writer, Header);

            foreach (var entry in BlockEntries)
                entry.ToData(writer, Header);
        }

        private int CalculateArchiveHeaderLength(int fileEntryCount, int blockEntryCount)
        {
            return PackfileHeader.DataHeaderSize +
                (FileEntry.DataHeaderSize * fileEntryCount) +
                (BlockEntry.DataHeaderSize * blockEntryCount);
        }

        private ulong GetHashForPath(string path)
        {
            SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(path + char.MinValue), 42, out ulong[] hash);
            return hash[0];
        }

        private int GetFileEntryIndex(string path)
        {
            return GetFileEntryIndex(GetHashForPath(path));
        }

        private int GetFileEntryIndex(ulong pathId)
        {
            int l = 0;
            int r = FileEntries.Count - 1;

            // Binary search
            while (l <= r)
            {
                int mid = l + (r - l) / 2;

                if (FileEntries[mid].PathHash == pathId)
                    return mid;
                else if (FileEntries[mid].PathHash < pathId)
                    l = mid + 1;
                else
                    r = mid - 1;
            }

            return int.MaxValue;
        }

        private int GetBlockEntryIndex(ulong offset)
        {
            // TODO: Binary search
            for (int i = 0; i < BlockEntries.Count; i++)
            {
                if (offset >= BlockEntries[i].DecompressedOffset &&
                    offset < (BlockEntries[i].DecompressedOffset + BlockEntries[i].DecompressedSize))
                    return i;
            }

            return int.MaxValue;
        }
    }
}
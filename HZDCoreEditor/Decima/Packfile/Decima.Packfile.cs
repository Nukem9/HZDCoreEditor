using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Decima
{
    /// <summary>
    /// Basic class instance for handling game archives (.bin files).
    /// </summary>
    public abstract class Packfile
    {
        public const int InvalidEntryIndex = int.MaxValue;

        public const string CoreExt = ".core";
        public const string StreamExt = ".core.stream";

        public static readonly string[] ValidFileExtensions = new string[]
        {
            ".core",
            ".stream",
            ".coretext",
            ".coredebug",
            ".dep",
        };

        public PackfileHeader Header { get; protected set; }
        public IReadOnlyList<FileEntry> FileEntries => _fileEntries.AsReadOnly();
        public IReadOnlyList<BlockEntry> BlockEntries => _blockEntries.AsReadOnly();

        protected List<FileEntry> _fileEntries;
        protected List<BlockEntry> _blockEntries;

        /// <summary>
        /// Main header for *.bin files.
        /// </summary>
        public sealed class PackfileHeader
        {
            public const int DataHeaderSize = 40;// Bytes
            private const uint HardcodedMagic = 0x20304050;
            private const uint HardcodedMagicEncrypted = 0x21304050;

            public uint Magic { get; internal set; }
            public uint XorKey { get; internal set; }
            public uint FileEntryCount { get; internal set; }
            public uint BlockEntryCount { get; internal set; }

            public bool IsEncrypted
            {
                get => Magic == HardcodedMagicEncrypted;
                internal set => Magic = value ? HardcodedMagicEncrypted : HardcodedMagic;
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

            public static PackfileHeader FromData(BinaryReader reader)
            {
                var header = new PackfileHeader();

                Span<byte> xorData = reader.ReadBytesStrict(DataHeaderSize);
                header.Magic = BitConverter.ToUInt32(xorData.Slice(0));     // 0x0
                header.XorKey = BitConverter.ToUInt32(xorData.Slice(4));    // 0x4

                switch (header.Magic)
                {
                    case HardcodedMagic:
                    case HardcodedMagicEncrypted:
                        break;

                    default:
                        throw new InvalidDataException("Unknown Packfile header magic");
                }

                if (header.IsEncrypted)
                {
                    var key1 = header.BuildXorKey(BitConverter.GetBytes(header.XorKey));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(header.XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        xorData[i + 24] ^= key2[i];// XOR bytes 24 to 40
                }

                //_ = Bits.ToUInt32(xorData.Slice(8));                              // 0x8 - 0x18 Not used?
                header.FileEntryCount = BitConverter.ToUInt32(xorData.Slice(24));   // 0x18
                header.BlockEntryCount = BitConverter.ToUInt32(xorData.Slice(32));  // 0x20
                //_ = Bits.ToUInt32(xorData.Slice(36));                             // 0x24 Not used?

                return header;
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
        /// the null terminator included. Entries must be sorted from smallest to largest based on
        /// <see cref="PathHash"/>.
        /// </remarks>
        public sealed class FileEntry
        {
            public const int DataHeaderSize = 32;// Bytes

            public ulong PathHash { get; internal set; }
            public ulong DecompressedOffset { get; internal set; }
            public uint DecompressedSize { get; internal set; }
            public uint XorKey1 { get; internal set; }
            public uint XorKey2 { get; internal set; }

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

            public static FileEntry FromData(Span<byte> xorData, PackfileHeader header)
            {
                var entry = new FileEntry();

                if (header.IsEncrypted)
                {
                    entry.XorKey1 = BitConverter.ToUInt32(xorData.Slice(4));    // 0x4
                    entry.XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));   // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(entry.XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(entry.XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0x4 and 0x1C in xorData are trashed now. They seem to ignore it.
                }

                //_ = Bits.ToUInt32(xorData.Slice(0));                                  // 0x0 Not used?
                entry.PathHash = BitConverter.ToUInt64(xorData.Slice(8));               // 0x8
                entry.DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(16));    // 0x10
                entry.DecompressedSize = BitConverter.ToUInt32(xorData.Slice(24));      // 0x18

                return entry;
            }
        }

        /// <summary>
        /// Data structure that comes directly after the <see cref="FileEntry"/> array. This describes
        /// chunks of compressed data.
        /// </summary>
        /// <remarks>
        /// Entries must be sorted from smallest to largest based on <see cref="DecompressedOffset"/>.
        /// </remarks>
        public sealed class BlockEntry
        {
            public const int DataHeaderSize = 32;// Bytes

            public ulong DecompressedOffset { get; internal set; }
            public uint DecompressedSize { get; internal set; }
            public ulong Offset { get; internal set; }
            public uint Size { get; internal set; }
            public uint XorKey1 { get; internal set; }
            public uint XorKey2 { get; internal set; }

            // There's no guarantee MD5 will be thread safe, so use a per-thread instance instead
            private static readonly ThreadLocal<MD5> _localMD5Context = new ThreadLocal<MD5>(() =>
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

            public static BlockEntry FromData(Span<byte> xorData, PackfileHeader header)
            {
                var entry = new BlockEntry();

                if (header.IsEncrypted)
                {
                    entry.XorKey1 = BitConverter.ToUInt32(xorData.Slice(12));         // 0xC
                    entry.XorKey2 = BitConverter.ToUInt32(xorData.Slice(28));         // 0x1C

                    var key1 = header.BuildXorKey(BitConverter.GetBytes(entry.XorKey1));
                    var key2 = header.BuildXorKey(BitConverter.GetBytes(entry.XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0xC and 0x1C in xorData are trashed now. They are manually restored in game code.
                }

                entry.DecompressedOffset = BitConverter.ToUInt64(xorData.Slice(0));   // 0x0
                entry.DecompressedSize = BitConverter.ToUInt32(xorData.Slice(8));     // 0x8
                entry.Offset = BitConverter.ToUInt64(xorData.Slice(16));              // 0x10
                entry.Size = BitConverter.ToUInt32(xorData.Slice(24));                // 0x18

                return entry;
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

                    var keyVector = new Vector<byte>(temp.ToArray());

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

                // XOR the seed with the data key. Pulled from ds.exe.
                var key = new byte[] { 0x37, 0x4A, 0x08, 0x6C, 0x95, 0x9D, 0x15, 0x7E, 0xE8, 0xF7, 0x5A, 0x3D, 0x3F, 0x7D, 0xAA, 0x18 };

                for (int i = 0; i < key.Length; i++)
                    key[i] ^= seed[i];

                return _localMD5Context.Value.ComputeHash(key);
            }
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected Packfile()
        {
        }

        /// <summary>
        /// Checks if a Decima-formatted path is valid for this archive. Case sensitive.
        /// </summary>
        public bool ContainsFile(string corePath)
        {
            return GetFileEntryIndex(corePath) != InvalidEntryIndex;
        }

        /// <summary>
        /// Checks if a hashed path is valid for this archive.
        /// </summary>
        public bool ContainsFile(ulong pathId)
        {
            return GetFileEntryIndex(pathId) != InvalidEntryIndex;
        }

        /// <summary>
        /// Gets the decompressed size of a file with a Decima core path.
        /// </summary>
        public uint GetFileSize(string corePath)
        {
            return GetFileSize(GetHashForPath(corePath));
        }

        /// <summary>
        /// Gets the decompressed size of a file with a hashed path.
        /// </summary>
        public uint GetFileSize(ulong pathId)
        {
            int index = GetFileEntryIndex(pathId);

            if (index == InvalidEntryIndex)
                throw new FileNotFoundException($"Unable to find file with path ID {pathId}");

            return _fileEntries[index].DecompressedSize;
        }

        /// <summary>
        /// Return the specific index for a <see cref="FileEntry"/> instance in <see cref="_fileEntries"/>.
        /// </summary>
        /// <returns>The index or <see cref="InvalidEntryIndex"/> if it was not found.</returns>
        public int GetFileEntryIndex(string corePath)
        {
            return GetFileEntryIndex(GetHashForPath(corePath));
        }

        /// <summary>
        /// Return the specific index for a <see cref="FileEntry"/> instance in <see cref="_fileEntries"/>.
        /// </summary>
        /// <returns>The index or <see cref="InvalidEntryIndex"/> if it was not found.</returns>
        public int GetFileEntryIndex(ulong pathId)
        {
            int l = 0;
            int r = _fileEntries.Count - 1;

            // Binary search
            while (l <= r)
            {
                int mid = l + (r - l) / 2;

                if (_fileEntries[mid].PathHash == pathId)
                    return mid;
                else if (_fileEntries[mid].PathHash < pathId)
                    l = mid + 1;
                else
                    r = mid - 1;
            }

            return InvalidEntryIndex;
        }

        /// <summary>
        /// Return the specific index for a <see cref="BlockEntry"/> instance in <see cref="_blockEntries"/>.
        /// </summary>
        /// <returns>The index or <see cref="InvalidEntryIndex"/> if it was not found.</returns>
        public int GetBlockEntryIndex(ulong offset)
        {
            int l = 0;
            int r = _blockEntries.Count - 1;

            // Binary search
            while (l <= r)
            {
                int mid = l + (r - l) / 2;
                ulong decompressedOffset = _blockEntries[mid].DecompressedOffset;

                if (offset >= decompressedOffset && offset < (decompressedOffset + _blockEntries[mid].DecompressedSize))
                    return mid;
                else if (decompressedOffset < offset)
                    l = mid + 1;
                else
                    r = mid - 1;
            }

            return InvalidEntryIndex;
        }

        /// <summary>
        /// Formats a path to match Decima's core file paths. Backslashes are replaced with forward
        /// slashes. Leading slashes are removed. An optional extension is applied if one is not present.
        /// </summary>
        public static string SanitizePath(string path, string defaultExt = CoreExt)
        {
            path = path.Replace("\\", "/");

            // Cannot start with leading '/'
            if (path.Length > 0 && path[0] == '/')
                path = path.Substring(1);

            // Must end with one of the valid extensions
            string ext = Path.GetExtension(path);

            if (ext.Length == 0 || !ValidFileExtensions.Any(x => ext.Equals(x)))
                path += defaultExt;

            return path;
        }

        /// <summary>
        /// Convert a Decima-formatted path to a hashed path. Hashes are case sensitive.
        /// </summary>
        public static ulong GetHashForPath(string corePath)
        {
            var data = Encoding.UTF8.GetBytes(corePath + char.MinValue);

            SMHasher.MurmurHash3_x64_128(data, 42, out ulong[] hash);
            return hash[0];
        }

        /// <summary>
        /// Return the size of all headers as if they were being written to disk.
        /// </summary>
        /// <returns>Size in bytes</returns>
        protected static int CalculateArchiveHeaderLength(int fileEntryCount, int blockEntryCount)
        {
            return PackfileHeader.DataHeaderSize +
                (FileEntry.DataHeaderSize * fileEntryCount) +
                (BlockEntry.DataHeaderSize * blockEntryCount);
        }
    }
}
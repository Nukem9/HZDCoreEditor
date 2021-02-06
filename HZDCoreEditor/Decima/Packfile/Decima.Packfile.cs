using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using HZDCoreEditor.Util;
using Utility;

namespace Decima
{
    public abstract class Packfile
    {
        public const string CoreExt = ".core";

        public PackfileHeader Header { protected set; get; }
        public List<FileEntry> FileEntries { protected set; get; }
        public List<BlockEntry> BlockEntries { protected set; get; }

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
                Bits.TryWriteBytes(data.Slice(0), Magic);           // 0x0
                Bits.TryWriteBytes(data.Slice(4), XorKey);          // 0x4
                Bits.TryWriteBytes(data.Slice(24), FileEntryCount); // 0x18
                Bits.TryWriteBytes(data.Slice(32), BlockEntryCount);// 0x20

                if (IsEncrypted)
                {
                    var key1 = BuildXorKey(Bits.GetBytes(XorKey));
                    var key2 = BuildXorKey(Bits.GetBytes(XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        data[i + 8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        data[i + 24] ^= key2[i];// XOR bytes 24 to 40
                }

                writer.Write(data.ToArray());
            }

            public PackfileHeader FromData(BinaryReader reader)
            {
                Span<byte> xorData = reader.ReadBytes(DataHeaderSize);
                Magic = Bits.ToUInt32(xorData.Slice(0));            // 0x0
                XorKey = Bits.ToUInt32(xorData.Slice(4));           // 0x4

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
                    var key1 = BuildXorKey(Bits.GetBytes(XorKey));
                    var key2 = BuildXorKey(Bits.GetBytes(XorKey + 1));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 8] ^= key1[i];// XOR bytes 8 to 24

                    for (int i = 0; i < 16; i++)
                        xorData[i + 24] ^= key2[i];// XOR bytes 24 to 40
                }

                //_ = Bits.ToUInt32(xorData.Slice(8));              // 0x8 - 0x18 Not used?
                FileEntryCount = Bits.ToUInt32(xorData.Slice(24));  // 0x18
                BlockEntryCount = Bits.ToUInt32(xorData.Slice(32)); // 0x20
                //_ = Bits.ToUInt32(xorData.Slice(36));             // 0x24 Not used?

                return this;
            }

            public byte[] BuildXorKey(ReadOnlySpan<byte> mixData)
            {
                // Seed value was pulled from ds.exe
                Span<byte> key = new byte[] { 0x43, 0x94, 0x3A, 0xFA, 0x62, 0xAB, 0x1C, 0xF4, 0x1C, 0x81, 0x76, 0xF3, 0x3E, 0x9E, 0xA8, 0xD2 };

                for (int i = 0; i < mixData.Length; i++)
                    key[i] = mixData[i];

                SMHasher.MurmurHash3_x64_128(key, 42, out ulong[] hash);
                Bits.TryWriteBytes(key.Slice(0), hash[0]);
                Bits.TryWriteBytes(key.Slice(8), hash[1]);

                return key.ToArray();
            }
        }

        /// <summary>
        /// Data structure that comes directly after the <see cref="PackfileHeader"/> header.
        /// </summary>
        /// <remarks>
        /// <see cref="FileEntry.PathHash"/> is calculated from the MurmurHash3 of the file path with
        /// the null terminator included. Entries must be sorted from smallest to largest based on
        /// <see cref="FileEntry.PathHash"/>.
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
                Bits.TryWriteBytes(data.Slice(8), PathHash);            // 0x8
                Bits.TryWriteBytes(data.Slice(16), DecompressedOffset); // 0x10
                Bits.TryWriteBytes(data.Slice(24), DecompressedSize);   // 0x18

                if (header.IsEncrypted)
                {
                    var key1 = header.BuildXorKey(Bits.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(Bits.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        data[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        data[i + 16] ^= key2[i];// XOR bytes 16 to 32
                }

                Bits.TryWriteBytes(data.Slice(4), XorKey1);             // 0x4
                Bits.TryWriteBytes(data.Slice(28), XorKey2);            // 0x1C

                writer.Write(data.ToArray());
            }

            public FileEntry FromData(Span<byte> xorData, PackfileHeader header)
            {
                if (header.IsEncrypted)
                {
                    XorKey1 = Bits.ToUInt32(xorData.Slice(4));          // 0x4
                    XorKey2 = Bits.ToUInt32(xorData.Slice(28));         // 0x1C

                    var key1 = header.BuildXorKey(Bits.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(Bits.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0x4 and 0x1C in xorData are trashed now. They seem to ignore it.
                }

                //_ = Bits.ToUInt32(xorData.Slice(0));                  // 0x0 Not used?
                PathHash = Bits.ToUInt64(xorData.Slice(8));             // 0x8
                DecompressedOffset = Bits.ToUInt64(xorData.Slice(16));  // 0x10
                DecompressedSize = Bits.ToUInt32(xorData.Slice(24));    // 0x18

                return this;
            }
        }

        /// <summary>
        /// Data structure that comes directly after the <see cref="FileEntry"/> array. This describes
        /// chunks of compressed data.
        /// </summary>
        /// <remarks>
        /// Entries must be sorted from smallest to largest based on <see cref="BlockEntry.DecompressedOffset"/>.
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
                Bits.TryWriteBytes(data.Slice(0), DecompressedOffset);  // 0x0
                Bits.TryWriteBytes(data.Slice(8), DecompressedSize);    // 0x8
                Bits.TryWriteBytes(data.Slice(16), Offset);             // 0x10
                Bits.TryWriteBytes(data.Slice(24), Size);               // 0x18

                if (header.IsEncrypted)
                {
                    var key1 = header.BuildXorKey(Bits.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(Bits.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        data[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        data[i + 16] ^= key2[i];// XOR bytes 16 to 32
                }

                Bits.TryWriteBytes(data.Slice(12), XorKey1);            // 0xC
                Bits.TryWriteBytes(data.Slice(28), XorKey2);            // 0x1C

                writer.Write(data.ToArray());
            }

            public BlockEntry FromData(Span<byte> xorData, PackfileHeader header)
            {
                if (header.IsEncrypted)
                {
                    XorKey1 = Bits.ToUInt32(xorData.Slice(12));         // 0xC
                    XorKey2 = Bits.ToUInt32(xorData.Slice(28));         // 0x1C

                    var key1 = header.BuildXorKey(Bits.GetBytes(XorKey1));
                    var key2 = header.BuildXorKey(Bits.GetBytes(XorKey2));

                    for (int i = 0; i < 16; i++)
                        xorData[i + 0] ^= key1[i];// XOR bytes 0 to 16

                    for (int i = 0; i < 16; i++)
                        xorData[i + 16] ^= key2[i];// XOR bytes 16 to 32

                    // The XOR keys at offset 0xC and 0x1C in xorData are trashed now. They are manually restored in game code.
                }

                DecompressedOffset = Bits.ToUInt64(xorData.Slice(0));   // 0x0
                DecompressedSize = Bits.ToUInt32(xorData.Slice(8));     // 0x8
                Offset = Bits.ToUInt64(xorData.Slice(16));              // 0x10
                Size = Bits.ToUInt32(xorData.Slice(24));                // 0x18

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
                Bits.TryWriteBytes(seed.Slice(0), DecompressedOffset);
                Bits.TryWriteBytes(seed.Slice(8), DecompressedSize);
                Bits.TryWriteBytes(seed.Slice(12), XorKey1);

                SMHasher.MurmurHash3_x64_128(seed, 42, out ulong[] hash);
                Bits.TryWriteBytes(seed.Slice(0), hash[0]);
                Bits.TryWriteBytes(seed.Slice(8), hash[1]);

                // XOR the seed with the data key
                var key = new byte[] { 0x37, 0x4A, 0x08, 0x6C, 0x95, 0x9D, 0x15, 0x7E, 0xE8, 0xF7, 0x5A, 0x3D, 0x3F, 0x7D, 0xAA, 0x18 };

                for (int i = 0; i < key.Length; i++)
                    key[i] ^= seed[i];

                return MD5Context.Value.ComputeHash(key);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Packfile()
        {
        }
        
        /// <summary>
        /// Checks if a Decima-formatted path is valid for this archive
        /// </summary>
        public bool ContainsFile(string path)
        {
            return GetFileEntryIndex(path) != int.MaxValue;
        }
        
        public static string EnsureExt(string path)
        {
            if (!path.EndsWith(CoreExt, StringComparison.OrdinalIgnoreCase))
                path += CoreExt;
            return path;
        }
        public static ulong GetHashForPath(string path)
        {
            path = EnsureExt(path).Replace('\\', '/');
            SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(path + char.MinValue), 42, out ulong[] hash);
            return hash[0];
        }

        protected int GetFileEntryIndex(string path)
        {
            return GetFileEntryIndex(GetHashForPath(path));
        }

        protected int GetFileEntryIndex(ulong pathId)
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

        protected int GetBlockEntryIndex(ulong offset)
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
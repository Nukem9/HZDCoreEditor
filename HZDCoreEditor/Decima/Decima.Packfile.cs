using BinaryStreamExtensions;
using System;
using System.IO;
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
            public const uint HardcodedMagic = 0x20304050; // Need to check DS

            public uint Magic;
            public uint FileEntryCount;
            public uint BlockEntryCount;

            public static PackfileHeader FromData(BinaryReader reader)
            {
                var x = new PackfileHeader();
                x.Magic = reader.ReadUInt32();          // 0
                _ = reader.ReadBytesStrict(20);         // 4
                x.FileEntryCount = reader.ReadUInt32(); // 24
                _ = reader.ReadUInt32();                // 28
                x.BlockEntryCount = reader.ReadUInt32();// 32
                _ = reader.ReadUInt32();                // 36

                return x;
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
            public ulong PathHash;
            public ulong DecompressedOffset;
            public uint DecompressedSize;

            public static FileEntry FromData(BinaryReader reader)
            {
                var x = new FileEntry();
                _ = reader.ReadBytesStrict(8);              // 0
                x.PathHash = reader.ReadUInt64();           // 8
                x.DecompressedOffset = reader.ReadUInt64(); // 16
                x.DecompressedSize = reader.ReadUInt32();   // 24
                _ = reader.ReadBytesStrict(4);              // 28

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
            public ulong Offset;
            public uint Size;

            public static BlockEntry FromData(BinaryReader reader)
            {
                var x = new BlockEntry();
                x.DecompressedOffset = reader.ReadUInt64(); // 0
                x.DecompressedSize = reader.ReadUInt32();   // 8
                _ = reader.ReadUInt32();                    // 12
                x.Offset = reader.ReadUInt64();             // 16
                x.Size = reader.ReadUInt32();               // 24
                _ = reader.ReadUInt32();                    // 28

                return x;
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

                    if (Header.Magic != PackfileHeader.HardcodedMagic)
                        throw new InvalidDataException("Unknown header magic");

                    FileEntries = new FileEntry[Header.FileEntryCount];
                    BlockEntries = new BlockEntry[Header.BlockEntryCount];

                    for (uint i = 0; i < FileEntries.Length; i++)
                        FileEntries[i] = FileEntry.FromData(reader);

                    for (uint i = 0; i < BlockEntries.Length; i++)
                        BlockEntries[i] = BlockEntry.FromData(reader);
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
            // Hashed path -> file entry -> block entries
            uint fileIndex = GetFileEntryIndex(path);
            var fileEntry = FileEntries[fileIndex];

            uint firstBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset);
            uint lastBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset + fileEntry.DecompressedSize - 1);

            using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(File.Open(destinationPath, allowOverwrite ? FileMode.CreateNew : FileMode.Create, FileAccess.Write)))
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

                    // Read & decompress from the bin
                    reader.BaseStream.Position = (long)block.Offset;
                    var data = reader.ReadBytes(block.Size);

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

        private ulong GetHashForPath(string path)
        {
            SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(path + char.MinValue), 42, out ulong[] hash);
            return hash[0];
        }

        private uint GetFileEntryIndex(string path)
        {
            ulong hash = GetHashForPath(path);

            // TODO: Binary search, return bool instead
            for (uint i = 0; i < FileEntries.Length; i++)
            {
                if (FileEntries[i].PathHash == hash)
                    return i;
            }

            return uint.MaxValue;
        }

        private uint GetBlockEntryIndex(ulong offset)
        {
            // TODO: Binary search, return bool instead
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
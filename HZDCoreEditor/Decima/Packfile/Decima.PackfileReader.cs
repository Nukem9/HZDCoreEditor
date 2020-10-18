using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Decima
{
    public class PackfileReader : Packfile
    {
        private const uint ReaderBlockSizeThreshold = 256 * 1024;

        private readonly FileStream ArchiveFileHandle;

        public PackfileReader(string archivePath)
        {
            ArchiveFileHandle = File.Open(archivePath, FileMode.Open, FileAccess.Read, FileShare.Read);

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

        public override void Dispose()
        {
            ArchiveFileHandle.Close();
            base.Dispose();
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
                Span<byte> decompressedData = new byte[ReaderBlockSizeThreshold * 2];

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

        /// <summary>
        /// Simple header validation:
        /// - FileEntries must be sorted in order based on PathHash (asc)
        /// - FileEntries must resolve to a valid block entry
        /// - BlockEntries must be sorted in order based on DecompressedOffset (asc)
        /// - BlockEntries should not exceed the file size (Offset + Size)
        /// </summary>
        public void Validate()
        {
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
    }
}
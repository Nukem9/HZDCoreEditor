using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Utility;

namespace Decima
{
    public class PackfileReader : Packfile
    {
        private readonly string _archivePath;
        private const uint ReaderBlockSizeThreshold = 256 * 1024;

        public PackfileReader(string archivePath)
        {
            _archivePath = archivePath;
            using var handle = File.OpenRead(_archivePath);

            using var reader = new BinaryReader(handle, Encoding.UTF8, true);

            Header = new PackfileHeader().FromData(reader);
            FileEntries = new List<FileEntry>((int)Header.FileEntryCount);
            BlockEntries = new List<BlockEntry>((int)Header.BlockEntryCount);

            Span<byte> fileData = reader.ReadBytes(Header.FileEntryCount * FileEntry.DataHeaderSize);
            for (int i = 0; i < Header.FileEntryCount; i++)
            {
                var data = fileData.Slice(i * FileEntry.DataHeaderSize, FileEntry.DataHeaderSize);
                FileEntries.Add(new FileEntry().FromData(data, Header));
            }

            Span<byte> blockData = reader.ReadBytes(Header.BlockEntryCount * BlockEntry.DataHeaderSize);
            for (int i = 0; i < Header.BlockEntryCount; i++)
            {
                var data = blockData.Slice(i * BlockEntry.DataHeaderSize, BlockEntry.DataHeaderSize);
                BlockEntries.Add(new BlockEntry().FromData(data, Header));
            }
        }

        public void ExtractFile(string path, string destinationPath, bool allowOverwrite = false)
        {
            ExtractFile(GetHashForPath(path), destinationPath, allowOverwrite);
        }

        public void ExtractFile(ulong pathId, string destinationPath, bool allowOverwrite = false)
        {
            using var fs = File.Open(destinationPath, allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
            ExtractFile(pathId, fs);
        }
        public void ExtractFile(ulong pathId, Stream stream)
        {
            using var handle = File.OpenRead(_archivePath);

            // Hashed path -> file entry -> block entries
            int fileIndex = GetFileEntryIndex(pathId);
            var fileEntry = FileEntries[fileIndex];

            int firstBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset);
            int lastBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset + fileEntry.DecompressedSize - 1);

            using var reader = new BinaryReader(handle, Encoding.UTF8, true);
            using var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), true);

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

                //if the buffer is bigger then the decompressed size OodleLZ v3 doesn't decompress data correctly every time
                //however if the buffer is the correct size it will decompress correctly but report that it failed
                OodleLZ.Decompress(data, decompressedData, block.DecompressedSize);

                // Copy data from the adjusted offset within the decompressed buffer. If the file requires another block,
                // truncate the copy and loop again.
                ulong copyOffset = fileDataOffset - block.DecompressedOffset;
                ulong copySize = Math.Min(fileDataLength, block.DecompressedSize - copyOffset);

                writer.Write(decompressedData.Slice((int)copyOffset, (int)copySize));

                fileDataOffset += copySize;
                fileDataLength -= copySize;
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
            using var handle = File.OpenRead(_archivePath);

            ulong previousOffset = 0;
            ulong previousPathHash = 0;

            // Run this check first - GetBlockEntryIndex could fail otherwise
            foreach (var entry in BlockEntries)
            {
                if (entry.DecompressedOffset < previousOffset)
                    throw new InvalidDataException("Archive block entry array isn't sorted properly.");

                if ((entry.Offset + entry.Size) > (ulong)handle.Length)
                    throw new InvalidDataException("Archive data truncated. A block entry header exceeds the file size.");

                previousOffset = entry.DecompressedOffset;
            }

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
        }
    }
}
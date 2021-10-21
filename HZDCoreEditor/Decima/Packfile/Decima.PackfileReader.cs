using HZDCoreEditor.Util;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decima
{
    public class PackfileReader : Packfile
    {
        private readonly string _archivePath;

        public PackfileReader(string archivePath)
        {
            _archivePath = archivePath;

            using var handle = File.OpenRead(_archivePath);
            using var reader = new BinaryReader(handle, Encoding.UTF8, true);

            Header = PackfileHeader.FromData(reader);
            _fileEntries = new List<FileEntry>((int)Header.FileEntryCount);
            _blockEntries = new List<BlockEntry>((int)Header.BlockEntryCount);

            Span<byte> fileData = reader.ReadBytesStrict(Header.FileEntryCount * FileEntry.DataHeaderSize);
            Span<byte> blockData = reader.ReadBytesStrict(Header.BlockEntryCount * BlockEntry.DataHeaderSize);

            for (int i = 0; i < Header.FileEntryCount; i++)
            {
                var data = fileData.Slice(i * FileEntry.DataHeaderSize);
                _fileEntries.Add(FileEntry.FromData(data, Header));
            }

            for (int i = 0; i < Header.BlockEntryCount; i++)
            {
                var data = blockData.Slice(i * BlockEntry.DataHeaderSize);
                _blockEntries.Add(BlockEntry.FromData(data, Header));
            }
        }

        public void ExtractFile(string corePath, string destinationPath, bool allowOverwrite = false)
        {
            ExtractFile(GetHashForPath(corePath), destinationPath, allowOverwrite);
        }

        public void ExtractFile(ulong pathId, string destinationPath, bool allowOverwrite = false)
        {
            using var fs = File.Open(destinationPath, allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
            ExtractFile(pathId, fs);
        }

        public void ExtractFile(string corePath, Stream stream)
        {
            ExtractFile(GetHashForPath(corePath), stream);
        }

        public void ExtractFile(ulong pathId, Stream stream)
        {
            using var handle = File.OpenRead(_archivePath);

            // Hashed path -> file entry -> block entries
            int fileIndex = GetFileEntryIndex(pathId);
            var fileEntry = _fileEntries[fileIndex];

            int firstBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset);
            int lastBlock = GetBlockEntryIndex(fileEntry.DecompressedOffset + fileEntry.DecompressedSize - 1);

            ulong fileDataOffset = fileEntry.DecompressedOffset; // 
            ulong fileDataLength = fileEntry.DecompressedSize;   // Remainder

            // Files can be split across multiple sequential blocks
            for (int blockIndex = firstBlock; blockIndex <= lastBlock; blockIndex++)
            {
                var block = _blockEntries[blockIndex];

                var compressedData = ArrayPool<byte>.Shared.Rent((int)block.Size);
                var decompressedData = ArrayPool<byte>.Shared.Rent((int)block.DecompressedSize);

                // Read from the bin, decrypt, and decompress
                handle.Position = (long)block.Offset;

                if (handle.Read(compressedData, 0, (int)block.Size) != block.Size)
                    throw new EndOfStreamException("Short read of archive data");

                if (Header.IsEncrypted)
                    block.XorDataBuffer(compressedData);

                // if the buffer is bigger then the decompressed size OodleLZ v3 doesn't decompress data correctly every time
                // however if the buffer is the correct size it will decompress correctly but report that it failed
                OodleLZ.Decompress(compressedData, decompressedData, block.DecompressedSize);

                // Copy data from the adjusted offset within the decompressed buffer. If the file requires another block,
                // truncate the copy and loop again.
                ulong copyOffset = fileDataOffset - block.DecompressedOffset;
                ulong copySize = Math.Min(fileDataLength, block.DecompressedSize - copyOffset);

                stream.Write(decompressedData, (int)copyOffset, (int)copySize);

                fileDataOffset += copySize;
                fileDataLength -= copySize;

                ArrayPool<byte>.Shared.Return(decompressedData);
                ArrayPool<byte>.Shared.Return(compressedData);
            }
        }

        /// <summary>
        /// Simple header validation:
        /// - _fileEntries must be sorted in order based on PathHash (asc)
        /// - _fileEntries must resolve to a valid block entry
        /// - _blockEntries must be sorted in order based on DecompressedOffset (asc)
        /// - _blockEntries should not exceed the file size (Offset + Size)
        /// </summary>
        public void Validate()
        {
            using var handle = File.OpenRead(_archivePath);

            ulong previousOffset = 0;
            ulong previousPathHash = 0;

            // Run this check first - GetBlockEntryIndex could fail otherwise
            foreach (var entry in _blockEntries)
            {
                if (entry.DecompressedOffset < previousOffset)
                    throw new InvalidDataException("Archive block entry array isn't sorted properly.");

                if ((entry.Offset + entry.Size) > (ulong)handle.Length)
                    throw new InvalidDataException("Archive data truncated. A block entry header exceeds the file size.");

                previousOffset = entry.DecompressedOffset;
            }

            foreach (var entry in _fileEntries)
            {
                if (entry.PathHash < previousPathHash)
                    throw new InvalidDataException("Archive file entry array isn't sorted properly.");

                int firstBlock = GetBlockEntryIndex(entry.DecompressedOffset);
                int lastBlock = GetBlockEntryIndex(entry.DecompressedOffset + entry.DecompressedSize - 1);

                if (firstBlock == InvalidEntryIndex || lastBlock == InvalidEntryIndex)
                    throw new InvalidDataException("Unable to resolve a file to one or more block entries.");

                previousPathHash = entry.PathHash;
            }
        }
    }
}
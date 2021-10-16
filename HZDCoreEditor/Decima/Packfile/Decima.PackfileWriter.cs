using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Decima
{
    public class PackfileWriter : Packfile
    {
        private const uint WriterBlockSizeThreshold = 256 * 1024;

        private readonly string _archivePath;
        private readonly bool _allowOverwrite;
        private ulong WriterDecompressedBlockOffset;

        public PackfileWriter(string archivePath, bool encrypted = false, bool allowOverwrite = false)
        {
            _archivePath = archivePath;
            _allowOverwrite = allowOverwrite;

            Header = new PackfileHeader();
            FileEntries = new List<FileEntry>();
            BlockEntries = new List<BlockEntry>();

            Header.IsEncrypted = encrypted;
        }

        public void BuildFromFileList(string physicalPathRoot, string[] sourceFiles)
        {
            WriterDecompressedBlockOffset = 0;
            byte[] tempCompressedBuffer = new byte[WriterBlockSizeThreshold * 2];

            long totalBlockSize = sourceFiles.Sum(file => new FileInfo(Path.Combine(physicalPathRoot, file)).Length);
            int blockCount = (int)((totalBlockSize + WriterBlockSizeThreshold) / WriterBlockSizeThreshold);
            int fileCount = sourceFiles.Length;

            using var fs = File.Open(_archivePath, _allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
            using var archiveWriter = new BinaryWriter(fs, Encoding.UTF8, true);
            using var blockStream = new MemoryStream();

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
                long compressedSize = HZDCoreEditor.Util.OodleLZ.Compress(blockStream.GetBuffer().AsSpan().Slice(readPosition, dataRemainder), compressorBufferCache);

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
    }
}
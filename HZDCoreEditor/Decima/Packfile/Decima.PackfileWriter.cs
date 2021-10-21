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
        private readonly FileMode _fileMode;
        private ulong _writerDecompressedBlockOffset;

        public PackfileWriter(string archivePath, bool encrypted = false, FileMode mode = FileMode.CreateNew)
        {
            _archivePath = archivePath;
            _fileMode = mode;

            Header = new PackfileHeader();
            _fileEntries = new List<FileEntry>();
            _blockEntries = new List<BlockEntry>();

            Header.IsEncrypted = encrypted;
        }

        public void BuildFromFileList(string physicalPathRoot, string[] sourceFiles)
        {
            _writerDecompressedBlockOffset = 0;
            byte[] tempCompressedBuffer = new byte[WriterBlockSizeThreshold * 2];

            long totalBlockSize = sourceFiles.Sum(file => new FileInfo(Path.Combine(physicalPathRoot, file)).Length);
            int blockCount = (int)((totalBlockSize + WriterBlockSizeThreshold) / WriterBlockSizeThreshold);
            int fileCount = sourceFiles.Length;

            using var fs = File.Open(_archivePath, _fileMode, FileAccess.ReadWrite, FileShare.None);
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
                _fileEntries.Add(fileEntry);
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
                    DecompressedOffset = _writerDecompressedBlockOffset,
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

                _writerDecompressedBlockOffset += blockEntry.DecompressedSize;
                readPosition += dataRemainder;

                _blockEntries.Add(blockEntry);
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
            _fileEntries.Sort((x, y) => x.PathHash.CompareTo(y.PathHash));
            Header.FileEntryCount = (uint)_fileEntries.Count;

            _blockEntries.Sort((x, y) => x.DecompressedOffset.CompareTo(y.DecompressedOffset));
            Header.BlockEntryCount = (uint)_blockEntries.Count;

            Header.ToData(writer);

            foreach (var entry in _fileEntries)
                entry.ToData(writer, Header);

            foreach (var entry in _blockEntries)
                entry.ToData(writer, Header);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Decima
{
    /// <summary>
    /// Game archive writer.
    /// </summary>
    public class PackfileWriter : Packfile
    {
        private const uint WriterBlockSizeThreshold = 256 * 1024;

        private readonly string _archivePath;
        private readonly FileMode _fileMode;
        private ulong _writerDecompressedBlockOffset;

        /// <summary>
        /// Open an archive (.bin) file and prepare for writing.
        /// </summary>
        /// <param name="archivePath">Disk path</param>
        /// <param name="encrypted">Encrypt file data</param>
        /// <param name="mode">File creation mode</param>
        public PackfileWriter(string archivePath, bool encrypted = false, FileMode mode = FileMode.CreateNew)
        {
            _archivePath = archivePath;
            _fileMode = mode;

            Header = new PackfileHeader();
            _fileEntries = new List<FileEntry>();
            _blockEntries = new List<BlockEntry>();

            Header.IsEncrypted = encrypted;
        }

        /// <summary>
        /// Generate an archive from a set of file paths on disk.
        /// </summary>
        /// <param name="baseDirectoryRoot">Base directory to look for files</param>
        /// <param name="sourceCorePaths">List of files in Decima core path format. <see cref="baseDirectoryRoot"/>
        /// is prepended to each element.</param>
        public void BuildFromFileList(string baseDirectoryRoot, IEnumerable<string> sourceCorePaths)
        {
            long totalBlockSize = sourceCorePaths.Sum(file => new FileInfo(Path.Combine(baseDirectoryRoot, file)).Length);
            int blockCount = (int)((totalBlockSize + WriterBlockSizeThreshold) / WriterBlockSizeThreshold);
            int fileCount = sourceCorePaths.Count();

            using var archiveStream = File.Open(_archivePath, _fileMode, FileAccess.ReadWrite, FileShare.None);
            using var blockStream = new MemoryStream();

            // Reserve space for the header
            archiveStream.Position = CalculateArchiveHeaderLength(fileCount, blockCount);

            // Write file data sequentially
            byte[] tempCompressedBuffer = new byte[WriterBlockSizeThreshold * 2];

            _writerDecompressedBlockOffset = 0;
            ulong decompressedFileOffset = 0;

            foreach (string corePath in sourceCorePaths)
            {
                using var fileStream = File.OpenRead(Path.Combine(baseDirectoryRoot, corePath));

                var fileEntry = new FileEntry()
                {
                    PathHash = GetHashForPath(corePath),
                    DecompressedOffset = decompressedFileOffset,
                    DecompressedSize = (uint)fileStream.Length,
                };

                // Append data until a 256KB block write/flush is triggered - combine multiple files into single block entries
                fileStream.CopyTo(blockStream);
                decompressedFileOffset += fileEntry.DecompressedSize;

                WriteBlockEntries(archiveStream, blockStream, false, tempCompressedBuffer);
                _fileEntries.Add(fileEntry);
            }

            WriteBlockEntries(archiveStream, blockStream, true, tempCompressedBuffer);

            // Rewind & insert headers before the compressed data
            archiveStream.Position = 0;
            WriteArchiveHeaders(new BinaryWriter(archiveStream, Encoding.UTF8, true));
        }

        private void WriteBlockEntries(Stream writerStream, MemoryStream blockStream, bool flushAllData, byte[] compressorBufferCache)
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
                    Offset = (ulong)writerStream.Position,
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
                writerStream.Write(compressorBufferCache, 0, (int)blockEntry.Size);

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
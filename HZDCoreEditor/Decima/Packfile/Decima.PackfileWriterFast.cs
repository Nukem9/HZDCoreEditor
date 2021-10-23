using HZDCoreEditor.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Decima
{
    /// <summary>
    /// Game archive writer. This is designed to quickly write a small number of files in a multithreaded fashion. The
    /// packfile and its contents have to fit in available RAM. Does not support compressing large packs.
    /// </summary>
    public class PackfileWriterFast : Packfile
    {
        private const uint BlockSize = 256 * 1024;

        private readonly string _archivePath;
        private readonly FileMode _fileMode;

        private class CompressBlock
        {
            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public byte[] DataBuffer;
            public byte[] CompressBuffer;
            public uint Size;
        }

        /// <summary>
        /// Open an archive (.bin) file and prepare for writing.
        /// </summary>
        /// <param name="archivePath">Disk path</param>
        /// <param name="encrypted">Encrypt file data</param>
        /// <param name="mode">File creation mode</param>
        public PackfileWriterFast(string archivePath, bool encrypted = false, FileMode mode = FileMode.CreateNew)
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
            var compressedBlocks = new ConcurrentBag<CompressBlock>();

            var tasks = new ParallelTasks<CompressBlock>(
                Environment.ProcessorCount, block =>
                {
                    block.CompressBuffer = new byte[BlockSize * 2];

                    // Compress
                    long compressedSize = HZDCoreEditor.Util.OodleLZ.Compress(
                        block.DataBuffer.AsSpan(),
                        (int)block.DecompressedSize,
                        block.CompressBuffer.AsSpan());

                    if (compressedSize == -1)
                        throw new Exception("Buffer compression failed");

                    block.Size = (uint)compressedSize;
                    block.DataBuffer = null;

                    compressedBlocks.Add(block);
                });

            tasks.Start();

            AddFiles(baseDirectoryRoot, sourceCorePaths, tasks);

            tasks.WaitForComplete();

            using var fs = File.Open(_archivePath, _fileMode, FileAccess.ReadWrite, FileShare.None);
            using var archiveWriter = new BinaryWriter(fs, Encoding.UTF8, true);

            archiveWriter.BaseStream.Position = CalculateArchiveHeaderLength(sourceCorePaths.Count(), compressedBlocks.Count);
            WriteBlockEntries(archiveWriter, compressedBlocks);

            archiveWriter.BaseStream.Position = 0;
            WriteArchiveHeaders(archiveWriter);
        }

        private void AddFiles(string baseDirectoryRoot, IEnumerable<string> sourceCorePaths, ParallelTasks<CompressBlock> tasks)
        {
            // Write file data sequentially
            ulong decompressedFileOffset = 0;
            var readBuffer = new byte[BlockSize];
            var readBufferPos = 0;
            ulong blockOffset = 0;

            foreach (string corePath in sourceCorePaths)
            {
                using var fs = File.OpenRead(Path.Combine(baseDirectoryRoot, corePath));

                var fileEntry = new FileEntry()
                {
                    PathHash = GetHashForPath(corePath),
                    DecompressedOffset = decompressedFileOffset,
                    DecompressedSize = (uint)fs.Length,
                };

                decompressedFileOffset += fileEntry.DecompressedSize;

                _fileEntries.Add(fileEntry);

                // This appends data until a 256KB block write/flush is triggered - combining multiple files into single block entries
                int read;
                while ((read = fs.Read(readBuffer, readBufferPos, readBuffer.Length - readBufferPos)) > 0)
                {
                    if (readBufferPos + read < BlockSize)
                    {
                        readBufferPos += read;
                        break;
                    }

                    tasks.AddItem(new CompressBlock()
                    {
                        DecompressedOffset = blockOffset,
                        DecompressedSize = BlockSize,
                        DataBuffer = readBuffer
                    });

                    readBufferPos = 0;
                    readBuffer = new byte[BlockSize];
                    blockOffset += BlockSize;
                }
            }

            if (readBufferPos > 0)
            {
                tasks.AddItem(new CompressBlock()
                {
                    DecompressedOffset = blockOffset,
                    DecompressedSize = (uint)readBufferPos,
                    DataBuffer = readBuffer
                });
            }
        }

        private void WriteBlockEntries(BinaryWriter writer, ConcurrentBag<CompressBlock> blocks)
        {
            foreach (var block in blocks.OrderBy(x => x.DecompressedOffset))
            {
                var blockEntry = new BlockEntry()
                {
                    DecompressedOffset = block.DecompressedOffset,
                    DecompressedSize = block.DecompressedSize,
                    Offset = (ulong)writer.BaseStream.Position,
                    Size = block.Size
                };

                // Encrypt
                if (Header.IsEncrypted)
                    blockEntry.XorDataBuffer(block.CompressBuffer);

                // Write to disk
                writer.Write(block.CompressBuffer, 0, (int)blockEntry.Size);

                _blockEntries.Add(blockEntry);
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
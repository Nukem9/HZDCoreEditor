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
    /// Packfile has to fit in memory, does not support compressing large packs
    /// </summary>
    public class PackfileWriterFast : Packfile
    {
        private const uint BlockSize = 256 * 1024;

        private readonly string _archivePath;
        private readonly bool _allowOverwrite;

        private class CompressBlock
        {
            public ulong DecompressedOffset;
            public uint DecompressedSize;
            public byte[] DataBuffer;
            public byte[] CompressBuffer;
            public uint Size;
        }

        public PackfileWriterFast(string archivePath, bool encrypted = false, bool allowOverwrite = false)
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

            AddFiles(physicalPathRoot, sourceFiles, tasks);

            tasks.WaitForComplete();

            using var fs = File.Open(_archivePath, _allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
            using var archiveWriter = new BinaryWriter(fs, Encoding.UTF8, true);

            archiveWriter.BaseStream.Position = CalculateArchiveHeaderLength(sourceFiles.Length, compressedBlocks.Count);
            WriteBlockEntries(archiveWriter, compressedBlocks);

            archiveWriter.BaseStream.Position = 0;
            WriteArchiveHeaders(archiveWriter);
        }

        private void AddFiles(string physicalPathRoot, string[] sourceFiles, ParallelTasks<CompressBlock> tasks)
        {
            // Write file data sequentially
            ulong decompressedFileOffset = 0;
            var readBuffer = new byte[BlockSize];
            var readBufferPos = 0;
            ulong blockOffset = 0;

            foreach (string filePath in sourceFiles)
            {
                using var fs = File.OpenRead(Path.Combine(physicalPathRoot, filePath));

                var fileEntry = new FileEntry()
                {
                    PathHash = GetHashForPath(filePath),
                    DecompressedOffset = decompressedFileOffset,
                    DecompressedSize = (uint)fs.Length,
                };

                decompressedFileOffset += fileEntry.DecompressedSize;

                FileEntries.Add(fileEntry);

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

                BlockEntries.Add(blockEntry);
            }
        }

        private int CalculateArchiveHeaderLength(int fileEntryCount, int blockEntryCount)
        {
            return PackfileHeader.DataHeaderSize +
                (FileEntry.DataHeaderSize * fileEntryCount) +
                (BlockEntry.DataHeaderSize * blockEntryCount);
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
    }
}
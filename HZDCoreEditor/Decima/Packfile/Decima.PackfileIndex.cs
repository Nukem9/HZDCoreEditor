using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decima
{
    /// <summary>
    /// Class for handling packfile index files (".idx"). They're present in HZD's data directory but aren't
    /// loaded by the game.
    /// </summary>
    public class PackfileIndex
    {
        private const uint HardcodedMagic = 0x10203040;

        public IReadOnlyList<IndexEntry> Entries => _entries.AsReadOnly();

        private List<IndexEntry> _entries;

        /// <remarks>
        /// File data format:
        /// UInt32  (+0)  Path string length
        /// UInt8[] (+4)  Path string
        /// GGUUID  (+8)  GUID
        /// UInt8[] (+24) 16 unknown bytes
        /// </remarks>
        public class IndexEntry
        {
            public string FilePath;
            public byte[] FileDataHash;
            public ulong DecompressedOffset;
            public ulong DecompressedSize;

            public void ToData(BinaryWriter writer)
            {
                var data = Encoding.UTF8.GetBytes(FilePath);
                writer.Write((uint)data.Length);

                if (data.Length > 0)
                    writer.Write(data);

                writer.Write(FileDataHash);
                writer.Write(DecompressedOffset);
                writer.Write(DecompressedSize);
            }

            public static IndexEntry FromData(BinaryReader reader)
            {
                var entry = new IndexEntry();

                // Strings are prefixed with their mount type ("cache:", "appdir:", ...). We don't care about those. Trim them.
                uint filePathLen = reader.ReadUInt32();

                if (filePathLen > 0)
                    entry.FilePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(filePathLen));
                else
                    throw new Exception("Unexpected condition: Packfile index entry supplied without a file name");

                int delimiter = entry.FilePath.IndexOf(':');

                if (delimiter != -1)
                    entry.FilePath = entry.FilePath.Substring(delimiter + 1);

                entry.FileDataHash = reader.ReadBytesStrict(16);
                entry.DecompressedOffset = reader.ReadUInt64();
                entry.DecompressedSize = reader.ReadUInt64();

                return entry;
            }
        }

        /// <summary>
        /// Deserialize a PackfileIndex entry from a BinaryReader.
        /// </summary>
        /// <param name="reader">Source stream</param>
        public static PackfileIndex FromData(BinaryReader reader)
        {
            var index = new PackfileIndex();

            uint magic = reader.ReadUInt32();
            uint entryCount = reader.ReadUInt32();

            if (magic != HardcodedMagic)
                throw new InvalidDataException("Unknown header magic");

            index._entries = new List<IndexEntry>((int)entryCount);

            for (uint i = 0; i < entryCount; i++)
                index._entries.Add(IndexEntry.FromData(reader));

            return index;
        }

        /// <summary>
        /// Deserialize a PackfileIndex entry from a file on disk.
        /// </summary>
        /// <param name="filePath">Disk path</param>
        public static PackfileIndex FromFile(string filePath)
        {
            using var reader = new BinaryReader(File.OpenRead(filePath));
            return FromData(reader);
        }
    }
}

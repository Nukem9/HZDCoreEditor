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

        private List<IndexEntry> _entries = new List<IndexEntry>();

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
                throw new NotImplementedException();
            }

            public static IndexEntry FromData(BinaryReader reader)
            {
                var entry = new IndexEntry();

                // Strings are prefixed with their mount type ("cache:", "appdir:", ...)
                uint filePathLen = reader.ReadUInt32();

                if (filePathLen > 0)
                    entry.FilePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(filePathLen));

                entry.FileDataHash = reader.ReadBytesStrict(16);
                entry.DecompressedOffset = reader.ReadUInt64();
                entry.DecompressedSize = reader.ReadUInt64();

                return entry;
            }
        }

        public static PackfileIndex FromData(BinaryReader reader)
        {
            var index = new PackfileIndex();

            uint magic = reader.ReadUInt32();
            uint entryCount = reader.ReadUInt32();

            if (magic != HardcodedMagic)
                throw new InvalidDataException("Unknown header magic");

            for (uint i = 0; i < entryCount; i++)
                index._entries.Add(IndexEntry.FromData(reader));

            return index;
        }

        public static PackfileIndex FromFile(string filePath)
        {
            using var reader = new BinaryReader(File.OpenRead(filePath));
            return FromData(reader);
        }

        public bool ResolvePathByHash(ulong hash, out string path)
        {
            /*
            if (Entries.TryGetValue(hash, out IndexEntry entry))
            {
                path = entry.FilePath;
                return true;
            }*/

            path = null;
            return false;
        }
    }
}

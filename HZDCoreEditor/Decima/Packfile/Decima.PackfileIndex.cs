using Utility;
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
        public Dictionary<ulong, IndexEntry> Entries { get; private set; }

        private const uint HardcodedMagic = 0x10203040;

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
            public ulong PathHash;
            public BaseGGUUID GUID;

            public void ToData(BinaryWriter writer)
            {
                throw new NotImplementedException();
            }

            public IndexEntry FromData(BinaryReader reader)
            {
                uint pathLength = reader.ReadUInt32();

                if (pathLength > 0)
                {
                    // Strings are expected to be lowercase and prefixed with "cache:"
                    var fullPath = Encoding.UTF8.GetString(reader.ReadBytesStrict(pathLength));
                    FilePath = fullPath.Replace("cache:", "");

                    SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(FilePath + char.MinValue), 42, out ulong[] hash);
                    PathHash = hash[0];
                }

                GUID = new BaseGGUUID().FromData(reader);
                var unknown = reader.ReadBytesStrict(16);

                return this;
            }
        }

        public PackfileIndex FromData(BinaryReader reader)
        {
            uint magic = reader.ReadUInt32();
            uint entryCount = reader.ReadUInt32();

            if (magic != HardcodedMagic)
                throw new InvalidDataException("Unknown header magic");

            for (uint i = 0; i < entryCount; i++)
            {
                var entry = new IndexEntry().FromData(reader);
                Entries[entry.PathHash] = entry;
            }

            return this;
        }

        public PackfileIndex FromFile(string filePath)
        {
            using (var reader = new BinaryReader(File.OpenRead(filePath)))
                return FromData(reader);
        }

        public bool ResolvePathByHash(ulong hash, out string path)
        {
            if (Entries.TryGetValue(hash, out IndexEntry entry))
            {
                path = entry.FilePath;
                return true;
            }

            path = null;
            return false;
        }
    }
}

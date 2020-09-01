using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Decima
{
    // Idx - unused file type. They're present in HZD's data directory but aren't loaded.
    public class PackfileIndex
    {
        public const uint HardcodedMagic = 0x10203040;

        public Dictionary<ulong, IndexEntry> Entries { get; private set; }
        private readonly FileStream IndexFileHandle;

        public class IndexEntry
        {
            public string FilePath;
            public ulong PathHash;
            public byte[] Guid;

            public static IndexEntry FromData(BinaryReader reader)
            {
                var x = new IndexEntry();
                uint pathLength = reader.ReadUInt32();

                if (pathLength > 0)
                {
                    // Strings are expected to be lowercase and prefixed with "cache:"
                    var fullPath = Encoding.UTF8.GetString(reader.ReadBytesStrict(pathLength));
                    x.FilePath = fullPath.Replace("cache:", "");

                    SMHasher.MurmurHash3_x64_128(Encoding.UTF8.GetBytes(x.FilePath + char.MinValue), 42, out ulong[] hash);
                    x.PathHash = hash[0];
                }

                x.Guid = reader.ReadBytesStrict(16);
                var unknown = reader.ReadBytesStrict(16);

                return x;
            }
        }

        public PackfileIndex(string indexPath, FileMode mode = FileMode.Open)
        {
            Entries = new Dictionary<ulong, IndexEntry>();

            if (mode == FileMode.Open)
            {
                IndexFileHandle = File.Open(indexPath, mode, FileAccess.Read, FileShare.Read);

                using (var reader = new BinaryReader(IndexFileHandle, Encoding.UTF8, true))
                {
                    uint magic = reader.ReadUInt32();
                    uint entryCount = reader.ReadUInt32();

                    if (magic != HardcodedMagic)
                        throw new InvalidDataException("Unknown header magic");

                    for (uint i = 0; i < entryCount; i++)
                    {
                        var entry = IndexEntry.FromData(reader);
                        Entries[entry.PathHash] = entry;
                    }
                }
            }
            else if (mode == FileMode.Create || mode == FileMode.CreateNew)
            {
                throw new NotImplementedException("Writing archives is not supported at the moment");
            }
            else
            {
                throw new NotImplementedException("Archive file mode must be Open, Create, or CreateNew");
            }
        }

        ~PackfileIndex()
        {
            IndexFileHandle.Close();
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

using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Decima
{
    class CoreBinary
    {
        /// <remarks>
        /// File data format:
        /// UInt64  (+0)  Type A.K.A. MurmurHash3 of the textual RTTI description
        /// UInt32  (+8)  Chunk size
        /// UInt8[] (+12) (Optional) Chunk data
        /// </remarks>
        public class Entry : RTTI.ISerializable
        {
            public ulong TypeId;
            public ulong ChunkOffset;
            public uint ChunkSize;

            public void Deserialize(BinaryReader reader)
            {
                ChunkOffset = (ulong)reader.BaseStream.Position;

                TypeId = reader.ReadUInt64();
                ChunkSize = reader.ReadUInt32();
            }
        }

        public static List<object> Load(string filePath, bool ignoreUnknownChunks = false)
        {
            var coreFileObjects = new List<object>();

            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    //Console.WriteLine($"Beginning chunk parse at {reader.BaseStream.Position:X}");

                    var entry = RTTI.DeserializeType<Entry>(reader);

                    long currentFilePos = reader.BaseStream.Position;
                    long expectedFilePos = currentFilePos + entry.ChunkSize;

                    if ((currentFilePos + entry.ChunkSize) > reader.StreamRemainder())
                        throw new InvalidDataException($"Invalid chunk size {entry.ChunkSize} was supplied in Core file at offset {currentFilePos:X}");

                    // TODO: This needs to be part of Entry
                    Type topLevelObjectType = RTTI.GetTypeById(entry.TypeId);
                    object topLevelObject = null;

                    if (topLevelObjectType != null)
                    {
                        topLevelObject = RTTI.DeserializeType(reader, topLevelObjectType);
                    }
                    else
                    {
                        if (!ignoreUnknownChunks)
                            throw new InvalidDataException($"Unknown type ID {entry.TypeId:X16} found in Core file at offset {currentFilePos:X}");

                        // Invalid or unknown chunk ID hit - create an array of bytes "object" and try to continue with the rest of the file
                        topLevelObject = reader.ReadBytes(entry.ChunkSize);
                    }

                    if (reader.BaseStream.Position > expectedFilePos)
                        throw new Exception("Read past the end of a chunk while deserializing object");
                    else if (reader.BaseStream.Position < expectedFilePos)
                        throw new Exception("Short read of a chunk while deserializing object");

                    reader.BaseStream.Position = expectedFilePos;
                    coreFileObjects.Add(topLevelObject);
                }
            }

            return coreFileObjects;
        }
    }
}

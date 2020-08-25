using System;
using System.Collections.Generic;
using System.IO;

namespace Decima
{
    class CoreBinary
    {
        public static List<object> Load(string filePath, bool ignoreUnknownChunks = false)
        {
            var coreFileObjects = new List<object>();

            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    Console.WriteLine($"Beginning chunk parse at {reader.BaseStream.Position:X}");

                    var entry = new GameData.CoreBinaryEntry();
                    entry.Deserialize(reader);

                    // TODO: This needs to be part of CoreBinaryEntry
                    object topLevelObject;

                    long currentFilePos = reader.BaseStream.Position;
                    long expectedFilePos = currentFilePos + entry.ChunkSize;

                    if ((currentFilePos + entry.ChunkSize) > reader.BaseStream.Length)
                        throw new Exception($"Invalid chunk size {entry.ChunkSize} was supplied at offset {currentFilePos:X}");

                    if (RTTI.TypeIdLookupMap.TryGetValue(entry.TypeId, out Type topLevelObjectType))
                    {
                        topLevelObject = RTTI.DeserializeType(reader, topLevelObjectType);
                    }
                    else
                    {
                        if (!ignoreUnknownChunks)
                            throw new Exception($"Unknown type ID {entry.TypeId:X16} found in Core file at offset {currentFilePos:X}");

                        // Invalid or unknown chunk ID hit - create an array of bytes "object" and try to continue with the rest of the file
                        topLevelObject = reader.ReadBytes((int)entry.ChunkSize);
                    }

                    if (reader.BaseStream.Position > expectedFilePos)
                        throw new Exception("Read past the end of a chunk while deserializing object");

                    //else if (reader.BaseStream.Position < expectedFilePos)
                    //    throw new Exception("Short read of a chunk while deserializing object");

                    reader.BaseStream.Position = expectedFilePos;
                    coreFileObjects.Add(topLevelObject);
                }
            }

            return coreFileObjects;
        }
    }
}

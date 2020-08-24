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
                    var entry = new GameData.CoreBinaryEntry();
                    entry.Deserialize(reader);

                    long currentFilePos = reader.BaseStream.Position;
                    long expectedFilePos = currentFilePos + entry.ChunkSize;

                    if ((currentFilePos + entry.ChunkSize) > reader.BaseStream.Length)
                        throw new Exception($"Invalid chunk size {entry.ChunkSize} was supplied at offset {currentFilePos}");

                    //Console.WriteLine("Beginning chunk parse at {0:X}", currentFilePos - 8 - 4);

                    Type topLevelObjectType;
                    object topLevelObject;

                    if (RTTI.TypeIdLookupMap.ContainsKey(entry.TypeId))
                    {
                        topLevelObjectType = RTTI.TypeIdLookupMap[entry.TypeId];
                        topLevelObject = Activator.CreateInstance(topLevelObjectType);

                        RTTI.DeserializeType(topLevelObject, null, reader);
                    }
                    else
                    {
                        // Invalid or unknown chunk ID hit - try to continue with the rest of the file
                        if (ignoreUnknownChunks)
                        {
                            // Append as raw binary data
                            topLevelObject = reader.ReadBytes((int)entry.ChunkSize);
                        }
                        else
                        {
                            throw new Exception($"Unknown type ID {entry.TypeId} found in Core file at offset {currentFilePos}");
                        }
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

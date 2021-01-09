using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utility;
using HZDCoreEditor.Util;

namespace Decima
{
    class CoreBinary : IEnumerable<object>
    {
        private List<CoreEntry> Entries;

        /// <remarks>
        /// File data format:
        /// UInt64  (+0)  Type A.K.A. MurmurHash3 of the textual RTTI description
        /// UInt32  (+8)  Chunk size
        /// UInt8[] (+12) (Optional) Chunk data
        /// </remarks>
        private class CoreEntry : RTTI.ISerializable
        {
            public const int DataHeaderSize = 12;// Bytes

            public ulong TypeId;
            public long ChunkOffset;
            public uint ChunkSize;
            public object ContainedObject;

            public void Deserialize(BinaryReader reader)
            {
                ChunkOffset = reader.BaseStream.Position;

                TypeId = reader.ReadUInt64();
                ChunkSize = reader.ReadUInt32();
            }

            public void Serialize(BinaryWriter writer)
            {
                ChunkOffset = writer.BaseStream.Position;

                writer.Write(TypeId);
                writer.Write(ChunkSize);
            }
        }

        public void ToData(BinaryWriter writer)
        {
            foreach (var entry in Entries)
            {
                // Allocate file space for a fake entry with all zeros
                entry.ChunkOffset = writer.BaseStream.Position;
                writer.BaseStream.Position += CoreEntry.DataHeaderSize;

                if (entry.ContainedObject is byte[] asBytes)
                {
                    // Unsupported (raw data) object type
                    writer.Write(asBytes);
                }
                else
                {
                    RTTI.SerializeType(writer, entry.ContainedObject);
                }

                // Now rewrite it with the updated fields
                entry.ChunkSize = (uint)(writer.BaseStream.Position - entry.ChunkOffset - CoreEntry.DataHeaderSize);

                long oldPosition = writer.BaseStream.Position;
                writer.BaseStream.Position = entry.ChunkOffset;
                RTTI.SerializeType(writer, entry);
                writer.BaseStream.Position = oldPosition;
            }
        }

        public void ToFile(string filePath)
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None)))
                ToData(writer);
        }

        public CoreBinary FromData(BinaryReader reader, bool ignoreUnknownChunks = false)
        {
            Entries = new List<CoreEntry>();

            while (reader.StreamRemainder() > 0)
            {
                //Debugger.Log(0, "Info", $"Beginning chunk parse at {reader.BaseStream.Position:X}");

                var entry = RTTI.DeserializeType<CoreEntry>(reader);
                var topLevelObjectType = RTTI.GetTypeById(entry.TypeId);
                long expectedStreamPos = reader.BaseStream.Position + entry.ChunkSize;

                if (entry.ChunkSize > reader.StreamRemainder())
                    throw new InvalidDataException($"Invalid chunk size {entry.ChunkSize} was supplied in Core file at offset {entry.ChunkOffset:X}");

                if (topLevelObjectType != null)
                {
                    entry.ContainedObject = RTTI.DeserializeType(reader, topLevelObjectType);
                }
                else
                {
                    if (!ignoreUnknownChunks)
                        throw new InvalidDataException($"Unknown type ID {entry.TypeId:X16} found in Core file at offset {entry.ChunkOffset:X}");

                    // Invalid or unknown chunk ID hit - create an array of bytes "object" and try to continue with the rest of the file
                    entry.ContainedObject = reader.ReadBytes(entry.ChunkSize);
                }

                // Check for overflows and underflows
                if (reader.BaseStream.Position > expectedStreamPos)
                    throw new Exception("Read past the end of a chunk while deserializing object");
                else if (reader.BaseStream.Position < expectedStreamPos)
                    throw new Exception("Short read of a chunk while deserializing object");

                //if (reader.BaseStream.Position < expectedStreamPos)
                //    Debugger.Log(0, "Warn", $"Short read of a chunk while deserializing object. {reader.BaseStream.Position} < {expectedStreamPos}. TypeId = {entry.TypeId:X16}\n");

                reader.BaseStream.Position = expectedStreamPos;
                Entries.Add(entry);
            }

            return this;
        }

        public CoreBinary FromFile(string filePath, bool ignoreUnknownChunks = false)
        {
            using (var reader = new BinaryReader(File.OpenRead(filePath)))
                return FromData(reader, ignoreUnknownChunks);
        }

        public void AddObject(object obj)
        {
            Entries.Add(new CoreEntry()
            {
                TypeId = RTTI.GetIdByType(obj.GetType()),
                ContainedObject = obj,
            });
        }

        public void RemoveObject(object obj)
        {
            Entries.Remove(Entries.Where(x => x.ContainedObject == obj).Single());
        }

        public List<BaseRef> GetAllReferences()
        {
            var refs = new List<BaseRef>();

            foreach (var entry in Entries)
            {
                VisitObjectTypes(entry.ContainedObject, (BaseRef baseRef) =>
                {
                    refs.Add(baseRef);
                });
            }

            return refs;
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var entry in Entries)
                yield return entry.ContainedObject;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void VisitObjectTypes<T>(object obj, Action<T> memberCallback) where T : class
        {
            if (obj == null)
                return;

            // Ignore all primitives
            var objectType = obj.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            // T, arrays, lists, then any other object
            if (objectType.Inherits(typeof(T)))
            {
                memberCallback(obj as T);
            }
            else if (objectType.IsArray)
            {
                // Skip large byte arrays (pure data)
                if (obj is byte[])
                    return;

                foreach (var arrayObj in (obj as Array))
                    VisitObjectTypes(arrayObj, memberCallback);
            }
            else if (objectType.InheritsGeneric(typeof(List<>)))
            {
                foreach (var listObj in (obj as IList))
                    VisitObjectTypes(listObj, memberCallback);
            }
            else
            {
                // Recursively gather all public and private instance members
                for (var type = objectType; type != null; type = type.BaseType)
                {
                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                        VisitObjectTypes(field.GetValue(obj), memberCallback);
                }
            }
        }
    }
}

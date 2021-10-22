using HZDCoreEditor.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Decima
{
    public class CoreBinary : IEnumerable<object>
    {
        private readonly List<CoreEntry> Entries = new List<CoreEntry>();

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

        public void ToFile(string filePath, FileMode mode = FileMode.CreateNew)
        {
            using var writer = new BinaryWriter(File.Open(filePath, mode, FileAccess.ReadWrite, FileShare.None));
            ToData(writer);
        }

        public static CoreBinary FromData(BinaryReader reader, bool ignoreUnknownChunks = true)
        {
            var core = new CoreBinary();

            while (reader.StreamRemainder() > 0)
            {
#if DEBUG
                System.Diagnostics.Debugger.Log(0, "Info", $"Beginning chunk parse at {reader.BaseStream.Position:X}");
#endif

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

#if DEBUG
                if (reader.BaseStream.Position < expectedStreamPos)
                    System.Diagnostics.Debugger.Log(0, "Warn", $"Short read of a chunk while deserializing object. {reader.BaseStream.Position} < {expectedStreamPos}. TypeId = {entry.TypeId:X16}\n");
#endif

                reader.BaseStream.Position = expectedStreamPos;
                core.Entries.Add(entry);
            }

            return core;
        }

        public static CoreBinary FromFile(string filePath, bool ignoreUnknownChunks = true)
        {
            using var reader = new BinaryReader(File.OpenRead(filePath));
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

        public bool RemoveObject(object obj)
        {
            var idx = Entries.FindIndex(x => x.ContainedObject == obj);

            if (idx < 0)
                return false;

            Entries.RemoveAt(idx);
            return true;
        }

        public int RemoveAll(Predicate<object> filter)
        {
            return Entries.RemoveAll(x => filter(x.ContainedObject));
        }

        public List<BaseRef> GetAllReferences()
        {
            var refs = new List<BaseRef>();

            VisitAllObjects<BaseRef>((baseRef, _) =>
            {
                refs.Add(baseRef);
            });

            return refs;
        }

        public void VisitAllObjects<T>(Action<T, object> memberCallback) where T : class
        {
            foreach (var entry in Entries)
                VisitObjectTypes(this, entry.ContainedObject, memberCallback);
        }

        private void VisitObjectTypes<T>(object parent, object obj, Action<T, object> memberCallback) where T : class
        {
            if (obj == null)
                return;

            var objectType = obj.GetType();

            // T, arrays, lists, then any other object
            if (objectType.Inherits(typeof(T)))
            {
                memberCallback(obj as T, parent);
            }
            else if (objectType.IsArray)
            {
                // Skip large primitive arrays (pure data)
                if (!objectType.GetElementType().IsClass)
                    return;

                foreach (var arrayObj in (obj as Array))
                    VisitObjectTypes(obj, arrayObj, memberCallback);
            }
            else if (objectType.InheritsGeneric(typeof(List<>)))
            {
                // Same as above
                if (!objectType.GetGenericArguments()[0].IsClass)
                    return;

                foreach (var listObj in (obj as IList))
                    VisitObjectTypes(obj, listObj, memberCallback);
            }
            else
            {
                // Recursively gather all public and private instance members
                for (var type = objectType; type != null; type = type.BaseType)
                {
                    // ...but ignore any inherited System types
                    if (type.Namespace.StartsWith("System"))
                        break;

                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                        VisitObjectTypes(obj, field.GetValue(obj), memberCallback);
                }
            }
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
    }
}

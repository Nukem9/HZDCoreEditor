using HZDCoreEditor.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Decima
{
    public class CoreBinary
    {
        public IEnumerable<object> Objects => _entries.Select(x => x.ContainedObject);

        private readonly List<CoreEntry> _entries = new List<CoreEntry>();

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

        /// <summary>
        /// Serialize all contained objects to a BinaryWriter.
        /// </summary>
        /// <param name="writer">Destination stream</param>
        public void ToData(BinaryWriter writer)
        {
            foreach (var entry in _entries)
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

        /// <summary>
        /// Serialize all contained objects to a file on disk.
        /// </summary>
        /// <param name="filePath">Disk path</param>
        /// <param name="mode">File overwrite mode</param>
        public void ToFile(string filePath, FileMode mode = FileMode.CreateNew)
        {
            using var writer = new BinaryWriter(File.Open(filePath, mode, FileAccess.ReadWrite, FileShare.None));
            ToData(writer);
        }

        /// <summary>
        /// Deserialize a set of core objects from a BinaryReader.
        /// </summary>
        /// <param name="reader">Source stream</param>
        /// <param name="ignoreUnknownChunks">If set to true and an unknown RTTI type is encountered, read the object as
        /// an array of bytes. Otherwise, raise an exception.</param>
        public static CoreBinary FromData(BinaryReader reader, bool ignoreUnknownChunks = true)
        {
            var core = new CoreBinary();

            while (reader.StreamRemainder() > 0)
            {
#if COREFILE_DEBUG
                System.Diagnostics.Debugger.Log(0, "Info", $"Beginning chunk parse at {reader.BaseStream.Position:X}\n");
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

#if COREFILE_DEBUG
                if (reader.BaseStream.Position < expectedStreamPos)
                    System.Diagnostics.Debugger.Log(0, "Warn", $"Short read of a chunk while deserializing object. {reader.BaseStream.Position} < {expectedStreamPos}. TypeId = {entry.TypeId:X16}\n");
#endif

                // Check for overflows and underflows
                if (reader.BaseStream.Position > expectedStreamPos)
                    throw new Exception("Read past the end of a chunk while deserializing object");
                else if (reader.BaseStream.Position < expectedStreamPos)
                    throw new Exception("Short read of a chunk while deserializing object");

                reader.BaseStream.Position = expectedStreamPos;
                core._entries.Add(entry);
            }

            return core;
        }

        /// <summary>
        /// Deserialize a set of core objects from a file on disk.
        /// </summary>
        /// <param name="filePath">Disk path</param>
        /// <param name="ignoreUnknownChunks">If set to true and an unknown RTTI type is encountered, read the object as
        /// an array of bytes. Otherwise, raise an exception.</param>
        public static CoreBinary FromFile(string filePath, bool ignoreUnknownChunks = true)
        {
            using var reader = new BinaryReader(File.OpenRead(filePath));
            return FromData(reader, ignoreUnknownChunks);
        }

        /// <summary>
        /// Add an object.
        /// </summary>
        /// <param name="obj">Object to use. This object's type must be registered with RTTI.</param>
        public void AddObject(object obj)
        {
            _entries.Add(new CoreEntry()
            {
                TypeId = RTTI.GetIdByType(obj.GetType()),
                ContainedObject = obj,
            });
        }

        /// <summary>
        /// Remove an object.
        /// </summary>
        /// <param name="obj">Object to search for.</param>
        /// <returns>True if the object was succesfully removed.</returns>
        public bool RemoveObject(object obj)
        {
            var idx = _entries.FindIndex(x => x.ContainedObject == obj);

            if (idx < 0)
                return false;

            _entries.RemoveAt(idx);
            return true;
        }

        /// <summary>
        /// Remove all objects that match the filter.
        /// </summary>
        /// <param name="filter">Filter that can return true to remove a specific object.</param>
        /// <returns>Number of object removed.</returns>
        public int RemoveAllObjects(Predicate<object> filter)
        {
            return _entries.RemoveAll(x => filter(x.ContainedObject));
        }

        /// <summary>
        /// Recursively iterate over all stored objects and their class members to gather BaseRef instances.
        /// </summary>
        public List<BaseRef> GetAllReferences()
        {
            var refs = new List<BaseRef>();

            VisitAllObjects<BaseRef>((baseRef, _) =>
            {
                refs.Add(baseRef);
            });

            return refs;
        }

        /// <summary>
        /// Recursively iterate over all stored objects and their class members to gather T instances.
        /// </summary>
        /// <param name="memberCallback">Callback used for each T instance and its parent object.</param>
        public void VisitAllObjects<T>(Action<T, object> memberCallback) where T : class
        {
            foreach (var entry in _entries)
                VisitObjectTypes(this, entry.ContainedObject, memberCallback);
        }

        private void VisitObjectTypes<T>(object parent, object obj, Action<T, object> memberCallback) where T : class
        {
            if (obj == null)
                return;

            var objectType = obj.GetType();

            // T, arrays, lists, dictionaries, then any other object
            if (obj is T asType)
            {
                memberCallback(asType, parent);
            }
            else if (objectType.IsArray)
            {
                // Skip large primitive arrays (pure data)
                if (!objectType.GetElementType().IsClass)
                    return;

                foreach (var arrayObj in (obj as Array))
                    VisitObjectTypes(obj, arrayObj, memberCallback);
            }
            else if (obj is IList asList)
            {
                foreach (var listObj in asList)
                {
                    // Same as above
                    if (!listObj.GetType().IsClass)
                        break;

                    VisitObjectTypes(obj, listObj, memberCallback);
                }
            }
            else if (obj is IDictionary asDictionary)
            {
                foreach (var dictKey in asDictionary.Keys)
                {
                    // Same as above
                    if (!dictKey.GetType().IsClass)
                        break;

                    VisitObjectTypes(obj, dictKey, memberCallback);
                }

                foreach (var dictValue in asDictionary.Values)
                {
                    // Same as above
                    if (!dictValue.GetType().IsClass)
                        break;

                    VisitObjectTypes(obj, dictValue, memberCallback);
                }
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
    }
}

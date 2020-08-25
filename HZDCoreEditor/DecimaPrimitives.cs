using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace Decima
{
    static partial class GameData
    {
        /// <remarks>
        /// File data format:
        /// UInt64  (+0)  Type A.K.A. MurmurHash3 of the textual RTTI descriptor
        /// UInt32  (+8)  Chunk size
        /// UInt8[] (+12) (Optional) Chunk data
        /// </remarks>
        public class CoreBinaryEntry : RTTI.ISerializable
        {
            public ulong TypeId;
            public uint ChunkSize;

            public void Deserialize(BinaryReader reader)
            {
                TypeId = reader.ReadUInt64();
                ChunkSize = reader.ReadUInt32();
            }
        }

        /// <remarks>
        /// File data format:
        /// UInt8  (+0)  Type
        /// GGUUID (+1)  (Optional) UUID
        /// String (+17) (Optional) External resource path
        /// </remarks>
        [DebuggerDisplay("{DebugDisplay,nq}")]
        public class Ref<T> : RTTI.ISerializable
        {
            public Guid Handle;
            public Filename ExternalFile;
            public Types Type;

            public enum Types
            {
                Null = 0,
                LocalCoreUUID = 1,
                ExternalCoreUUID = 2,
                StreamingRef = 3,
                // Unknown4 = 4,
                UUIDRef = 5,
            }

            public void Deserialize(BinaryReader reader)
            {
                Type = (Types)reader.ReadByte();

                switch (Type)
                {
                    case Types.Null:
                        break;

                    case Types.LocalCoreUUID:
                    case Types.UUIDRef:
                        Handle = new Guid(reader.ReadBytes(16));
                        break;

                    case Types.ExternalCoreUUID:
                    case Types.StreamingRef:
                        Handle = new Guid(reader.ReadBytes(16));

                        // This could be a regular String instance - no way to determine the type
                        ExternalFile = new Filename();
                        ExternalFile.Deserialize(reader);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            private string DebugDisplay
            {
                get
                {
                    switch (Type)
                    {
                        case Types.Null:
                            return "Ref<Null>";

                        case Types.LocalCoreUUID:
                        case Types.UUIDRef:
                            return $"Ref<Local> {{{Handle}}}";

                        case Types.ExternalCoreUUID:
                        case Types.StreamingRef:
                            return $"Ref<Extern> {{{ExternalFile.Value}, {Handle}}}";
                    }

                    throw new NotImplementedException();
                }
            }
        }

        public class StreamingRef<T> : Ref<T>
        {
        }

        public class UUIDRef<T> : Ref<T>
        {
        }

        public class Ptr<T> : Ref<T>
        {
        }

        public class WeakPtr<T> : RTTI.ISerializable
        {
        }

        /// <remarks>
        /// File data format:
        /// UInt32 (+0) Item count
        /// T[]    (+4) (Optional) Array items
        /// </remarks>
        public class Array<T> : List<T>, RTTI.ISerializable
        {
            public void Deserialize(BinaryReader reader)
            {
                uint itemCount = reader.ReadUInt32();

                if (itemCount > reader.BaseStream.Length)
                    throw new Exception("Array item count is out of bounds");

                for (int i = 0; i < itemCount; i++)
                {
                    var newObj = RTTI.DeserializeType<T>(reader);

                    Add(newObj);
                }
            }
        }

        [DebuggerDisplay("{Value}")]
        public class HashMap<T> : RTTI.ISerializable
        {
            Dictionary<uint, T> Value;

            public void Deserialize(BinaryReader reader)
            {
                var itemCount = reader.ReadUInt32();
                var items = new Dictionary<uint, T>();

                for (uint i = 0; i < itemCount; i++)
                {
                    var unknown = reader.ReadUInt32();
                    var newObj = RTTI.DeserializeType<T>(reader);

                    items.Add(unknown, newObj);
                }

                Value = items;
            }
        }

        [DebuggerDisplay("{Value}")]
        public class HashSet<T> : RTTI.ISerializable
        {
            Dictionary<uint, T> Value;

            public void Deserialize(BinaryReader reader)
            {
                var itemCount = reader.ReadUInt32();
                var setItems = new Dictionary<uint, T>();

                if (itemCount > reader.BaseStream.Length)
                    throw new Exception("HashSet item count is out of bounds");

                if (itemCount > 1)
                    throw new Exception("Found a hash set with more than 1 entry! Debug this.");

                for (uint i = 0; i < itemCount; i++)
                {
                    uint entryHash = reader.ReadUInt32();
                    var newObj = RTTI.DeserializeType<T>(reader);

                    setItems.Add(entryHash, newObj);
                }

                Value = setItems;
            }
        }

        /// <remarks>
        /// File data format:
        /// UInt32  (+0) String length
        /// UInt32  (+4) Case sensitive CRC32-C hash
        /// UInt8[] (+8) String data
        /// </remarks>
        [DebuggerDisplay("{Value}")]
        public class String : RTTI.ISerializable
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Value;

            public void Deserialize(BinaryReader reader)
            {
                uint stringLength = reader.ReadUInt32();

                if (stringLength > reader.BaseStream.Length)
                    throw new Exception("String is out of bounds");

                if (stringLength > 0)
                {
                    uint hash = reader.ReadUInt32();
                    byte[] data = reader.ReadBytes((int)stringLength * sizeof(byte));

                    if (stringLength != data.Length)
                        throw new Exception("Short read while trying to load string");

                    Value = Encoding.UTF8.GetString(data);
                }
            }
        }

        public class Filename : String
        {
        }

        /// <remarks>
        /// File data format:
        /// UInt32   (+0) String length
        /// UInt32   (+4) Case sensitive CRC32-C hash
        /// UInt16[] (+8) String data
        /// </remarks>
        [DebuggerDisplay("{Value}")]
        public class WString : RTTI.ISerializable
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Value;

            public void Deserialize(BinaryReader reader)
            {
                uint stringLength = reader.ReadUInt32();

                if (stringLength > 0)
                {
                    uint hash = reader.ReadUInt32();
                    byte[] data = reader.ReadBytes((int)stringLength * sizeof(ushort));

                    if (stringLength != data.Length)
                        throw new Exception("Short read while trying to load string");

                    throw new NotImplementedException();
                }
            }
        }

        [DebuggerDisplay("{Value}")]
        public class uint128 : RTTI.ISerializable
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public BigInteger Value;

            public void Deserialize(BinaryReader reader)
            {
                Value = new BigInteger(reader.ReadBytes(16));
            }
        }
    }
}

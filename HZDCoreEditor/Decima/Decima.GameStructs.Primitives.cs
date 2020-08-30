using BinaryStreamExtensions;
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
        /// UInt8  (+0)  Type
        /// GGUUID (+1)  (Optional) UUID
        /// String (+17) (Optional) External resource path
        /// </remarks>
        [DebuggerDisplay("{DebugDisplay,nq}")]
        public class Ref<T> : RTTI.ISerializable
        {
            public Types Type;
            public Guid Handle;
            public Filename ExternalFile;

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
                        Handle = new Guid(reader.ReadBytesStrict(16));
                        break;

                    case Types.ExternalCoreUUID:
                    case Types.StreamingRef:
                        Handle = new Guid(reader.ReadBytesStrict(16));

                        // This could be a regular String instance - no way to determine the type
                        ExternalFile = new Filename();
                        ExternalFile.Deserialize(reader);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

                if (itemCount > reader.StreamLength())
                    throw new Exception("Array item count is out of bounds");

                if (typeof(T) == typeof(byte))
                {
                    // Avoid wasting time on large arrays
                    if (itemCount > 0)
                        (this as List<byte>).AddRange(reader.ReadBytes(itemCount));
                }
                else
                {
                    for (uint i = 0; i < itemCount; i++)
                    {
                        var newObj = RTTI.DeserializeType<T>(reader);

                        Add(newObj);
                    }
                }
            }
        }

        public class GlobalRenderVariableInfo_GLOBAL_RENDER_VAR_COUNT<T> : Array<T>
        {
        }

        public class float_GLOBAL_RENDER_VAR_COUNT<T> : Array<T>
        {
        }

        public class uint64_PLACEMENT_LAYER_MASK_SIZE<T> : Array<T>
        {
        }

        public class uint16_PBD_MAX_SKIN_WEIGHTS<T> : Array<T>
        {
        }

        public class uint8_PBD_MAX_SKIN_WEIGHTS<T> : Array<T>
        {
        }

        public class ShaderProgramResourceSet_36<T> : Array<T>
        {
        }

        /// <remarks>
        /// File data format:
        /// UInt32        (+0) Item count
        /// <UInt32, T>[] (+4) (Optional) Array items
        /// </remarks>
        [DebuggerDisplay("{Value}")]
        public class HashMap<T> : Dictionary<uint, T>, RTTI.ISerializable
        {
            public void Deserialize(BinaryReader reader)
            {
                uint itemCount = reader.ReadUInt32();

                for (uint i = 0; i < itemCount; i++)
                {
                    uint entryHash = reader.ReadUInt32();
                    var newObj = RTTI.DeserializeType<T>(reader);

                    // TODO: is unknown actually a hash?
                    Add(entryHash, newObj);
                }
            }
        }

        /// <remarks>
        /// File data format:
        /// UInt32        (+0) Item count
        /// <UInt32, T>[] (+4) (Optional) Array items
        /// </remarks>
        [DebuggerDisplay("{Value}")]
        public class HashSet<T> : Dictionary<uint, T>, RTTI.ISerializable
        {
            public void Deserialize(BinaryReader reader)
            {
                uint itemCount = reader.ReadUInt32();

                if (itemCount > reader.StreamLength())
                    throw new Exception("HashSet item count is out of bounds");

                for (uint i = 0; i < itemCount; i++)
                {
                    uint entryHash = reader.ReadUInt32();
                    var newObj = RTTI.DeserializeType<T>(reader);

                    // TODO: is entryHash actually a hash?
                    Add(entryHash, newObj);
                }
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
                uint readLength = reader.ReadUInt32() * sizeof(byte);

                if (readLength > reader.StreamLength())
                    throw new Exception("String is out of bounds");

                if (readLength > 0)
                {
                    uint hash = reader.ReadUInt32();
                    byte[] data = reader.ReadBytesStrict(readLength);

                    Value = Encoding.UTF8.GetString(data);
                }
            }
        }

        /// <summary>
        /// Alias for a regular String
        /// </summary>
        public class Filename : String
        {
        }

        /// <remarks>
        /// File data format:
        /// UInt32   (+0) String length
        /// UInt16[] (+4) String data
        /// </remarks>
        [DebuggerDisplay("{Value}")]
        public class WString : RTTI.ISerializable
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Value;

            public void Deserialize(BinaryReader reader)
            {
                uint readLength = reader.ReadUInt32() * sizeof(ushort);

                if (readLength > reader.StreamLength())
                    throw new Exception("String is out of bounds");

                if (readLength > 0)
                {
                    byte[] data = reader.ReadBytesStrict(readLength);

                    Value = Encoding.Unicode.GetString(data);
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
                Value = new BigInteger(reader.ReadBytesStrict(16));
            }
        }
    }
}

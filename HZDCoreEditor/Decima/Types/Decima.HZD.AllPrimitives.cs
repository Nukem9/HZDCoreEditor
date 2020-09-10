using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace Decima.HZD
{
    /// <remarks>
    /// File data format:
    /// UInt8  (+0)  Type
    /// GGUUID (+1)  (Optional) UUID
    /// String (+17) (Optional) External resource path
    /// </remarks>
    [DebuggerDisplay("{DebugDisplay,nq}")]
    public class Ref<T> : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public Types Type;
        public GGUUID GUID;
        public String ExternalFile;
        private object ResolvedObject;

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
                    GUID = GGUUID.FromData(reader);
                    break;

                case Types.ExternalCoreUUID:
                case Types.StreamingRef:
                    GUID = GGUUID.FromData(reader);

                    // This could be a Filename instance - no way to determine the type
                    ExternalFile = new String();
                    ExternalFile.Deserialize(reader);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public virtual void DeserializeStateObject(SaveDataSerializer serializer)
        {
            ResolvedObject = serializer.ReadObjectHandle();

            if (ResolvedObject != null)
                Type = Types.LocalCoreUUID;// Not entirely correct...
            else
                Type = Types.Null;
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
                        return $"Ref<Local> {{{GUID}}}";

                    case Types.ExternalCoreUUID:
                    case Types.StreamingRef:
                        return $"Ref<Extern> {{'{ExternalFile.Value}', {GUID}}}";
                }

                throw new NotImplementedException();
            }
        }
    }

    public class StreamingRef<T> : Ref<T>
    {
        public override void DeserializeStateObject(SaveDataSerializer serializer)
        {
            Type = Types.StreamingRef;
            ExternalFile = serializer.ReadIndexedString();
            GUID = serializer.ReadIndexedGUID();

            // if not zero, calls something into the streaming manager
            byte unknown = serializer.Reader.ReadByte();
        }
    }

    public class UUIDRef<T> : Ref<T>
    {
    }

    public class Ptr<T> : Ref<T>
    {
    }

    public class WeakPtr<T> : RTTI.ISerializable, RTTI.ISaveSerializable
    {
    }

    /// <remarks>
    /// File data format:
    /// UInt32 (+0) Item count
    /// T[]    (+4) (Optional) Array items
    /// </remarks>
    public class Array<T> : List<T>, RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public Array() : base()
        {
        }

        public Array(int capacity) : base(capacity)
        {
        }

        public void Deserialize(BinaryReader reader)
        {
            uint itemCount = reader.ReadUInt32();

            if (itemCount > reader.StreamRemainder())
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

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            int itemCount = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < itemCount; i++)
            {
                var newObj = serializer.DeserializeType<T>();

                Add(newObj);
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
    public class HashMap<T> : Dictionary<uint, T>, RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public void Deserialize(BinaryReader reader)
        {
            uint itemCount = reader.ReadUInt32();

            if (itemCount > reader.StreamRemainder())
                throw new Exception("HashMap item count is out of bounds");

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
    public class HashSet<T> : Dictionary<uint, T>, RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public void Deserialize(BinaryReader reader)
        {
            uint itemCount = reader.ReadUInt32();

            if (itemCount > reader.StreamRemainder())
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
    public class String : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Value;

        public String()
        {
        }

        public String(string value)
        {
            Value = value;
        }

        public void Deserialize(BinaryReader reader)
        {
            uint readLength = reader.ReadUInt32() * sizeof(byte);

            if (readLength > reader.StreamRemainder())
                throw new Exception("String is out of bounds");

            if (readLength > 0)
            {
                uint hash = reader.ReadUInt32();
                byte[] data = reader.ReadBytesStrict(readLength);

                Value = Encoding.UTF8.GetString(data);
            }
        }

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            Value = serializer.ReadIndexedString();
        }

        // TODO: Implicit operators might not be the best idea
        public static implicit operator string(String value)
        {
            return value.Value;
        }

        public static implicit operator String(string value)
        {
            return new String(value);
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
    public class WString : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Value;

        public void Deserialize(BinaryReader reader)
        {
            uint readLength = reader.ReadUInt32() * sizeof(ushort);

            if (readLength > reader.StreamRemainder())
                throw new Exception("String is out of bounds");

            if (readLength > 0)
            {
                byte[] data = reader.ReadBytesStrict(readLength);

                Value = Encoding.Unicode.GetString(data);
            }
        }

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            Value = serializer.ReadIndexedWideString();
        }
    }

    [DebuggerDisplay("{Value}")]
    public class uint128 : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BigInteger Value;

        public void Deserialize(BinaryReader reader)
        {
            Value = new BigInteger(reader.ReadBytesStrict(16));
        }
    }
}

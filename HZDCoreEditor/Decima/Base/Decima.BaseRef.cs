using System;
using System.Diagnostics;
using System.IO;

namespace Decima
{
    /// <remarks>
    /// File data format:
    /// UInt8  (+0)  Type
    /// GGUUID (+1)  (Optional) UUID
    /// String (+17) (Optional) External resource path
    /// </remarks>
    [DebuggerDisplay("{ToString(),nq}")]
    public class BaseRef : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public Types Type;
        public BaseGGUUID GUID;
        public BaseString ExternalFile;
        private object ResolvedObject;
        private Type ObjectType;

        public enum Types
        {
            Null = 0,
            LocalCoreUUID = 1,
            ExternalCoreUUID = 2,
            StreamingRef = 3,
            // Unknown4 = 4,
            UUIDRef = 5,
        }

        public BaseRef(Type objectType)
        {
            ObjectType = objectType;
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
                    GUID = new BaseGGUUID().FromData(reader);
                    break;

                case Types.ExternalCoreUUID:
                case Types.StreamingRef:
                    GUID = new BaseGGUUID().FromData(reader);

                    // This could be a Filename instance - no way to determine the type
                    ExternalFile = new BaseString();
                    ExternalFile.Deserialize(reader);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Type);

            switch (Type)
            {
                case Types.Null:
                    break;

                case Types.LocalCoreUUID:
                case Types.UUIDRef:
                    GUID.ToData(writer);
                    break;

                case Types.ExternalCoreUUID:
                case Types.StreamingRef:
                    GUID.ToData(writer);

                    ExternalFile.Serialize(writer);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public virtual void DeserializeStateObject(SaveState state)
        {
            ResolvedObject = state.ReadObjectHandle();

            if (ResolvedObject != null)
                Type = Types.LocalCoreUUID;// Not entirely correct...
            else
                Type = Types.Null;
        }

        public virtual void SerializeStateObject(SaveState state) => throw new NotImplementedException();

        public override string ToString()
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
                    return $"Ref<Extern> {{'{ExternalFile}', {GUID}}}";
            }

            throw new NotImplementedException();
        }
    }

    public class BaseStreamingRef<T> : BaseRef
    {
        public BaseStreamingRef() : base(typeof(T))
        {
        }

        public override void DeserializeStateObject(SaveState state)
        {
            Type = Types.StreamingRef;
            ExternalFile = new BaseString(state.ReadIndexedString());
            GUID = state.ReadIndexedGUID();

            // if not zero, calls something into the streaming manager
            byte unknown = state.Reader.ReadByte();
        }
    }

    public class BaseUUIDRef<T> : BaseRef
    {
        public BaseUUIDRef() : base(typeof(T))
        {
        }
    }

    public class BaseCPtr<T> : BaseRef
    {
        public BaseCPtr() : base(typeof(T))
        {
        }
    }

    public class BaseWeakPtr<T> : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        // Unimplemented
        public void Deserialize(BinaryReader reader) => throw new NotImplementedException();
        public void Serialize(BinaryWriter writer) => throw new NotImplementedException();
        public void DeserializeStateObject(SaveState state) => throw new NotImplementedException();
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}

using System;
using System.IO;

namespace Decima
{
    static partial class RTTI
    {
        /// <summary>
        /// Describes a class, struct, or enum that is serialized as Core binary data using reflection
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
        public class SerializableAttribute : Attribute
        {
            public readonly ulong BinaryTypeId;
            public readonly bool IsPrimitiveType;

            public SerializableAttribute(ulong binaryTypeId, bool isPrimitiveType = false)
            {
                BinaryTypeId = binaryTypeId;
                IsPrimitiveType = isPrimitiveType;
            }
        }

        /// <summary>
        /// Describes a class member that is serialized as Core binary data
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class MemberAttribute : Attribute
        {
            public readonly uint Order;
            public readonly uint RuntimeOffset;
            public readonly string Category;
            public readonly bool IgnoreBinarySerialization;

            public MemberAttribute(uint order, uint runtimeOffset, string category = "", bool ignoreBinarySerialization = false)
            {
                Order = order;
                RuntimeOffset = runtimeOffset;
                Category = category;
                IgnoreBinarySerialization = ignoreBinarySerialization;
            }

            public MemberAttribute(uint order, uint runtimeOffset, bool ignoreBinarySerialization) : this(order, runtimeOffset, "", ignoreBinarySerialization)
            {
            }
        }

        /// <summary>
        /// Describes a class member that is emulating C++ multiple base class inheritance
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class BaseClassAttribute : MemberAttribute
        {
            public BaseClassAttribute(uint runtimeOffset) : base(0, runtimeOffset, null)
            {
            }
        }

        /// <summary>
        /// Interface used for classes that manually encode/decode all of their fields
        /// </summary>
        public interface ISerializable
        {
            public void Deserialize(BinaryReader reader) => throw new NotImplementedException();

            public void Serialize(BinaryWriter writer) => throw new NotImplementedException();
        }

        /// <summary>
        /// Interface used for classes that have data appended after the serialized structure (Equivalent to MsgReadBinary)
        /// </summary>
        public interface IExtraBinaryDataCallback
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
            }

            public void SerializeExtraData(BinaryWriter writer) => throw new NotImplementedException();
        }
    }
}
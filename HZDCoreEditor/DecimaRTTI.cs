using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Decima
{
    static class RTTI
    {
        public static readonly Dictionary<ulong, Type> TypeIdLookupMap;

        /// <summary>
        /// Describes a class, struct, or enum that is serialized as Core binary data
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

            public MemberAttribute(uint order, uint runtimeOffset, string category = "")
            {
                Order = order;
                RuntimeOffset = runtimeOffset;
                Category = category;
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

        public interface ISerializable
        {
            public void Deserialize(BinaryReader reader) => throw new NotImplementedException();

            public void Serialize(BinaryWriter writer) => throw new NotImplementedException();
        }

        public interface IExtraBinaryDataCallback
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                //throw new NotImplementedException();
            }

            public void SerializeExtraData(BinaryWriter writer) => throw new NotImplementedException();
        }

        static RTTI()
        {
            // Build a cache of the 64-bit type IDs to actual C# types
            TypeIdLookupMap = new Dictionary<ulong, Type>();

            foreach (var classType in typeof(SerializableAttribute).Assembly.GetTypes())
            {
                var attribute = classType.GetCustomAttribute<SerializableAttribute>();

                if (attribute != null)
                    TypeIdLookupMap.Add(attribute.BinaryTypeId, classType);
            }
        }

        public static T DeserializeType<T>(BinaryReader reader)
        {
            return (T)DeserializeType(reader, typeof(T));
        }

        public static object DeserializeType(BinaryReader reader, Type type)
        {
            // Enums are essentially trivial types
            if (type.IsEnum)
            {
                if (!TryReadTrivialType(reader, type.GetEnumUnderlyingType(), out object enumValue))
                    throw new Exception("Failed to handle underlying enum type");

                return enumValue;
            }

            // Actual trivial types
            if (TryReadTrivialType(reader, type, out object trivialValue))
                return trivialValue;

            // Classes and structs
            if (type.IsClass || type.IsValueType)
            {
                var newObj = Activator.CreateInstance(type);

                if (newObj is ISerializable asSerializable)
                {
                    // Custom deserialization function implemented. Let the interface do the work.
                    asSerializable.Deserialize(reader);
                }
                else
                {
                    //
                    // This tries to replicate HZD's sorting mechanism. Rebuild class member hierarchy for a few reasons:
                    //
                    // - C# doesn't allow multiple inheritance / multiple base classes.
                    // - C# doesn't expose private fields from base classes with ParentType.GetFields().
                    // - Things are not well defined when multiple fields are declared at offset 0. Their reflection parser is buggy.
                    //
                    // All [RTTI.Member()] fields are enumerated and sorted by offset, order, and most complex class type. Multiple
                    // inheritance offsets are handled by the dumping code ([RTTI.BaseClass()]) so it doesn't need to be taken into account.
                    //
                    // Test: AIDynamicObstacleRectangleResource members are out of order and overlap the base (AIDynamicObstacleResource)
                    // Test: CubemapZone emulated MI (Shape2DExtrusion)
                    //
                    var allFields = new List<(MemberAttribute Attr, uint Offset, uint ClassOrder, FieldInfo Field)>();
                    uint classIndex = 0;

                    void addFieldsRecursively(Type classType, uint offset = 0)
                    {
                        // Drill down until System.Object is hit
                        for (; classType != null; classType = classType.BaseType)
                        {
                            if (!classType.IsDefined(typeof(SerializableAttribute)))
                                continue;

                            classIndex++;
                            var fields = classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                            foreach (var field in fields)
                            {
                                var baseClassAttr = field.GetCustomAttribute<BaseClassAttribute>();
                                var reflectionAttr = field.GetCustomAttribute<MemberAttribute>();

                                if (baseClassAttr != null)
                                    addFieldsRecursively(field.FieldType, offset + baseClassAttr.RuntimeOffset);
                                else if (reflectionAttr != null)
                                    allFields.Add((reflectionAttr, offset + reflectionAttr.RuntimeOffset, classIndex, field));
                            }
                        }
                    }

                    // Sort: member offset, member index, class index
                    addFieldsRecursively(type);

                    var finalHierarchy = allFields
                        .OrderBy(x => x.Offset)
                        .ThenBy(x => x.Attr.Order)
                        .ThenByDescending(x => x.ClassOrder)
                        .Select(x => x.Field);

                    foreach (var field in finalHierarchy)
                        DeserializeTypeFromField(null, null, field);
                }

                // Done reading. Now copy what the engine does and notify MsgReadBinary subscribers.
                if (newObj is IExtraBinaryDataCallback asExtraBinaryDataCallback)
                    asExtraBinaryDataCallback.DeserializeExtraData(reader);

                return newObj;
            }

            // Invalid
            return null;
        }

        public static void DeserializeTypeFromField(BinaryReader reader, object instance, FieldInfo field)
        {
            field.SetValue(instance, DeserializeType(reader, field.FieldType));
        }

        private static bool TryReadTrivialType(BinaryReader reader, Type type, out object value)
        {
            bool valid = true;

            // The game always does a direct memory copy for these
            value = Type.GetTypeCode(type) switch
            {
                TypeCode.Boolean => reader.ReadBoolean(),
                TypeCode.SByte => reader.ReadSByte(),
                TypeCode.Byte => reader.ReadByte(),
                TypeCode.Int16 => reader.ReadInt16(),
                TypeCode.UInt16 => reader.ReadUInt16(),
                TypeCode.Int32 => reader.ReadInt32(),
                TypeCode.UInt32 => reader.ReadUInt32(),
                TypeCode.Int64 => reader.ReadInt64(),
                TypeCode.UInt64 => reader.ReadUInt64(),
                TypeCode.Single => reader.ReadSingle(),
                TypeCode.Double => reader.ReadDouble(),
                _ => valid = false,
            };

            return valid;
        }
    }
}

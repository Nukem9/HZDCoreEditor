using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Decima
{
    static class RTTI
    {
        public static readonly Dictionary<ulong, Type> TypeIdLookupMap;

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
        public class SerializableAttribute : Attribute
        {
            public readonly ulong BinaryTypeId;
            public readonly uint RuntimeSizeOf;
            public readonly bool IsPrimitiveType;

            public SerializableAttribute(ulong binaryTypeId, uint runtimeSizeOf, bool isPrimitiveType = false)
            {
                BinaryTypeId = binaryTypeId;
                RuntimeSizeOf = runtimeSizeOf;
                IsPrimitiveType = isPrimitiveType;
            }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class MemberAttribute : Attribute
        {
            public readonly uint RuntimeOffset;
            public readonly uint Order;

            public MemberAttribute(uint runtimeOffset, uint order)
            {
                RuntimeOffset = runtimeOffset;
                Order = order;
            }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class BrokenReflectionOffsetAttribute : Attribute
        {
            public readonly uint RealOffset;

            public BrokenReflectionOffsetAttribute(uint realOffset)
            {
                RealOffset = realOffset;
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

            public void SerializeExtraData(BinaryWriter writer)
            {
            }
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

        private static bool TryReadPrimitiveType(out object value, Type type, BinaryReader reader)
        {
            // The game engine does a direct memory copy for these trivial types
            if (type == typeof(bool))
                value = reader.ReadBoolean();
            else if (type == typeof(sbyte))
                value = reader.ReadSByte();
            else if (type == typeof(byte))
                value = reader.ReadByte();
            else if (type == typeof(short))
                value = reader.ReadInt16();
            else if (type == typeof(ushort))
                value = reader.ReadUInt16();
            else if (type == typeof(int))
                value = reader.ReadInt32();
            else if (type == typeof(uint))
                value = reader.ReadUInt32();
            else if (type == typeof(long))
                value = reader.ReadInt64();
            else if (type == typeof(ulong))
                value = reader.ReadUInt64();
            else if (type == typeof(float))
                value = reader.ReadSingle();
            else if (type == typeof(double))
                value = reader.ReadDouble();
            else
            {
                value = null;
                return false;
            }

            return true;
        }

        public static object TestDeserializeType<T>(BinaryReader reader)
        {
            var baseType = typeof(T);

            if (TryReadPrimitiveType(out object value, baseType, reader))
                return value;
            else
            {
                // Class, structure, or enum. Overwrite the current object instance if no member offset is given.
                var newObj = Activator.CreateInstance(baseType);

                if (newObj is ISerializable)
                {
                    // Custom deserialization function implemented. Let the interface do the work.
                    (newObj as ISerializable).Deserialize(reader);
                }
                else
                {
                    // Manually build the hierarchy since C# doesn't allow you to grab private fields from base classes (Type.GetFields()). This
                    // recursively parses child members & assumes multiple inheritance doesn't exist.
                    var baseClassTypes = new List<Type>();
                    var allStructureFields = new List<FieldInfo>();

                    for (var currentType = baseType; currentType != null; currentType = currentType.BaseType)
                    {
                        if (currentType.IsDefined(typeof(SerializableAttribute)))
                            baseClassTypes.Add(currentType);
                    }

                    for (int i = baseClassTypes.Count - 1; i >= 0; i--)
                    {
                        var fields = baseClassTypes[i].GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        allStructureFields.AddRange(fields);
                    }

                    // Fields that have BrokenReflectionOffsetAttribute must be read first regardless of field order
                    foreach (var field in allStructureFields)
                    {
                        var reflectionFixAttr = field.GetCustomAttribute<BrokenReflectionOffsetAttribute>();

                        if (reflectionFixAttr != null)
                        {
                            if (reflectionFixAttr.RealOffset != 0)
                                throw new Exception("This code doesn't handle non-0 offsets");

                            DeserializeType(newObj, field, reader);
                        }
                    }

                    foreach (var field in allStructureFields)
                    {
                        var reflectionFixAttr = field.GetCustomAttribute<BrokenReflectionOffsetAttribute>();

                        if (reflectionFixAttr == null)
                            DeserializeType(newObj, field, reader);
                    }
                }

                return newObj;
            }

            return null;
        }

        public static void DeserializeType(object instance, FieldInfo info, BinaryReader reader)
        {
            var baseType = info == null ? instance.GetType() : info.FieldType;

            if (baseType.IsEnum)
            {
                if (TryReadPrimitiveType(out object value, baseType.GetEnumUnderlyingType(), reader))
                    info.SetValue(instance, value);
                else
                    throw new Exception("Failed to handle underlying enum type");
            }
            else if (TryReadPrimitiveType(out object value, baseType, reader))
                info.SetValue(instance, value);
            else
            {
                // Class, structure, or enum. Overwrite the current object instance if no member offset is given.
                var newObj = info == null ? instance : Activator.CreateInstance(baseType);

                if (newObj is ISerializable asISerializable)
                {
                    // Custom deserialization function implemented. Let the interface do the work.
                    asISerializable.Deserialize(reader);
                }
                else
                {
                    if (baseType.GetCustomAttribute<SerializableAttribute>() == null)
                        throw new Exception();

                    // Manually build the hierarchy since C# doesn't allow you to grab private fields from base classes (Type.GetFields()). This
                    // assumes multiple inheritance is not used & recursively parses child members.
                    var baseClassTypes = new List<Type>();
                    var allStructureFields = new List<FieldInfo>();

                    for (var currentType = baseType; currentType != null; currentType = currentType.BaseType)
                    {
                        if (currentType.IsDefined(typeof(SerializableAttribute)))
                            baseClassTypes.Add(currentType);
                    }

                    for (int i = baseClassTypes.Count - 1; i >= 0; i--)
                    {
                        var fields = baseClassTypes[i].GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        allStructureFields.AddRange(fields);
                    }

                    // Fields that have BrokenReflectionOffsetAttribute must be read first regardless of field order
                    foreach (var field in allStructureFields)
                    {
                        var reflectionFixAttr = field.GetCustomAttribute<BrokenReflectionOffsetAttribute>();

                        if (reflectionFixAttr != null)
                        {
                            //if (reflectionFixAttr.RealOffset != 0)
                            //    throw new Exception("This code doesn't handle non-0 offsets");

                            DeserializeType(newObj, field, reader);
                        }
                    }

                    foreach (var field in allStructureFields)
                    {
                        var reflectionFixAttr = field.GetCustomAttribute<BrokenReflectionOffsetAttribute>();

                        if (field.Name == "XX_Text")
                            continue;

                        if (reflectionFixAttr == null)
                            DeserializeType(newObj, field, reader);
                    }
                }

                if (newObj is IExtraBinaryDataCallback asIExtraBinaryDataCallback)
                    asIExtraBinaryDataCallback.DeserializeExtraData(reader);

                if (info != null)
                    info.SetValue(instance, newObj);
            }
        }
    }
}

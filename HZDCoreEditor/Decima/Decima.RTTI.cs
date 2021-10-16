using HZDCoreEditor.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Decima
{
    public enum GameType
    {
        DS,     // Death Stranding
        HZD,    // Horizon Zero Dawn
    }

    public static partial class RTTI
    {
        private static Dictionary<ulong, Type> _typeIdLookupMap;
        private static ConcurrentDictionary<Type, OrderedFieldInfo> _typeFieldInfoCache;
        private static readonly Dictionary<string, string> _dotNetTypeToDecima;

        public class OrderedFieldInfo
        {
            public sealed class Entry
            {
                public readonly FieldInfo MIBase;
                public readonly FieldInfo Field;
                public readonly bool SaveStateOnly;

                public Entry(FieldInfo miBase, FieldInfo field, bool saveStateOnly)
                {
                    MIBase = miBase;
                    Field = field;
                    SaveStateOnly = saveStateOnly;
                }

                public void SetValue(object parent, object value)
                {
                    // If using emulated MI: fetch the base class pointer first, then write the field
                    Field.SetValue(MIBase != null ? MIBase.GetValue(parent) : parent, value);
                }

                public object GetValue(object parent)
                {
                    return Field.GetValue(MIBase != null ? MIBase.GetValue(parent) : parent);
                }
            }

            public readonly FieldInfo[] MIBases;
            public readonly Entry[] Members;

            public OrderedFieldInfo(FieldInfo[] bases, Entry[] members)
            {
                MIBases = bases;
                Members = members;
            }
        }

        static RTTI()
        {
            _dotNetTypeToDecima = new Dictionary<string, string>()
            {
                {"Boolean", "bool"},
                {"SByte", "int8"},
                {"Byte", "uint8"},
                {"Int16", "int16"},
                {"UInt16", "uint16"},
                {"Int32", "int"},
                {"UInt32", "uint"},
                {"Int64", "int64"},
                {"UInt64", "uint64"},
                {"Single", "float"},
            };

            SetGameMode(GameType.HZD);
        }

        public static void SetGameMode(GameType game)
        {
            // Build a cache of the 64-bit type IDs to actual C# types. Previous values are erased.
            _typeIdLookupMap = new Dictionary<ulong, Type>();
            _typeFieldInfoCache = new ConcurrentDictionary<Type, OrderedFieldInfo>();

            foreach (var classType in typeof(SerializableAttribute).Assembly.GetTypes())
            {
                var attribute = classType.GetCustomAttribute<SerializableAttribute>();

                if (attribute != null)
                {
                    if (attribute.Game != game)
                        continue;

                    _typeIdLookupMap.Add(attribute.BinaryTypeId, classType);
                }
            }
        }

        public static Type GetTypeByName(string name)
        {
            // Slow
            var type = _typeIdLookupMap.Values
                .Where(x => x.Name == name)
                .Single();

            return type;
        }

        public static Type GetTypeById(ulong typeId)
        {
            if (_typeIdLookupMap.TryGetValue(typeId, out Type objectType))
                return objectType;

            return null;
        }

        public static ulong GetIdByType(Type type)
        {
            return type.GetCustomAttribute<SerializableAttribute>().BinaryTypeId;
        }

        public static string GetTypeNameString(Type type)
        {
            // Array<Ref<PlayerParams>> -> Array`1 -> Array_Ref_PlayerParams
            static string getGenericTypeString(Type genericType)
            {
                string typeName = genericType.Name;

                if (!genericType.IsGenericType)
                {
                    if (_dotNetTypeToDecima.TryGetValue(typeName, out string translatedName))
                        return translatedName;

                    return typeName;
                }

                typeName = typeName.Substring(0, typeName.IndexOf('`'));

                foreach (var argType in genericType.GetGenericArguments())
                    typeName += $"_{getGenericTypeString(argType)}";

                return typeName;
            }

            return getGenericTypeString(type);
        }

        public static string GetFieldCategory(FieldInfo field)
        {
            return field.GetCustomAttribute<MemberAttribute>()?.Category;
        }

        public static string GetFieldName(FieldInfo field)
        {
            string name = field.Name;
            var memberAttr = field.GetCustomAttribute<MemberAttribute>();

            //
            // Decima classes can have duplicated member names under different categories. In order to support it in C#,
            // I had to prefix some variables with "_" or "CATEGORY_". So here's a workaround.
            //
            // Strip the category prefix or "_" prefix for reserved names.
            //
            if (memberAttr?.Category.Length > 0)
                name = name.Replace($"{memberAttr.Category}_", "");
            else if (name.IndexOf('_') == 0)
                name = name.Substring(1);

            return name;
        }

        public static OrderedFieldInfo GetOrderedFieldsForClass(Type type)
        {
            if (_typeFieldInfoCache.TryGetValue(type, out OrderedFieldInfo info))
                return info;

            if (!type.IsDefined(typeof(SerializableAttribute)))
            {
                _typeFieldInfoCache.TryAdd(type, null);
                return null;
            }

            //
            // This insane code tries to replicate HZD's sorting mechanism. Rebuild the class member hierarchy because of a few reasons:
            //
            // - C# doesn't allow multiple inheritance / multiple base classes.
            // - C# doesn't expose private fields from base classes with ParentType.GetFields().
            // - Properties are declared at offset 0. Serialization order is undefined (???) when multiple are declared at offset 0.
            // - Parent class members can overlap base class members.
            //
            // All [RTTI.Member()] fields are enumerated and sorted by offset, order, and most complex class type. Multiple inheritance
            // offsets are handled by the dumping code ([RTTI.BaseClass()]) so it doesn't need to be taken into account.
            //
            // Test: AIDynamicObstacleRectangleResource members are out of order and overlap the base (AIDynamicObstacleResource)
            // Test: CubemapZone emulated MI (Shape2DExtrusion)
            //
            var allFields = new List<(MemberAttribute Attr, uint Offset, uint ClassOrder, FieldInfo MIBase, FieldInfo Field)>();
            uint classIndex = 0;

            void addFieldsRecursively(Type classType, FieldInfo miBase = null, uint offset = 0)
            {
                // Drill down until System.Object is hit
                for (; classType != null; classType = classType.BaseType)
                {
                    classIndex++;
                    var fields = classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                    {
                        var baseClassAttr = field.GetCustomAttribute<BaseClassAttribute>();
                        var reflectionAttr = field.GetCustomAttribute<MemberAttribute>();

                        if (baseClassAttr != null)
                            addFieldsRecursively(field.FieldType, field, offset + baseClassAttr.RuntimeOffset);
                        else if (reflectionAttr != null)
                            allFields.Add((reflectionAttr, offset + reflectionAttr.RuntimeOffset, classIndex, miBase, field));
                    }
                }
            }

            // Sort: class index, { member order | member offset }
            addFieldsRecursively(type);

            var sortedHierarchy = allFields
                .OrderByDescending(x => x.ClassOrder)
                .ThenBy(x => x.Attr.Order)
                .ToArray();

            PCore.Quicksort(sortedHierarchy, (
                (MemberAttribute, uint Offset, uint, FieldInfo, FieldInfo) a,
                (MemberAttribute, uint Offset, uint, FieldInfo, FieldInfo) b) =>
            {
                return a.Offset < b.Offset;
            });

            // Unique base classes
            var miBases = sortedHierarchy
                .Where(x => x.MIBase != null)
                .Select(x => x.MIBase)
                .Distinct()
                .ToArray();

            // All members
            var members = sortedHierarchy
                .Select(x => new OrderedFieldInfo.Entry(x.MIBase, x.Field, x.Attr.SaveStateOnly))
                .ToArray();

            info = new OrderedFieldInfo(miBases, members);

            // Another thread might insert this entry before we do, but it doesn't matter as long as the data is identical
            _typeFieldInfoCache.TryAdd(type, info);
            return info;
        }

        public static T CreateObjectInstance<T>() where T : class
        {
            return (T)CreateObjectInstance(typeof(T));
        }

        public static object CreateObjectInstance(Type type)
        {
            var objectInstance = Activator.CreateInstance(type);
            var info = GetOrderedFieldsForClass(type);

            if (info != null)
            {
                // Instantiate bases
                foreach (var baseClass in info.MIBases)
                    baseClass.SetValue(objectInstance, Activator.CreateInstance(baseClass.FieldType));
            }

            return objectInstance;
        }

        public static T DeserializeType<T>(BinaryReader reader)
        {
            return (T)DeserializeType(reader, typeof(T));
        }

        public static object DeserializeType(BinaryReader reader, Type type)
        {
            // Enums and trivial types
            if (DeserializeTrivialType(reader, type, out object trivialValue))
                return trivialValue;

            // Classes and structs
            if (DeserializeObjectType(reader, type, out object objectValue))
                return objectValue;

            throw new NotImplementedException($"Unhandled object type '{type.FullName}'");
        }

        public static bool DeserializeTrivialType(BinaryReader reader, Type type, out object value)
        {
            bool valid = true;

            // The game always does a direct memory copy for these
            value = Type.GetTypeCode(type) switch
            {
                TypeCode.Boolean => reader.ReadBooleanStrict(),
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

        private static bool DeserializeObjectType(BinaryReader reader, Type type, out object objectInstance)
        {
            if (!type.IsClass && !type.IsValueType)
            {
                objectInstance = null;
                return false;
            }

            objectInstance = CreateObjectInstance(type);

            if (objectInstance is ISerializable asSerializable)
            {
                // Custom deserialization function implemented. Let the interface do the work.
                asSerializable.Deserialize(reader);
            }
            else
            {
                var info = GetOrderedFieldsForClass(type);

                // Read members
                foreach (var member in info.Members)
                {
                    if (member.SaveStateOnly)
                        continue;

                    member.SetValue(objectInstance, DeserializeType(reader, member.Field.FieldType));
                }
            }

            // Done reading. Now copy what the engine does and notify MsgReadBinary subscribers.
            if (objectInstance is IExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.DeserializeExtraData(reader);

            return true;
        }

        public static void SerializeType(BinaryWriter writer, object objectInstance)
        {
            Type type = objectInstance.GetType();

            // Enums and trivial types
            if (SerializeTrivialType(writer, type, objectInstance))
                return;

            // Classes and structs
            if (SerializeObjectType(writer, type, objectInstance))
                return;

            throw new NotImplementedException($"Unhandled object type '{type.FullName}'");
        }

        public static bool SerializeTrivialType(BinaryWriter writer, Type type, object value)
        {
            // TODO: Switch is probably not needed with BinaryWriter.Write() overload
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: writer.Write((bool)value); break;
                case TypeCode.SByte: writer.Write((sbyte)value); break;
                case TypeCode.Byte: writer.Write((byte)value); break;
                case TypeCode.Int16: writer.Write((short)value); break;
                case TypeCode.UInt16: writer.Write((ushort)value); break;
                case TypeCode.Int32: writer.Write((int)value); break;
                case TypeCode.UInt32: writer.Write((uint)value); break;
                case TypeCode.Int64: writer.Write((long)value); break;
                case TypeCode.UInt64: writer.Write((ulong)value); break;
                case TypeCode.Single: writer.Write((float)value); break;
                case TypeCode.Double: writer.Write((double)value); break;
                default: return false;
            }

            return true;
        }

        private static bool SerializeObjectType(BinaryWriter writer, Type type, object objectInstance)
        {
            if (!type.IsClass && !type.IsValueType)
                return false;

            if (objectInstance is ISerializable asSerializable)
            {
                // Custom writer function implemented. Let the interface do the work.
                asSerializable.Serialize(writer);
            }
            else
            {
                var info = GetOrderedFieldsForClass(type);

                // Write members
                foreach (var member in info.Members)
                {
                    if (member.SaveStateOnly)
                        continue;

                    SerializeType(writer, member.GetValue(objectInstance));
                }
            }

            if (objectInstance is IExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.SerializeExtraData(writer);

            return true;
        }
    }
}

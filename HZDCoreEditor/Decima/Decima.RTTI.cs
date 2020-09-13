using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Decima
{
    static partial class RTTI
    {
        private static readonly Dictionary<ulong, Type> TypeIdLookupMap;
        private static readonly Dictionary<Type, OrderedFieldInfo> TypeFieldInfoCache;
        private static readonly Dictionary<string, string> DotNetTypeToDecima;

        public class VirtualRTTIList
        {
            public Type ClassType { get; private set; }
            public IReadOnlyList<OrderedFieldInfo.Entry> ResolvedMembers { get { return _ResolvedMembers.AsReadOnly(); } }

            private readonly List<Entry> Members;
            private readonly List<OrderedFieldInfo.Entry> _ResolvedMembers;

            public struct Entry
            {
                public string Type;
                public string Category;
                public string Name;
            }

            public VirtualRTTIList(string className, int capacity = 0)
            {
                ClassType = GetTypeByName(className);
                Members = new List<Entry>(capacity);
                _ResolvedMembers = new List<OrderedFieldInfo.Entry>();
            }

            public void Add(string type, string category, string name)
            {
                Members.Add(new Entry
                {
                    Type = type,
                    Category = category,
                    Name = name,
                });
            }

            public void ResolveMembersToFieldInfo()
            {
                var info = GetOrderedFieldsForClass(ClassType);

                foreach (var virtualMember in Members)
                {
                    var resolvedMember = info.Members
                        .Where(x => MatchField(x.Field, virtualMember.Type, virtualMember.Category, virtualMember.Name))
                        .Single();

                    _ResolvedMembers.Add(resolvedMember);
                }
            }

            private static bool MatchField(FieldInfo field, string type, string category, string name)
            {
                if (GetFieldCategory(field) != category)
                    return false;

                if (GetFieldName(field) != name)
                    return false;

                string ftn = GetFieldTypeName(field);

                // TODO: Custom int32 type - C# doesn't support typedefs. I can pretend this isn't a problem until I need
                // to write fields.
                if (ftn != "int" && type != "int32")
                {
                    if (!ftn.Equals(type, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                return true;
            }
        }

        public class OrderedFieldInfo
        {
            public struct Entry
            {
                public readonly FieldInfo MIBase;
                public readonly FieldInfo Field;
                public readonly bool IgnoreBinarySerialization;

                public Entry(FieldInfo miBase, FieldInfo field, bool ignoreBinarySerialization)
                {
                    MIBase = miBase;
                    Field = field;
                    IgnoreBinarySerialization = ignoreBinarySerialization;
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
            // Build a cache of the 64-bit type IDs to actual C# types
            TypeIdLookupMap = new Dictionary<ulong, Type>();
            TypeFieldInfoCache = new Dictionary<Type, OrderedFieldInfo>();

            foreach (var classType in typeof(SerializableAttribute).Assembly.GetTypes())
            {
                var attribute = classType.GetCustomAttribute<SerializableAttribute>();

                if (attribute != null)
                    TypeIdLookupMap.Add(attribute.BinaryTypeId, classType);
            }

            DotNetTypeToDecima = new Dictionary<string, string>()
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
        }

        public static Type GetTypeByName(string name)
        {
            var type = TypeIdLookupMap.Values
                .Where(x => x.Name == name)
                .Single();

            return type;
        }

        public static Type GetTypeById(ulong typeId)
        {
            if (TypeIdLookupMap.TryGetValue(typeId, out Type objectType))
                return objectType;

            return null;
        }

        public static ulong GetIdByType(Type type)
        {
            return type.GetCustomAttribute<SerializableAttribute>().BinaryTypeId;
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

        public static string GetFieldTypeName(FieldInfo field)
        {
            // Array<Ref<PlayerParams>> -> Array`1 -> Array_Ref_PlayerParams
            static string getGenericTypeString(Type type)
            {
                string typeName = type.Name;

                if (!type.IsGenericType)
                {
                    if (DotNetTypeToDecima.TryGetValue(typeName, out string translatedName))
                        return translatedName;

                    return typeName;
                }

                typeName = typeName.Substring(0, typeName.IndexOf('`'));

                foreach (var argType in type.GetGenericArguments())
                    typeName += $"_{getGenericTypeString(argType)}";

                return typeName;
            }

            return getGenericTypeString(field.FieldType);
        }

        public static OrderedFieldInfo GetOrderedFieldsForClass(Type type)
        {
            if (TypeFieldInfoCache.TryGetValue(type, out OrderedFieldInfo info))
                return info;

            if (!type.IsDefined(typeof(SerializableAttribute)))
            {
                TypeFieldInfoCache.Add(type, null);
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
                    if (!classType.IsDefined(typeof(SerializableAttribute)))
                        continue;

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

            // Sort: member offset, member index, class index
            addFieldsRecursively(type);

            var sortedHierarchy = allFields
                .OrderBy(x => x.Offset)
                .ThenBy(x => x.Attr.Order)
                .ThenByDescending(x => x.ClassOrder);

            // Unique base classes
            var miBases = sortedHierarchy
                .Where(x => x.MIBase != null)
                .Select(x => x.MIBase)
                .Distinct()
                .ToArray();

            // All members
            var members = sortedHierarchy
                .Select(x => new OrderedFieldInfo.Entry(x.MIBase, x.Field, x.Attr.IgnoreBinarySerialization))
                .ToArray();

            info = new OrderedFieldInfo(miBases, members);
            TypeFieldInfoCache.Add(type, info);

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
                    if (member.IgnoreBinarySerialization)
                        continue;

                    // Check if this field needs to be applied to an emulated base class
                    var baseClass = member.MIBase != null ? member.MIBase.GetValue(objectInstance) : objectInstance;

                    member.Field.SetValue(baseClass, DeserializeType(reader, member.Field.FieldType));
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
                    if (member.IgnoreBinarySerialization)
                        continue;

                    // If using a base class: pull the value out separately, then write it
                    var baseClass = member.MIBase != null ? member.MIBase.GetValue(objectInstance) : objectInstance;

                    SerializeType(writer, member.Field.GetValue(baseClass));
                }
            }

            // Done reading. Now copy what the engine does and notify MsgReadBinary subscribers.
            if (objectInstance is IExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.SerializeExtraData(writer);

            return true;
        }
    }
}

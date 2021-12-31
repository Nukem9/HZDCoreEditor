using HZDCoreEditor.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Decima
{
    public static partial class RTTI
    {
        private static Dictionary<string, string> _dotNetTypeToDecima;
        private static Dictionary<ulong, Type> _typeIdLookupMap;
        private static ConcurrentDictionary<Type, OrderedFieldInfo> _typeFieldInfoCache;

        static RTTI()
        {
            SetGameMode(GameType.HZD);
        }

        public static void SetGameMode(GameType game)
        {
            // Basic aliases
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
            var attribute = type.GetCustomAttribute<SerializableAttribute>();

            if (attribute == null)
                throw new NotSupportedException("This type is unsupported. Objects must have a SerializableAttribute in order to determine a type ID.");

            return attribute.BinaryTypeId;
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

        public static string GetFieldCategory(RttiField field)
        {
            return field.GetCustomAttribute<MemberAttribute>()?.Category;
        }

        public static string GetFieldName(RttiField field)
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

        public static T CreateObjectInstance<T>() where T : class
        {
            return (T)CreateObjectInstance(typeof(T));
        }

        public static object CreateObjectInstance(Type type)
        {
            var objectInstance = Activator.CreateInstance(type);
            var info = GetOrderedFieldsForClass(type, false);

            if (info != null)
            {
                // Instantiate bases
                foreach (var baseClass in info.MIBases)
                    baseClass.SetValue(objectInstance, Activator.CreateInstance(baseClass.Type));
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
                var info = GetOrderedFieldsForClass(type, false);

                // Read members
                foreach (var member in info.Members)
                    member.SetValue(objectInstance, DeserializeType(reader, member.Field.Type));
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
                var info = GetOrderedFieldsForClass(type, false);

                // Write members
                foreach (var member in info.Members)
                    SerializeType(writer, member.GetValue(objectInstance));
            }

            if (objectInstance is IExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.SerializeExtraData(writer);

            return true;
        }

        private static OrderedFieldInfo GetOrderedFieldsForClass(Type type, bool saveState)
        {
            if (saveState)
                throw new NotImplementedException("SaveState RTTI fields are currently unsupported");

            if (_typeFieldInfoCache.TryGetValue(type, out OrderedFieldInfo info))
                return info;

            if (!type.IsDefined(typeof(SerializableAttribute)))
            {
                _typeFieldInfoCache.TryAdd(type, null);
                return null;
            }

            //
            // This insane code tries to replicate Decima's sorting mechanism. Rebuild the class member hierarchy because of a few reasons:
            //
            // - C# doesn't allow multiple inheritance.
            // - C# doesn't expose private fields from base classes with ParentType.GetFields().
            // - C# doesn't have strict ordering for members returned in GetFields().
            // - Save state variables may interfere with PCore.Quicksort order. The upside is that they're not used in the core files. The
            // downside is that we need to decode save games.
            // - Child class members can overlap parent class members.
            // - Decima properties are declared at offset 0. Serialization order is unstable/undefined when multiple are declared at offset 0.
            //
            // All [RTTI.Member()] fields are enumerated and sorted by offset, order, and most complex class type. Multiple inheritance
            // offsets are handled by the dumping code ([RTTI.BaseClass()]) so it doesn't need to be taken into account.
            //
            // Test: HZD.AIDynamicObstacleRectangleResource members are out of order and overlap the base.
            // Test: HZD.CubemapZone emulated MI.
            // Test: DS.IndirectLightingBakeZone order is unstable, emulated MI.
            // Test: DS.LightShadowedRenderVolume order is unstable, emulated MI, and contains save state variables.
            //
            var allFields = new List<(MemberAttribute Attr, uint Offset, uint ClassOrder, RttiField MIBase, RttiField Field)>();
            uint classIndex = 0;

            void addFieldsRecursively(Type classType, RttiField miBase = null, uint offset = 0)
            {
                // Gather each class type in reverse order of inheritance
                var typesToVisit = new List<Type>();

                for (; classType != null; classType = classType.BaseType)
                    typesToVisit.Add(classType);

                typesToVisit.Reverse();

                // Then start from the most basic class
                foreach (var type in typesToVisit)
                {
                    classIndex++;
                    var fields = RttiField.GetMembers(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                    // Emulated MI
                    foreach (var field in fields)
                    {
                        var baseClassAttr = field.GetCustomAttribute<BaseClassAttribute>();

                        if (baseClassAttr != null)
                        {
                            addFieldsRecursively(field.Type, field, offset + baseClassAttr.RuntimeOffset);
                            classIndex++;
                        }
                    }

                    // Real members
                    foreach (var field in fields)
                    {
                        var reflectionAttr = field.GetCustomAttribute<MemberAttribute>();

                        if (reflectionAttr != null && reflectionAttr.GetType() == typeof(MemberAttribute) && !reflectionAttr.SaveStateOnly)
                            allFields.Add((reflectionAttr, offset + reflectionAttr.RuntimeOffset, classIndex, miBase, field));
                    }
                }
            }

            // Sort: class index, { member order | member offset }
            addFieldsRecursively(type);

            var sortedHierarchy = allFields
                .OrderBy(x => x.ClassOrder)
                .ThenBy(x => x.Attr.Order)
                .ToArray();

            PCore.Quicksort(sortedHierarchy, (
                (MemberAttribute, uint Offset, uint, RttiField, RttiField) a,
                (MemberAttribute, uint Offset, uint, RttiField, RttiField) b) =>
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
            return _typeFieldInfoCache.GetOrAdd(type, info);
        }
    }
}

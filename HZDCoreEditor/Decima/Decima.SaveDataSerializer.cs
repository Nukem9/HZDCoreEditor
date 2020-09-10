using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Decima
{
    public class SaveDataSerializer
    {
        public BinaryReader Reader { get; private set; }

        public uint FileDataVersion { private set; get; }
        private uint FileDataOffset;
        private uint FileDataLength;

        private StringTableContainer[] StringTablePool; // Strings/char
        private string[] WideStringPool;                // Wide strings/wchar_t
        private HZD.GGUUID[][] GUIDPool;                // GUIDs/16 bytes

        private Dictionary<int, object> LocalSaveObjects;       // Object instances created from this save data (HashMap<int, RTTIObject>)
        private Dictionary<int, HZD.GGUUID> GameDataObjects;    // Object instances stored in the game files (StreamingManager)

        private RTTIContainer[] RTTIContainers;

        private class RTTIContainer
        {
            public RTTI.VirtualRTTIList MemberList { get; private set; }

            public static RTTIContainer FromData(SaveDataSerializer serializer)
            {
                string classType = serializer.ReadIndexedString();
                int memberCount = serializer.ReadVariableLengthOffset();

                var map = new RTTIContainer
                {
                    MemberList = new RTTI.VirtualRTTIList(classType, memberCount)
                };

                for (int i = 0; i < memberCount; i++)
                {
                    string type = serializer.ReadIndexedString();
                    string category = serializer.ReadIndexedString();
                    string name = serializer.ReadIndexedString();

                    map.MemberList.Add(type, category, name);
                }

                map.MemberList.ResolveMembersToFieldInfo();
                return map;
            }
        }

        private class StringTableContainer
        {
            private int BitShiftMask;
            private int BlockCount;
            private int BlockSize;
            private byte[][] Data;
            private byte[] UnknownData;

            public static StringTableContainer FromData(SaveDataSerializer serializer)
            {
                var reader = serializer.Reader;
                int blockCount = reader.ReadInt32();

                if (blockCount < 0)
                    return null;

                int blockSize = reader.ReadInt32();

                if (blockSize < 0)
                    return null;

                if ((blockCount * blockSize) > reader.StreamRemainder() ||
                    ((blockSize - 1) & blockSize) != 0)
                    return null;

                var container = new StringTableContainer()
                {
                    BitShiftMask = System.Numerics.BitOperations.TrailingZeroCount(blockSize),
                    BlockCount = blockCount,
                    BlockSize = blockSize,
                    Data = new byte[blockCount][],
                };

                for (int i = 0; i < container.BlockCount; i++)
                    container.Data[i] = reader.ReadBytesStrict(container.BlockSize);

                // No clue what this is
                int unknownCount = reader.ReadInt32();

                if (unknownCount < 0 || (unknownCount * 8) > reader.StreamRemainder())
                    return null;

                container.UnknownData = reader.ReadBytesStrict(unknownCount * 8);

                return container;
            }

            public string LookupString(int offset)
            {
                int blockIndex = offset >> BitShiftMask;
                int dataIndex = offset & (BlockSize - 1);

                // Build the null-terminated string manually
                ReadOnlySpan<byte> block = Data[blockIndex];

                for (int i = dataIndex; i < block.Length; i++)
                {
                    if (block[i] == 0)
                        return Encoding.UTF8.GetString(block[dataIndex..i]);
                }

                return null;
            }
        }

        public SaveDataSerializer(BinaryReader reader, uint version)
        {
            Reader = reader;
            FileDataVersion = version;
        }

        public void ReadStringsAndRTTIFields(uint fileDataOffset, uint fileDataLength)
        {
            FileDataOffset = fileDataOffset;
            FileDataLength = fileDataLength;

            Reader.BaseStream.Position += FileDataLength - 0x8;
            uint typeDataChunkOffset = Reader.ReadUInt32();
            uint rawDataChunkOffset = Reader.ReadUInt32();

            // String/GUID tables
            Reader.BaseStream.Position = FileDataOffset + rawDataChunkOffset;
            {
                // UTF-8 strings
                int stringTableCount = ReadVariableLengthOffset();
                StringTablePool = new StringTableContainer[stringTableCount];

                for (int i = 0; i < StringTablePool.Length; i++)
                    StringTablePool[i] = StringTableContainer.FromData(this);

                // UTF-16 strings
                int wideStringCount = ReadVariableLengthOffset();
                WideStringPool = new string[wideStringCount];

                for (int i = 0; i < WideStringPool.Length; i++)
                {
                    int stringLength = ReadVariableLengthOffset() * sizeof(short);

                    WideStringPool[i] = Encoding.Unicode.GetString(Reader.ReadBytesStrict(stringLength));
                }

                // GUIDs
                if (FileDataVersion >= 26)
                {
                    GUIDPool = new HZD.GGUUID[256][];

                    for (int i = 0; i < GUIDPool.Length; i++)
                    {
                        int guidCount = ReadVariableLengthOffset();
                        GUIDPool[i] = new HZD.GGUUID[guidCount];

                        for (int j = 0; j < guidCount; j++)
                            GUIDPool[i][j] = HZD.GGUUID.FromData(Reader);
                    }
                }
            }

            // Serialized types
            Reader.BaseStream.Position = FileDataOffset + typeDataChunkOffset;
            {
                // Create basic objects that are immediately registered with the engine
                int objectInstanceTypeCount = ReadVariableLengthOffset();
                LocalSaveObjects = new Dictionary<int, object>();

                for (int i = 0; i < objectInstanceTypeCount; i++)
                {
                    Type objectType = RTTI.GetTypeByName(ReadIndexedString());
                    int instanceCount = ReadVariableLengthOffset();

                    for (int j = 0; j < instanceCount; j++)
                    {
                        int objectId = ReadVariableLengthOffset();

                        LocalSaveObjects.Add(objectId, RTTI.CreateObjectInstance(objectType));
                    }
                }

                // Handles to game data objects
                int gameDataObjectCount = ReadVariableLengthOffset();
                GameDataObjects = new Dictionary<int, HZD.GGUUID>();

                for (int i = 0; i < gameDataObjectCount; i++)
                {
                    int objectId = ReadVariableLengthOffset();
                    var guid = HZD.GGUUID.FromData(Reader);

                    GameDataObjects.Add(objectId, guid);
                }

                // RTTI/class member layouts
                int rttiContainerCount = ReadVariableLengthOffset();
                RTTIContainers = new RTTIContainer[rttiContainerCount];

                for (int i = 0; i < RTTIContainers.Length; i++)
                    RTTIContainers[i] = RTTIContainer.FromData(this);

                // Unknown
                int counter4 = ReadVariableLengthOffset();

                for (int i = 0; i < counter4; i++)
                {
                    string unknown = ReadIndexedString();
                }
            }

            Reader.BaseStream.Position = FileDataOffset;
        }

        public object ReadObjectHandle()
        {
            int objectId = ReadVariableLengthInt();

            if (objectId == 0)
                return null;

            if ((objectId & 1) == 0)
            {
                var gameDataGUID = GameDataObjects[objectId];

                // Need core files to be loaded in order to resolve this GUID
                return new object();
            }

            var localObject = LocalSaveObjects[objectId];
            byte loadType = Reader.ReadByte();

            if (loadType == 1)
            {
                /*
                uint unknownOffset = 0;

                if (qword8)
                {
                    unknownOffset = Reader.ReadUInt32();
                }

                if (object present in guid table?)
                {
                    if (qword8)
                        Reader.BaseStream.Position += unknownOffset;
                }
                else
                */
                ManuallyResolveObject(localObject.GetType(), localObject);
                return localObject;
            }
            else if (loadType == 2)
            {
                throw new Exception();
            }

            throw new Exception($"Unknown load type {loadType}");
        }

        public T DeserializeType<T>()
        {
            return (T)DeserializeType(typeof(T));
        }

        public object DeserializeType(Type type)
        {
            // Special handling for int32/uint32
            if (!type.IsEnum)
            {
                if (type == typeof(int))
                    return ReadVariableLengthInt();
                else if (type == typeof(uint))
                    return (uint)ReadVariableLengthInt();
            }

            // Enums and trivial types
            if (DeserializeTrivialType(type, out object trivialValue))
                return trivialValue;

            // Classes and structs
            if (DeserializeObjectType(type, out object objectValue))
                return objectValue;

            throw new NotImplementedException($"Unhandled object type '{type.FullName}'");
        }

        private void DeserializeTypeFromField(object instance, FieldInfo field)
        {
            field.SetValue(instance, DeserializeType(field.FieldType));
        }

        private bool DeserializeObjectType(Type type, out object objectInstance)
        {
            if (!type.IsClass && !type.IsValueType)
            {
                objectInstance = null;
                return false;
            }

            objectInstance = Activator.CreateInstance(type);

            if (objectInstance is RTTI.ISaveSerializable asSerializable)
            {
                // Custom deserialization function implemented. Let the interface do the work.
                asSerializable.DeserializeStateObject(this);
            }
            else
            {
                ManuallyResolveClassMembers(type, objectInstance);
            }

            if (objectInstance is RTTI.ISaveExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.DeserializeStateObjectExtraData(this);

            return true;
        }

        public void ManuallyResolveClassMembers(Type type, object objectInstance)
        {
            int typeIndex = ReadVariableLengthInt();
            var container = RTTIContainers[typeIndex];

            if (container.MemberList.ClassType != type)
                Debugger.Break();

            var info = RTTI.GetOrderedFieldsForClass(type);

            // Instantiate bases
            foreach (var baseClass in info.MIBases)
                baseClass.SetValue(objectInstance, Activator.CreateInstance(baseClass.FieldType));

            // Read members
            foreach (var member in container.MemberList._resolvedMembers)
            {
                if (member.MIBase != null)
                    DeserializeTypeFromField(member.MIBase.GetValue(objectInstance), member.Field);
                else
                    DeserializeTypeFromField(objectInstance, member.Field);
            }
        }

        public void ManuallyResolveObject(Type type, object objectInstance)
        {
            if (!type.IsClass && !type.IsValueType)
                throw new Exception();

            if (objectInstance is RTTI.ISaveSerializable asSerializable)
            {
                // Custom deserialization function implemented. Let the interface do the work.
                asSerializable.DeserializeStateObject(this);
            }
            else
            {
                ManuallyResolveClassMembers(type, objectInstance);
            }

            if (objectInstance is RTTI.ISaveExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.DeserializeStateObjectExtraData(this);
        }

        private bool DeserializeTrivialType(Type type, out object value)
        {
            bool valid = true;

            // The game always does a direct memory copy for these
            value = Type.GetTypeCode(type) switch
            {
                TypeCode.Boolean => Reader.ReadBooleanStrict(),
                TypeCode.SByte => Reader.ReadSByte(),
                TypeCode.Byte => Reader.ReadByte(),
                TypeCode.Int16 => Reader.ReadInt16(),
                TypeCode.UInt16 => Reader.ReadUInt16(),
                TypeCode.Int32 => Reader.ReadInt32(),
                TypeCode.UInt32 => Reader.ReadUInt32(),
                TypeCode.Int64 => Reader.ReadInt64(),
                TypeCode.UInt64 => Reader.ReadUInt64(),
                TypeCode.Single => Reader.ReadSingle(),
                TypeCode.Double => Reader.ReadDouble(),
                _ => valid = false,
            };

            return valid;
        }

        public int ReadVariableLengthInt()
        {
            // EndOfStreamException may be raised here
            sbyte asByteValue = Reader.ReadSByte();

            int value = asByteValue switch
            {
                -128 => Reader.ReadInt32(),
                -127 => Reader.ReadInt16(),
                _ => asByteValue,
            };

            return value;
        }

        public int ReadVariableLengthOffset()
        {
            int value = ReadVariableLengthInt();

            if (value < 0 || value > Reader.StreamRemainder())
                throw new InvalidDataException();

            return value;
        }

        public string ReadIndexedString()
        {
            int index = ReadVariableLengthInt();
            int offset = ReadVariableLengthInt();

            if (index < 0 || index >= StringTablePool.Length)
                throw new InvalidDataException();

            return StringTablePool[index].LookupString(offset);
        }

        public string ReadIndexedWideString()
        {
            int index = ReadVariableLengthInt();

            if (index < 0 || index >= WideStringPool.Length)
                throw new InvalidDataException();

            return WideStringPool[index];
        }

        public HZD.GGUUID ReadIndexedGUID()
        {
            if (FileDataVersion < 26)
            {
                // Inline read of 16 bytes
                return HZD.GGUUID.FromData(Reader);
            }

            // GUID pool lookup
            short value = Reader.ReadInt16();

            if (value == -1)
                return HZD.GGUUID.Empty;

            int index = (value >> 8) & 0xFF;
            int offset = value & 0xFF;

            return GUIDPool[index][offset];
        }
    }
}

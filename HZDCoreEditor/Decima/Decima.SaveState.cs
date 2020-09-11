using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decima
{
    public class SaveState
    {
        public BinaryReader Reader { get; private set; }
        public uint SaveVersion { get; private set; }
        private uint SaveDataOffset;
        private uint SaveDataLength;

        private StringTableContainer StringPool;                // Strings/char
        private WideStringTableContainer WideStringPool;        // Wide strings/wchar_t
        private GUIDTableContainer GUIDPool;                    // GUIDs/16 bytes

        private Dictionary<int, object> LocalSaveObjects;       // Object instances created from this save data (HashMap<int, RTTIObject>)
        private Dictionary<int, HZD.GGUUID> GameDataObjects;    // Object instances stored in the game files (StreamingManager)
        private RTTIMapContainer RTTIContainer;                 // Stored class layouts
        private List<string> PrefetchFilePaths;                 // Preload/cache core files that will be needed soon

        private class StringTableContainer
        {
            private class Table
            {
                public int BitShiftMask;
                public int BlockCount;
                public int BlockSize;
                public byte[][] Data;
                public byte[] UnknownData;
            };

            private readonly List<Table> Tables = new List<Table>();

            public string LookupString(int index, int offset)
            {
                Table stringTable = Tables[index];

                int blockIndex = offset >> stringTable.BitShiftMask;
                int dataIndex = offset & (stringTable.BlockSize - 1);

                // Build the null-terminated string manually
                ReadOnlySpan<byte> block = stringTable.Data[blockIndex];

                for (int i = dataIndex; i < block.Length; i++)
                {
                    if (block[i] == 0)
                        return Encoding.UTF8.GetString(block[dataIndex..i]);
                }

                return null;
            }

            public static StringTableContainer FromData(SaveState state)
            {
                int stringTableCount = state.ReadVariableLengthOffset();

                var reader = state.Reader;
                var container = new StringTableContainer();

                for (int i = 0; i < stringTableCount; i++)
                {
                    int blockCount = reader.ReadInt32();

                    if (blockCount < 0)
                        return null;

                    int blockSize = reader.ReadInt32();

                    if (blockSize < 0)
                        return null;

                    if ((blockCount * blockSize) > reader.StreamRemainder() ||
                        ((blockSize - 1) & blockSize) != 0)
                        return null;

                    var table = new Table()
                    {
                        BitShiftMask = System.Numerics.BitOperations.TrailingZeroCount(blockSize),
                        BlockCount = blockCount,
                        BlockSize = blockSize,
                        Data = new byte[blockCount][],
                    };

                    for (int j = 0; j < table.BlockCount; j++)
                        table.Data[j] = reader.ReadBytesStrict(table.BlockSize);

                    // No clue what this is
                    int unknownCount = reader.ReadInt32();

                    if (unknownCount < 0 || (unknownCount * 8) > reader.StreamRemainder())
                        return null;

                    table.UnknownData = reader.ReadBytesStrict(unknownCount * 8);

                    // Insert
                    container.Tables.Add(table);
                }

                return container;
            }
        }

        private class WideStringTableContainer
        {
            private readonly List<string> Table = new List<string>();

            public string LookupString(int index)
            {
                return Table[index];
            }

            public static WideStringTableContainer FromData(SaveState state)
            {
                int wideStringCount = state.ReadVariableLengthOffset();
                var container = new WideStringTableContainer();

                for (int i = 0; i < wideStringCount; i++)
                {
                    int stringLength = state.ReadVariableLengthOffset() * sizeof(short);
                    string str = Encoding.Unicode.GetString(state.Reader.ReadBytesStrict(stringLength));

                    container.Table.Add(str);
                }

                return container;
            }
        }

        private class GUIDTableContainer
        {
            private const int HardcodedTableCount = 256;
            private readonly List<HZD.GGUUID>[] Tables = new List<HZD.GGUUID>[HardcodedTableCount];

            public GUIDTableContainer()
            {
                for (int i = 0; i < Tables.Length; i++)
                    Tables[i] = new List<HZD.GGUUID>();
            }

            public HZD.GGUUID LookupGUID(int index, int offset)
            {
                return Tables[index][offset];
            }

            public static GUIDTableContainer FromData(SaveState state)
            {
                var container = new GUIDTableContainer();

                // Why do they try to keep GUID allocations balanced across 256 tables? I don't see the point
                for (int i = 0; i < HardcodedTableCount; i++)
                {
                    int guidCount = state.ReadVariableLengthOffset();

                    for (int j = 0; j < guidCount; j++)
                        container.Tables[i].Add(HZD.GGUUID.FromData(state.Reader));
                }

                return container;
            }
        }

        private class RTTIMapContainer
        {
            public List<RTTI.VirtualRTTIList> Map = new List<RTTI.VirtualRTTIList>();

            public static RTTIMapContainer FromData(SaveState state)
            {
                int rttiEntryCount = state.ReadVariableLengthOffset();
                var container = new RTTIMapContainer();

                for (int i = 0; i < rttiEntryCount; i++)
                {
                    string classType = state.ReadIndexedString();
                    int memberCount = state.ReadVariableLengthOffset();

                    var rttiList = new RTTI.VirtualRTTIList(classType, memberCount);

                    for (int j = 0; j < memberCount; j++)
                    {
                        string type = state.ReadIndexedString();
                        string category = state.ReadIndexedString();
                        string name = state.ReadIndexedString();

                        rttiList.Add(type, category, name);
                    }

                    rttiList.ResolveMembersToFieldInfo();
                    container.Map.Add(rttiList);
                }

                return container;
            }

            public RTTI.VirtualRTTIList LookupRTTI(int index)
            {
                return Map[index];
            }
        }

        public SaveState(BinaryReader reader, uint saveVersion, uint saveDataOffset, uint saveDataLength)
        {
            Reader = reader;
            SaveVersion = saveVersion;
            SaveDataOffset = saveDataOffset;
            SaveDataLength = saveDataLength;

            ReadHeaderData();
        }

        private void ReadHeaderData()
        {
            Reader.BaseStream.Position = SaveDataOffset + SaveDataLength - 0x8;
            uint typeDataChunkOffset = Reader.ReadUInt32();
            uint rawDataChunkOffset = Reader.ReadUInt32();

            // String/GUID tables
            Reader.BaseStream.Position = SaveDataOffset + rawDataChunkOffset;
            {
                // UTF-8 strings
                StringPool = StringTableContainer.FromData(this);

                // UTF-16 strings
                WideStringPool = WideStringTableContainer.FromData(this);

                // GUIDs
                if (SaveVersion >= 26)
                    GUIDPool = GUIDTableContainer.FromData(this);
            }

            // Serialized type information and object instances
            Reader.BaseStream.Position = SaveDataOffset + typeDataChunkOffset;
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
                RTTIContainer = RTTIMapContainer.FromData(this);

                // File prefetch
                int prefetchFileCount = ReadVariableLengthOffset();
                PrefetchFilePaths = new List<string>(prefetchFileCount);

                for (int i = 0; i < prefetchFileCount; i++)
                    PrefetchFilePaths.Add(ReadIndexedString());
            }

            Reader.BaseStream.Position = SaveDataOffset;
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
            if (RTTI.DeserializeTrivialType(Reader, type, out object trivialValue))
                return trivialValue;

            // Classes and structs
            if (DeserializeObjectType(type, out object objectValue))
                return objectValue;

            throw new NotImplementedException($"Unhandled object type '{type.FullName}'");
        }

        public bool DeserializeObjectType(Type type, out object objectInstance, object existingObjectInstance = null)
        {
            if (!type.IsClass && !type.IsValueType)
            {
                objectInstance = null;
                return false;
            }

            objectInstance = existingObjectInstance ?? RTTI.CreateObjectInstance(type);

            if (objectInstance is RTTI.ISaveSerializable asSerializable)
            {
                // Custom deserialization function implemented. Let the interface do the work.
                asSerializable.DeserializeStateObject(this);
            }
            else
            {
                DeserializeObjectClassMembers(type, objectInstance);
            }

            return true;
        }

        public void DeserializeObjectClassMembers(Type type, object objectInstance)
        {
            int typeIndex = ReadVariableLengthInt();
            var rtti = RTTIContainer.LookupRTTI(typeIndex);

            if (rtti.ClassType != type)
                throw new InvalidDataException("Trying to deserialize an object that does not match the binary data type");

            // Read members
            foreach (var member in rtti._resolvedMembers)
            {
                // Check if this field needs to be applied to an emulated base class
                var baseClass = member.MIBase != null ? member.MIBase.GetValue(objectInstance) : objectInstance;

                member.Field.SetValue(baseClass, DeserializeType(member.Field.FieldType));
            }
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

            return StringPool.LookupString(index, offset);
        }

        public string ReadIndexedWideString()
        {
            int index = ReadVariableLengthInt();

            return WideStringPool.LookupString(index);
        }

        public HZD.GGUUID ReadIndexedGUID()
        {
            if (SaveVersion < 26)
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

            return GUIDPool.LookupGUID(index, offset);
        }

        public object ReadObjectHandle()
        {
            int objectId = ReadVariableLengthInt();

            if (objectId == 0)
                return null;

            if ((objectId & 1) == 0)
            {
                var gameDataGUID = GameDataObjects[objectId];

                // Need core files to be loaded in order to resolve GUID
                return new object();
            }

            var savedObject = LocalSaveObjects[objectId];
            byte loadType = Reader.ReadByte();

            if (loadType == 1)
            {
                /*
                uint unknownOffset = 0;

                if (qword8)
                    unknownOffset = Reader.ReadUInt32();

                if (object present in guid table?)
                {
                    if (qword8)
                        Reader.BaseStream.Position += unknownOffset;
                }
                else
                */

                if (!DeserializeObjectType(savedObject.GetType(), out savedObject, savedObject))
                    throw new Exception("Why did this fail?");

                return savedObject;
            }
            else if (loadType == 2)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException($"Unknown load type {loadType}");
        }
    }
}

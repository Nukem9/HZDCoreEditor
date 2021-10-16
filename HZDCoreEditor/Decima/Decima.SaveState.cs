using HZDCoreEditor.Util;
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
        private uint _saveDataOffset;
        private uint _saveDataLength;

        private StringTableContainer _stringPool;                   // Strings/char
        private WideStringTableContainer _wideStringPool;           // Wide strings/wchar_t
        private GUIDTableContainer _GUIDPool;                       // GUIDs/16 bytes

        private Dictionary<int, object> _localSaveObjects;          // Object instances created from this save data (HashMap<int, RTTIObject>)
        private Dictionary<int, BaseGGUUID> _gameDataObjects;       // Object instances stored in the game files (StreamingManager)
        private RTTIMapContainer _RTTIContainer;                     // Stored class layouts
        private List<string> _prefetchFilePaths;                    // Preload/cache core files that will be needed soon

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

            private readonly List<Table> _tables = new List<Table>();

            public string LookupString(int index, int offset)
            {
                Table stringTable = _tables[index];

                int blockIndex = offset >> stringTable.BitShiftMask;
                int dataIndex = offset & (stringTable.BlockSize - 1);

                // Build the null-terminated string manually
                ReadOnlySpan<byte> block = stringTable.Data[blockIndex];

                for (int i = dataIndex; i < block.Length; i++)
                {
                    if (block[i] == 0)
                        return Encoding.UTF8.GetString(block.Slice(dataIndex, i - dataIndex).ToArray());
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
                        BitShiftMask = BitOperations.TrailingZeroCount(blockSize),
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
                    container._tables.Add(table);
                }

                return container;
            }
        }

        private class WideStringTableContainer
        {
            private readonly List<string> _table = new List<string>();

            public string LookupString(int index)
            {
                return _table[index];
            }

            public static WideStringTableContainer FromData(SaveState state)
            {
                int wideStringCount = state.ReadVariableLengthOffset();
                var container = new WideStringTableContainer();

                for (int i = 0; i < wideStringCount; i++)
                {
                    int stringLength = state.ReadVariableLengthOffset() * sizeof(short);
                    string str = Encoding.Unicode.GetString(state.Reader.ReadBytesStrict(stringLength));

                    container._table.Add(str);
                }

                return container;
            }
        }

        private class GUIDTableContainer
        {
            private const int HardcodedTableCount = 256;
            private readonly List<BaseGGUUID>[] _tables = new List<BaseGGUUID>[HardcodedTableCount];

            public GUIDTableContainer()
            {
                for (int i = 0; i < _tables.Length; i++)
                    _tables[i] = new List<BaseGGUUID>();
            }

            public BaseGGUUID LookupGUID(int index, int offset)
            {
                return _tables[index][offset];
            }

            public static GUIDTableContainer FromData(SaveState state)
            {
                var container = new GUIDTableContainer();

                // Why do they try to keep GUID allocations balanced across 256 tables? I don't see the point
                for (int i = 0; i < HardcodedTableCount; i++)
                {
                    int guidCount = state.ReadVariableLengthOffset();

                    for (int j = 0; j < guidCount; j++)
                        container._tables[i].Add(new BaseGGUUID().FromData(state.Reader));
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
            _saveDataOffset = saveDataOffset;
            _saveDataLength = saveDataLength;

            ReadHeaderData();
        }

        private void ReadHeaderData()
        {
            Reader.BaseStream.Position = _saveDataOffset + _saveDataLength - 0x8;
            uint typeDataChunkOffset = Reader.ReadUInt32();
            uint rawDataChunkOffset = Reader.ReadUInt32();

            // String/GUID tables
            Reader.BaseStream.Position = _saveDataOffset + rawDataChunkOffset;
            {
                // UTF-8 strings
                _stringPool = StringTableContainer.FromData(this);

                // UTF-16 strings
                _wideStringPool = WideStringTableContainer.FromData(this);

                // GUIDs
                if (SaveVersion >= 26)
                    _GUIDPool = GUIDTableContainer.FromData(this);
            }

            // Serialized type information and object instances
            Reader.BaseStream.Position = _saveDataOffset + typeDataChunkOffset;
            {
                // Create basic objects that are immediately registered with the engine
                int objectInstanceTypeCount = ReadVariableLengthOffset();
                _localSaveObjects = new Dictionary<int, object>();

                for (int i = 0; i < objectInstanceTypeCount; i++)
                {
                    Type objectType = RTTI.GetTypeByName(ReadIndexedString());
                    int instanceCount = ReadVariableLengthOffset();

                    for (int j = 0; j < instanceCount; j++)
                    {
                        int objectId = ReadVariableLengthOffset();

                        _localSaveObjects.Add(objectId, RTTI.CreateObjectInstance(objectType));
                    }
                }

                // Handles to game data objects
                int gameDataObjectCount = ReadVariableLengthOffset();
                _gameDataObjects = new Dictionary<int, BaseGGUUID>();

                for (int i = 0; i < gameDataObjectCount; i++)
                {
                    int objectId = ReadVariableLengthOffset();
                    var guid = new BaseGGUUID().FromData(Reader);

                    _gameDataObjects.Add(objectId, guid);
                }

                // RTTI/class member layouts
                _RTTIContainer = RTTIMapContainer.FromData(this);

                // File prefetch
                int prefetchFileCount = ReadVariableLengthOffset();
                _prefetchFilePaths = new List<string>(prefetchFileCount);

                for (int i = 0; i < prefetchFileCount; i++)
                    _prefetchFilePaths.Add(ReadIndexedString());
            }

            Reader.BaseStream.Position = _saveDataOffset;
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
            var rtti = _RTTIContainer.LookupRTTI(typeIndex);

            if (rtti.ClassType != type)
                throw new InvalidDataException("Trying to deserialize an object that does not match the binary data type");

            // Read members
            foreach (var member in rtti.ResolvedMembers)
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

        public delegate void ItemReaderAction<T>(ref T item);

        public List<T> ReadVariableItemList<T>(ItemReaderAction<T> action)
        {
            // Not part of the game code. Added for convenience.
            int counter = ReadVariableLengthOffset();
            var items = new List<T>(counter);

            for (int i = 0; i < counter; i++)
            {
                T item = Activator.CreateInstance<T>();
                action(ref item);

                items.Add(item);
            }

            return items;
        }

        public string ReadIndexedString()
        {
            int index = ReadVariableLengthInt();
            int offset = ReadVariableLengthInt();

            return _stringPool.LookupString(index, offset);
        }

        public string ReadIndexedWideString()
        {
            int index = ReadVariableLengthInt();

            return _wideStringPool.LookupString(index);
        }

        public BaseGGUUID ReadIndexedGUID()
        {
            if (SaveVersion < 26)
            {
                // Inline read of 16 bytes
                return new BaseGGUUID().FromData(Reader);
            }

            // GUID pool lookup
            short value = Reader.ReadInt16();

            if (value == -1)
                return BaseGGUUID.Empty;

            int index = (value >> 8) & 0xFF;
            int offset = value & 0xFF;

            return _GUIDPool.LookupGUID(index, offset);
        }

        public object ReadObjectHandle()
        {
            int objectId = ReadVariableLengthInt();

            if (objectId == 0)
                return null;

            if ((objectId & 1) == 0)
            {
                var gameDataGUID = _gameDataObjects[objectId];

                // Need core files to be loaded in order to resolve GUID
                return new object();
            }

            var savedObject = _localSaveObjects[objectId];
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

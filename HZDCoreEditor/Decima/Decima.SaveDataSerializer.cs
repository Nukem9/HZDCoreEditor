using BinaryStreamExtensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Decima
{
    public class SaveDataSerializer
    {
        public BinaryReader Reader { get; private set; }

        private uint FileDataVersion;
        private uint FileDataOffset;
        private uint FileDataLength;

        private StringTableContainer[] StringTables;
        private string[] WideStringTable;
        private byte[] GUIDTable;
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

        public SaveDataSerializer(BinaryReader reader)
        {
            Reader = reader;
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
                StringTables = new StringTableContainer[stringTableCount];

                for (int i = 0; i < StringTables.Length; i++)
                    StringTables[i] = StringTableContainer.FromData(this);

                // UTF-16 strings
                int wideStringCount = ReadVariableLengthOffset();
                WideStringTable = new string[wideStringCount];

                for (int i = 0; i < WideStringTable.Length; i++)
                {
                    int stringLength = ReadVariableLengthOffset() * sizeof(short);

                    WideStringTable[i] = Encoding.Unicode.GetString(Reader.ReadBytesStrict(stringLength));
                }

                // GUIDs
                if (FileDataVersion > 26)
                {
                    int guidCount = ReadVariableLengthOffset();

                    if (guidCount > 0)
                        GUIDTable = Reader.ReadBytesStrict(guidCount * 16);
                }
            }

            // Serialized types
            Reader.BaseStream.Position = FileDataOffset + typeDataChunkOffset;
            {
                // Create basic objects?
                int counter1 = ReadVariableLengthOffset();

                if (Reader.BaseStream.Position != 0x5C26)
                    Debugger.Break();

                for (int i = 0; i < counter1; i++)
                {
                }

                // Type GUIDs?
                int counter2 = ReadVariableLengthOffset();

                if (Reader.BaseStream.Position != 0x5C27)
                    Debugger.Break();

                for (int i = 0; i < counter2; i++)
                {
                }

                // RTTI/class member layouts
                int rttiContainerCount = ReadVariableLengthOffset();

                if (Reader.BaseStream.Position != 0x5C28)
                    Debugger.Break();

                RTTIContainers = new RTTIContainer[rttiContainerCount];

                for (int i = 0; i < RTTIContainers.Length; i++)
                    RTTIContainers[i] = RTTIContainer.FromData(this);

                // Unknown
                int counter4 = ReadVariableLengthOffset();

                if (Reader.BaseStream.Position != 0x614D)
                    Debugger.Break();

                for (int i = 0; i < counter4; i++)
                {
                    string unknown = ReadIndexedString();
                }
            }

            Reader.BaseStream.Position = FileDataOffset;
        }

        public T DeserializeType<T>()
        {
            return (T)DeserializeType(typeof(T));
        }

        public object DeserializeType(Type type)
        {
            // Special handling for int32/uint32
            if (!type.IsEnum && (type == typeof(int) || type == typeof(uint)))
                return ReadVariableLengthInt();

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

            if (objectInstance is RTTI.ISaveExtraBinaryDataCallback asExtraBinaryDataCallback)
                asExtraBinaryDataCallback.DeserializeStateObjectExtraData(this);

            return true;
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

            if (index < 0 || index >= StringTables.Length)
                throw new InvalidDataException();

            return StringTables[index].LookupString(offset);
        }

        public string ReadIndexedWideString()
        {
            int index = ReadVariableLengthInt();

            if (index < 0 || index >= WideStringTable.Length)
                throw new InvalidDataException();

            return WideStringTable[index];
        }
    }
}

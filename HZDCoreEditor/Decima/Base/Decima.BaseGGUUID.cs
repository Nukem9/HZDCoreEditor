using HZDCoreEditor.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Decima
{
    using uint8 = System.Byte;

    /// <summary>
    /// Shared wrapper for any Decima game that implements GGUUID. Assumes that the TypeId is 0x211FDC8FD3395464.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public class BaseGGUUID : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public static readonly BaseGGUUID Empty = new BaseGGUUID();

        [RTTI.Member(0, 0x0)] public uint8 Data0 { get => _data[0]; set => _data[0] = value; }
        [RTTI.Member(1, 0x1)] public uint8 Data1 { get => _data[1]; set => _data[1] = value; }
        [RTTI.Member(2, 0x2)] public uint8 Data2 { get => _data[2]; set => _data[2] = value; }
        [RTTI.Member(3, 0x3)] public uint8 Data3 { get => _data[3]; set => _data[3] = value; }
        [RTTI.Member(4, 0x4)] public uint8 Data4 { get => _data[4]; set => _data[4] = value; }
        [RTTI.Member(5, 0x5)] public uint8 Data5 { get => _data[5]; set => _data[5] = value; }
        [RTTI.Member(6, 0x6)] public uint8 Data6 { get => _data[6]; set => _data[6] = value; }
        [RTTI.Member(7, 0x7)] public uint8 Data7 { get => _data[7]; set => _data[7] = value; }
        [RTTI.Member(8, 0x8)] public uint8 Data8 { get => _data[8]; set => _data[8] = value; }
        [RTTI.Member(9, 0x9)] public uint8 Data9 { get => _data[9]; set => _data[9] = value; }
        [RTTI.Member(10, 0xA)] public uint8 Data10 { get => _data[10]; set => _data[10] = value; }
        [RTTI.Member(11, 0xB)] public uint8 Data11 { get => _data[11]; set => _data[11] = value; }
        [RTTI.Member(12, 0xC)] public uint8 Data12 { get => _data[12]; set => _data[12] = value; }
        [RTTI.Member(13, 0xD)] public uint8 Data13 { get => _data[13]; set => _data[13] = value; }
        [RTTI.Member(14, 0xE)] public uint8 Data14 { get => _data[14]; set => _data[14] = value; }
        [RTTI.Member(15, 0xF)] public uint8 Data15 { get => _data[15]; set => _data[15] = value; }

        private const int GUIDDataLength = 16;// Bytes
        private readonly byte[] _data = new byte[GUIDDataLength];

        public BaseGGUUID()
        {
        }

        public BaseGGUUID(BaseGGUUID other)
        {
            Assign(other._data);
        }

        public void ToData(BinaryWriter writer)
        {
            writer.Write(_data);
        }

        public override string ToString()
        {
            return new Guid(_data).ToString("B").ToUpper();
        }

        public static BaseGGUUID FromData(ReadOnlySpan<byte> data)
        {
            var guid = new BaseGGUUID();
            guid.Assign(data);

            return guid;
        }

        public static BaseGGUUID FromData(BinaryReader reader)
        {
            return FromData(reader.ReadBytesStrict(GUIDDataLength));
        }

        public static BaseGGUUID FromString(string value)
        {
            if (!Guid.TryParse(value, out Guid guid))
                throw new ArgumentException("Invalid GUID", nameof(value));

            return FromData(guid.ToByteArray());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseGGUUID guid))
                return false;
            
            return Enumerable.SequenceEqual(_data, guid._data);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < 16; i++)
                    hash = (hash ^ _data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            if (reader.Read(_data, 0, GUIDDataLength) != GUIDDataLength)
                throw new EndOfStreamException("Short read of GUID");
        }

        public void Serialize(BinaryWriter writer)
        {
            ToData(writer);
        }

        public void DeserializeStateObject(SaveState state)
        {
            Assign(state.ReadIndexedGUID()._data);
        }

        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();

        protected void Assign(ReadOnlySpan<byte> data)
        {
            for (int i = 0; i < GUIDDataLength; i++)
                _data[i] = data[i];
        }

        public static implicit operator BaseGGUUID(string value)
        {
            return FromString(value);
        }

        public static implicit operator BaseGGUUID(Guid value)
        {
            return FromData(value.ToByteArray());
        }

        public static bool operator ==(BaseGGUUID left, BaseGGUUID right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(BaseGGUUID left, BaseGGUUID right)
        {
            return !(left == right);
        }
    }
}

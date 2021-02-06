using Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Decima
{
    using uint8 = System.Byte;

    /// <summary>
    /// Shared wrapper for any Decima game that implements GGUUID. Assumes that the TypeId is 0x211FDC8FD3395464.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public class BaseGGUUID : RTTI.ISaveSerializable
    {
        public static readonly BaseGGUUID Empty = new BaseGGUUID();

        [RTTI.Member(0, 0x0)] public uint8 Data0;
        [RTTI.Member(1, 0x1)] public uint8 Data1;
        [RTTI.Member(2, 0x2)] public uint8 Data2;
        [RTTI.Member(3, 0x3)] public uint8 Data3;
        [RTTI.Member(4, 0x4)] public uint8 Data4;
        [RTTI.Member(5, 0x5)] public uint8 Data5;
        [RTTI.Member(6, 0x6)] public uint8 Data6;
        [RTTI.Member(7, 0x7)] public uint8 Data7;
        [RTTI.Member(8, 0x8)] public uint8 Data8;
        [RTTI.Member(9, 0x9)] public uint8 Data9;
        [RTTI.Member(10, 0xA)] public uint8 Data10;
        [RTTI.Member(11, 0xB)] public uint8 Data11;
        [RTTI.Member(12, 0xC)] public uint8 Data12;
        [RTTI.Member(13, 0xD)] public uint8 Data13;
        [RTTI.Member(14, 0xE)] public uint8 Data14;
        [RTTI.Member(15, 0xF)] public uint8 Data15;

        public void ToData(BinaryWriter writer)
        {
            writer.Write(Data0);
            writer.Write(Data1);
            writer.Write(Data2);
            writer.Write(Data3);
            writer.Write(Data4);
            writer.Write(Data5);
            writer.Write(Data6);
            writer.Write(Data7);
            writer.Write(Data8);
            writer.Write(Data9);
            writer.Write(Data10);
            writer.Write(Data11);
            writer.Write(Data12);
            writer.Write(Data13);
            writer.Write(Data14);
            writer.Write(Data15);
        }

        public override string ToString()
        {
            return new Guid(ToBytes()).ToString("B");
        }

        public BaseGGUUID FromData(BinaryReader reader)
        {
            return FromData(reader.ReadBytesStrict(16));
        }
        public BaseGGUUID FromData(ReadOnlySpan<byte> data)
        {
            AssignFromData(data);
            return this;
        }

        public BaseGGUUID FromString(string value)
        {
            if (!Guid.TryParse(value, out Guid guid))
                throw new ArgumentException("Invalid GUID", nameof(value));

            return FromData(guid.ToByteArray());
        }

        public void DeserializeStateObject(SaveState state)
        {
            AssignFromOther(state.ReadIndexedGUID());
        }
        public static BaseGGUUID FromOther(BaseGGUUID other)
        {
            var x = new BaseGGUUID();
            x.AssignFromOther(other);

            return x;
        }

        public void AssignFromOther(BaseGGUUID other)
        {
            // No unions. No marshaling. Assign each manually...
            Data0 = other.Data0;
            Data1 = other.Data1;
            Data2 = other.Data2;
            Data3 = other.Data3;
            Data4 = other.Data4;
            Data5 = other.Data5;
            Data6 = other.Data6;
            Data7 = other.Data7;
            Data8 = other.Data8;
            Data9 = other.Data9;
            Data10 = other.Data10;
            Data11 = other.Data11;
            Data12 = other.Data12;
            Data13 = other.Data13;
            Data14 = other.Data14;
            Data15 = other.Data15;
        }

        protected void AssignFromData(ReadOnlySpan<byte> data)
        {
            Data0 = data[0];
            Data1 = data[1];
            Data2 = data[2];
            Data3 = data[3];
            Data4 = data[4];
            Data5 = data[5];
            Data6 = data[6];
            Data7 = data[7];
            Data8 = data[8];
            Data9 = data[9];
            Data10 = data[10];
            Data11 = data[11];
            Data12 = data[12];
            Data13 = data[13];
            Data14 = data[14];
            Data15 = data[15];
        }

        public byte[] ToBytes()
        {
            return new []
            {
                Data0, Data1, Data2, Data3, Data4, Data5, Data6, Data7,
                Data8, Data9, Data10, Data11, Data12, Data13, Data14, Data15
            };
        }

        public override bool Equals(object obj)
        {
            return obj is BaseGGUUID gGUUID &&
                Data0 == gGUUID.Data0 &&
                Data1 == gGUUID.Data1 &&
                Data2 == gGUUID.Data2 &&
                Data3 == gGUUID.Data3 &&
                Data4 == gGUUID.Data4 &&
                Data5 == gGUUID.Data5 &&
                Data6 == gGUUID.Data6 &&
                Data7 == gGUUID.Data7 &&
                Data8 == gGUUID.Data8 &&
                Data9 == gGUUID.Data9 &&
                Data10 == gGUUID.Data10 &&
                Data11 == gGUUID.Data11 &&
                Data12 == gGUUID.Data12 &&
                Data13 == gGUUID.Data13 &&
                Data14 == gGUUID.Data14 &&
                Data15 == gGUUID.Data15;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;
                
                hash = (hash ^ Data0) * p;
                hash = (hash ^ Data1) * p;
                hash = (hash ^ Data2) * p;
                hash = (hash ^ Data3) * p;
                hash = (hash ^ Data4) * p;
                hash = (hash ^ Data5) * p;
                hash = (hash ^ Data6) * p;
                hash = (hash ^ Data7) * p;
                hash = (hash ^ Data8) * p;
                hash = (hash ^ Data9) * p;
                hash = (hash ^ Data10) * p;
                hash = (hash ^ Data11) * p;
                hash = (hash ^ Data12) * p;
                hash = (hash ^ Data13) * p;
                hash = (hash ^ Data14) * p;
                hash = (hash ^ Data15) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();

        public static implicit operator BaseGGUUID(string value)
        {
            return new BaseGGUUID().FromString(value);
        }

        public static implicit operator BaseGGUUID(Guid value)
        {
            return new BaseGGUUID().FromData(value.ToByteArray());
        }

        public static bool operator ==(BaseGGUUID left, BaseGGUUID right)
        {
            return EqualityComparer<BaseGGUUID>.Default.Equals(left, right);
        }

        public static bool operator !=(BaseGGUUID left, BaseGGUUID right)
        {
            return !(left == right);
        }
    }
}

using Utility;
using System;
using System.Diagnostics;
using System.IO;

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

<<<<<<< HEAD
        public void DeserializeStateObject(SaveState state)
        {
            AssignFromOther(state.ReadIndexedGUID());
        }

        public bool IsEmpty()
        {
            return
                Data0 == 0 && Data1 == 0 && Data2 == 0 && Data3 == 0 &&
                Data4 == 0 && Data5 == 0 && Data6 == 0 && Data7 == 0 &&
                Data8 == 0 && Data9 == 0 && Data10 == 0 && Data11 == 0 &&
                Data12 == 0 && Data13 == 0 && Data14 == 0 && Data15 == 0;
        }

        public override string ToString()
        {
            return $"{{{Data3:X2}{Data2:X2}{Data1:X2}{Data0:X2}-{Data5:X2}{Data4:X2}-{Data7:X2}{Data6:X2}-{Data8:X2}{Data9:X2}" +
                $"-{Data10:X2}{Data11:X2}{Data12:X2}{Data13:X2}{Data14:X2}{Data15:X2}}}";
        }

=======
>>>>>>> nukem9/master
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

<<<<<<< HEAD
        public static BaseGGUUID FromString(string data)
        {
            if (data == null) 
                throw new ArgumentNullException(nameof(data));
            if (data.Length != 38)
                throw new Exception($"Invalid BaseGGUUID data length: {data.Length}");

            byte GetHexByte(string text, int idx) => Convert.ToByte(text.Substring(idx, 2), 16);

            var byteIdx = 0;
            var offset = 1;

            var id = new BaseGGUUID();
            id.Data3  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data2  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data1  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data0  = GetHexByte(data, byteIdx++ * 2 + offset);
            offset++;
            id.Data5  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data4  = GetHexByte(data, byteIdx++ * 2 + offset);
            offset++;
            id.Data7  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data6  = GetHexByte(data, byteIdx++ * 2 + offset);
            offset++;
            id.Data8  = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data9  = GetHexByte(data, byteIdx++ * 2 + offset);
            offset++;
            id.Data10 = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data11 = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data12 = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data13 = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data14 = GetHexByte(data, byteIdx++ * 2 + offset);
            id.Data15 = GetHexByte(data, byteIdx++ * 2 + offset);

            return id;
        }

        public static BaseGGUUID FromData(BinaryReader reader)
=======
        public override string ToString()
        {
            return $"{{{Data3:X2}{Data2:X2}{Data1:X2}{Data0:X2}-{Data5:X2}{Data4:X2}-{Data7:X2}{Data6:X2}-{Data8:X2}{Data9:X2}-{Data10:X2}{Data11:X2}{Data12:X2}{Data13:X2}{Data14:X2}{Data15:X2}}}";
        }

        public BaseGGUUID FromData(BinaryReader reader)
>>>>>>> nukem9/master
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

        protected void AssignFromOther(BaseGGUUID other)
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

<<<<<<< HEAD
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
            HashCode hash = new HashCode();
            hash.Add(Data0);
            hash.Add(Data1);
            hash.Add(Data2);
            hash.Add(Data3);
            hash.Add(Data4);
            hash.Add(Data5);
            hash.Add(Data6);
            hash.Add(Data7);
            hash.Add(Data8);
            hash.Add(Data9);
            hash.Add(Data10);
            hash.Add(Data11);
            hash.Add(Data12);
            hash.Add(Data13);
            hash.Add(Data14);
            hash.Add(Data15);
            return hash.ToHashCode();
=======
        public static implicit operator BaseGGUUID(string value)
        {
            return new BaseGGUUID().FromString(value);
>>>>>>> nukem9/master
        }
    }
}

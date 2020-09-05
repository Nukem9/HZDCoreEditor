using BinaryStreamExtensions;

namespace Decima.HZD
{
    using uint8 = System.Byte;

    [RTTI.Serializable(0x211FDC8FD3395464)]
    public class GGUUID : RTTI.ISaveSerializable
    {
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

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            var data = serializer.Reader.ReadBytesStrict(16);
        }
    }
}

using System.IO;

namespace Decima.DS
{
    using uint8 = System.Byte;
    using uint16 = System.UInt16;
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x4915E84F7E31E2AA, GameType.DS)]
    public class WwiseWemResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(8, 0x0, "Format")] public bool IsStreaming;
        [RTTI.Member(3, 0x20, "Data")] public uint32 WemID;
        [RTTI.Member(4, 0x24, "Data")] public uint32 WemSize;
        [RTTI.Member(5, 0x28, "Data")] public Array<uint8> WemData;
        [RTTI.Member(6, 0x38, "Data")] public uint16 BitField;
        [RTTI.Member(9, 0xE8, "Format")] public float mLengthInSeconds;
        public StreamHandle StreamInfo;

        public void DeserializeExtraData(BinaryReader reader)
        {
            StreamInfo = StreamHandle.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            StreamInfo.ToData(writer);
        }
    }
}
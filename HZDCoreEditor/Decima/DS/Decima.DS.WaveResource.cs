namespace Decima.DS
{
    using uint8 = System.Byte;
    using uint16 = System.UInt16;
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x257040983B11DA11, GameType.DS)]
    public class WaveResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(6, 0x0, "Format")] public bool IsStreaming;
        [RTTI.Member(7, 0x0, "Format")] public bool UseVBR;
        [RTTI.Member(8, 0x0, "Format")] public EWaveDataEncodingQuality EncodingQuality;
        [RTTI.Member(3, 0x20, "Data")] public Array<uint8> WaveData;
        [RTTI.Member(4, 0x30, "Data")] public uint WaveDataSize;
        [RTTI.Member(13, 0x40, "Format")] public int SampleRate;
        [RTTI.Member(12, 0x44, "Format")] public uint8 ChannelCount;
        [RTTI.Member(11, 0x48, "Format")] public EWaveDataEncoding Encoding;
        [RTTI.Member(14, 0x4C, "Format")] public uint16 BitsPerSample;
        [RTTI.Member(15, 0x50, "Format")] public uint32 BitsPerSecond;
        [RTTI.Member(16, 0x54, "Format")] public uint16 BlockAlignment;
        [RTTI.Member(17, 0x56, "Format")] public uint16 FormatTag;
        [RTTI.Member(10, 0x5A, "Format")] public uint16 FrameSize;
        [RTTI.Member(9, 0x5C, "Format")] public int SampleCount;
    }
}
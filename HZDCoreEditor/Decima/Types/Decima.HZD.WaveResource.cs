using System;
using System.IO;

namespace Decima.HZD
{
    using uint8 = System.Byte;
    using uint16 = System.UInt16;
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x685DC980BBF316E3)]
    public class WaveResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(0, 0x0, "Format")] public EWaveDataEncodingQuality EncodingQuality;
        [RTTI.Member(1, 0x0, "Format")] public bool IsStreaming;
        [RTTI.Member(2, 0x0, "Format")] public bool UseVBR;
        [RTTI.Member(3, 0x28, "Data")] public Array<uint8> WaveData;
        [RTTI.Member(4, 0x38, "Data")] public uint WaveDataSize;
        [RTTI.Member(5, 0x48, "Format")] public int SampleRate;
        [RTTI.Member(6, 0x4C, "Format")] public uint8 ChannelCount;
        [RTTI.Member(7, 0x50, "Format")] public EWaveDataEncoding Encoding;
        [RTTI.Member(8, 0x54, "Format")] public uint16 BitsPerSample;
        [RTTI.Member(9, 0x58, "Format")] public uint32 BitsPerSecond;
        [RTTI.Member(10, 0x5C, "Format")] public uint16 BlockAlignment;
        [RTTI.Member(11, 0x5E, "Format")] public uint16 FormatTag;
        [RTTI.Member(12, 0x62, "Format")] public uint16 FrameSize;
        [RTTI.Member(13, 0x64, "Format")] public int SampleCount;
        StreamHandle StreamInfo;

        [Flags]
        private enum Flags : byte
        {
            Streaming = 1,
            VBR = 2,
            EncodingQualityMask = 15,
        }

        public void DeserializeExtraData(BinaryReader reader)
        {
            if (IsStreaming)
                StreamInfo = StreamHandle.FromData(reader);
        }

        public byte EncodeFlags()
        {
            byte flags = 0;
            flags |= (byte)(IsStreaming ? Flags.Streaming : 0);
            flags |= (byte)(UseVBR ? Flags.VBR : 0);
            flags |= (byte)(((byte)EncodingQuality & (byte)Flags.EncodingQualityMask) << 2);

            return flags;
        }

        public void DecodeFlags(byte value)
        {
            IsStreaming = (value & (byte)Flags.Streaming) != 0;
            UseVBR = (value & (byte)Flags.VBR) != 0;
            EncodingQuality = (EWaveDataEncodingQuality)((value >> 2) & (byte)Flags.EncodingQualityMask);
        }
    }
}

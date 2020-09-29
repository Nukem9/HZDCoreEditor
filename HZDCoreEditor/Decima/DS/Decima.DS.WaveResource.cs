#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.
#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.

namespace Decima.DS
{
    using int8 = System.SByte;
    using uint8 = System.Byte;
    using int16 = System.Int16;
    using uint16 = System.UInt16;
    using int32 = System.Int32;
    using uint32 = System.UInt32;
    using int64 = System.Int64;
    using uint64 = System.UInt64;

    using wchar = System.Int16;
    using ucs4 = System.Int32;

    using HalfFloat = System.UInt16;
    using LinearGainFloat = System.Single;
    using MusicTime = System.UInt64;

    using MaterialType = System.UInt16;
    using AnimationNodeID = System.UInt16;
    using AnimationTagID = System.UInt32;
    using AnimationSet = System.UInt32;
    using AnimationStateID = System.UInt32;
    using AnimationEventID = System.UInt32;
    using PhysicsCollisionFilterInfo = System.UInt32;

[RTTI.Serializable(0x257040983B11DA11, GameType.DS)]
public class WaveResource : Resource, RTTI.IExtraBinaryDataCallback
{
	[RTTI.Member(0, 0x0, "Format")] public bool IsStreaming;
	[RTTI.Member(1, 0x0, "Format")] public bool UseVBR;
	[RTTI.Member(2, 0x0, "Format")] public EWaveDataEncodingQuality EncodingQuality;
	[RTTI.Member(3, 0x20, "Data")] public Array<uint8> WaveData;
	[RTTI.Member(4, 0x30, "Data")] public uint WaveDataSize;
	[RTTI.Member(5, 0x40, "Format")] public int SampleRate;
	[RTTI.Member(6, 0x44, "Format")] public uint8 ChannelCount;
	[RTTI.Member(7, 0x48, "Format")] public EWaveDataEncoding Encoding;
	[RTTI.Member(8, 0x4C, "Format")] public uint16 BitsPerSample;
	[RTTI.Member(9, 0x50, "Format")] public uint32 BitsPerSecond;
	[RTTI.Member(10, 0x54, "Format")] public uint16 BlockAlignment;
	[RTTI.Member(11, 0x56, "Format")] public uint16 FormatTag;
	[RTTI.Member(12, 0x5A, "Format")] public uint16 FrameSize;
	[RTTI.Member(13, 0x5C, "Format")] public int SampleCount;
}

}

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

[RTTI.Serializable(0x4C7DBDD040598FCC, GameType.DS)]
public class MusicResource : Resource
{
	[RTTI.Member(0, 0x20)] public Array<uint8> WorkspaceBuffer;
	[RTTI.Member(1, 0x30)] public Array<StreamingDataSource> StreamingDataSources;
	[RTTI.Member(2, 0x40)] public Array<uint32> StreamingDataHash;
	[RTTI.Member(3, 0x50)] public int BitRate;
	[RTTI.Member(4, 0x54)] public bool StripSilence;
	[RTTI.Member(5, 0x58)] public int StripSilenceThreshold;
	[RTTI.Member(6, 0x60)] public Array<MusicSubmixBinding> SubmixBindings;
}

}

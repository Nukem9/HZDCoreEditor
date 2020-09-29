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

[RTTI.Serializable(0xC726DF870437D774, GameType.DS)]
public class LocalizedSimpleSoundResource : WwiseSimpleSoundResource, RTTI.IExtraBinaryDataCallback
{
	[RTTI.Member(0, 0x120, "General")] public Ref<SoundMixStateResource> SoundMixState;
	[RTTI.Member(1, 0x128, "General")] public Ref<LocalizedSoundPreset> Preset;
	[RTTI.Member(2, 0x130, "General")] public Array<float> LengthInSeconds;
	[RTTI.Member(3, 0x140, "General")] public Array<uint32> WemIDs;
	[RTTI.Member(4, 0x150, "General")] public float VolumeCorrectionRTPCValue;
}

}

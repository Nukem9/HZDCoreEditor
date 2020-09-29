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

[RTTI.Serializable(0x3B616D6260E90151, GameType.DS)]
public class QuestSave : RTTIObject
{
	[RTTI.Member(0, 0x10, "StateSaving", true)] public GGUUID QuestResourceUUID;
	[RTTI.Member(1, 0x20, "StateSaving", true)] public EQuestState State;
	[RTTI.Member(2, 0x24, "StateSaving", true)] public bool Tracked;
	[RTTI.Member(3, 0x25, "StateSaving", true)] public bool TrackingEnabled;
	[RTTI.Member(4, 0x26, "StateSaving", true)] public EQuestRunState RunState;
	[RTTI.Member(5, 0x28, "StateSaving", true)] public int StartTime;
	[RTTI.Member(6, 0x2C, "StateSaving", true)] public int LastProgressTime;
	[RTTI.Member(7, 0x30, "StateSaving", true)] public bool RewindCounter;
	[RTTI.Member(8, 0x58, "StateSaving", true)] public int Version;
}

}

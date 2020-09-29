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

[RTTI.Serializable(0x7520F5D9A86D15E3, GameType.DS)]
public class GeneratedQuestSave : RTTIObject
{
	[RTTI.Member(0, 0x10, "Saving", true)] public GGUUID QuestUUID;
	[RTTI.Member(1, 0x20, "Saving", true)] public GGUUID StartUUID;
	[RTTI.Member(2, 0x30, "Saving", true)] public GGUUID EndUUID;
	[RTTI.Member(3, 0x40, "Saving", true)] public GGUUID SubSectionUUID;
	[RTTI.Member(4, 0x50, "Saving", true)] public GGUUID MainObjectiveUUID;
	[RTTI.Member(5, 0x60, "Saving", true)] public GGUUID FinishObjectiveUUID;
	[RTTI.Member(6, 0x70, "Saving", true)] public GGUUID TriggerUUID;
	[RTTI.Member(7, 0x90, "Saving", true)] public GGUUID Recipe;
	[RTTI.Member(8, 0xA0, "Saving", true)] public GGUUID TradingItem;
	[RTTI.Member(9, 0xC0, "Saving", true)] public GGUUID TurnInLocationUUID;
	[RTTI.Member(10, 0xD0, "Saving", true)] public GGUUID MerchantSpawnSetupUUID;
}

}

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

[RTTI.Serializable(0xC0E251CE6C81D04D, GameType.DS)]
public class RotMatrix
{
	[RTTI.Member(0, 0x0)] public Vec3Pack Col0;
	[RTTI.Member(1, 0xC)] public Vec3Pack Col1;
	[RTTI.Member(2, 0x18)] public Vec3Pack Col2;
}

}

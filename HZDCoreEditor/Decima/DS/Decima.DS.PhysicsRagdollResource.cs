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

[RTTI.Serializable(0xE034B1EFA4CAE66C, GameType.DS)]
public class PhysicsRagdollResource : PhysicsResource, RTTI.IExtraBinaryDataCallback
{
	[RTTI.Member(0, 0x30, "General")] public float BuoyancyFactor;
	[RTTI.Member(1, 0x34, "General")] public float DragFactor;
	[RTTI.Member(2, 0x38, "General")] public float ImpulseFactor;
	[RTTI.Member(3, 0x40, "General")] public Ref<MaterialTypeResource> MaterialTypeResource;
	[RTTI.Member(4, 0x48, "General")] public Ref<PhysicsSkeleton> PhysicsSkeleton;
	[RTTI.Member(5, 0x50, "General")] public Array<int> BodiesTriggeringContactPoints;
	[RTTI.Member(6, 0x70, "General")] public int CollisionLayer;
	[RTTI.Member(7, 0x78, "General")] public Ref<PhysicsCollisionGroupsResource> CollisionGroups;
	[RTTI.Member(8, 0x80, "General")] public float MaxAngularVelocity;
	[RTTI.Member(9, 0x84, "General")] public float MaxLinearVelocity;
	[RTTI.Member(10, 0x88, "General")] public float MaxContactImpulse;
}

}

namespace Decima.DS
{
    [RTTI.Serializable(0xE034B1EFA4CAE66C, GameType.DS)]
    public class PhysicsRagdollResource : PhysicsResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(9, 0x30, "General")] public float BuoyancyFactor;
        [RTTI.Member(10, 0x34, "General")] public float DragFactor;
        [RTTI.Member(11, 0x38, "General")] public float ImpulseFactor;
        [RTTI.Member(12, 0x40, "General")] public Ref<MaterialTypeResource> MaterialTypeResource;
        [RTTI.Member(8, 0x48, "General")] public Ref<PhysicsSkeleton> PhysicsSkeleton;
        [RTTI.Member(13, 0x50, "General")] public Array<int> BodiesTriggeringContactPoints;
        [RTTI.Member(14, 0x70, "General")] public int CollisionLayer;
        [RTTI.Member(15, 0x78, "General")] public Ref<PhysicsCollisionGroupsResource> CollisionGroups;
        [RTTI.Member(16, 0x80, "General")] public float MaxAngularVelocity;
        [RTTI.Member(17, 0x84, "General")] public float MaxLinearVelocity;
        [RTTI.Member(18, 0x88, "General")] public float MaxContactImpulse;
    }
}
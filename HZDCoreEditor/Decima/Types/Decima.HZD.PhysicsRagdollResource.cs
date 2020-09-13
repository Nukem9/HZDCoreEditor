using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    using MaterialType = System.UInt16;

    [RTTI.Serializable(0x3FCEBC6413D208FD)]
    public class PhysicsRagdollResource : PhysicsResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(0, 0x38, "General")] public float BuoyancyFactor;
        [RTTI.Member(1, 0x3C, "General")] public float DragFactor;
        [RTTI.Member(2, 0x40, "General")] public float ImpulseFactor;
        [RTTI.Member(3, 0x44, "General")] public MaterialType MaterialType;
        [RTTI.Member(4, 0x48, "General")] public Ref<PhysicsSkeleton> PhysicsSkeleton;
        [RTTI.Member(5, 0x50, "General")] public Array<int> BodiesTriggeringContactPoints;
        [RTTI.Member(6, 0x70, "General")] public int CollisionLayer;
        [RTTI.Member(7, 0x78, "General")] public Ref<PhysicsCollisionGroupsResource> CollisionGroups;
        [RTTI.Member(8, 0x80, "General")] public float MaxAngularVelocity;
        [RTTI.Member(9, 0x84, "General")] public float MaxLinearVelocity;
        [RTTI.Member(10, 0x88, "General")] public float MaxContactImpulse;
        [RTTI.Member(11, 0x8C, "General")] public int PoolSize;
        public byte[] HavokData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            // Generic havok reader / Havok 2014 HKX file ("hk_2014.2.0-r1")
            uint havokDataLength = reader.ReadUInt32();
            HavokData = reader.ReadBytesStrict(havokDataLength);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)HavokData.Length);
            writer.Write(HavokData);
        }
    }
}

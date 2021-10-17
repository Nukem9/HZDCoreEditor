using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xB36C3ADC211AB947, GameType.DS)]
    public class StaticMeshResource : MeshResourceBase, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(16, 0x0, "Lighting", true)] public EDrawPartType RenderType;
        [RTTI.Member(14, 0x0, "Lighting", true)] public bool CastShadows;
        [RTTI.Member(15, 0x0, "Lighting", true)] public bool CastDynamicShadows;
        [RTTI.Member(17, 0x0, "Lighting", true)] public EViewLayer ViewLayer;
        [RTTI.Member(19, 0x0, "Lighting", true)] public bool VoxelizeBaking;
        [RTTI.Member(18, 0x0, "Lighting", true)] public EShadowCull ShadowCullMode;
        [RTTI.Member(13, 0x80, "Lighting")] public DrawFlags DrawFlags;
        [RTTI.Member(8, 0x88, "MeshDescription")] public Array<Ref<PrimitiveResource>> Primitives;
        [RTTI.Member(7, 0x98, "MeshDescription")] public Array<Ref<ShadingGroup>> ShadingGroups;
        [RTTI.Member(9, 0xB8, "MeshDescription")] public Ref<SkeletonHelpers> OrientationHelpers;
        [RTTI.Member(10, 0xC0, "MeshDescription")] public Ref<StaticMeshSimulationInfo> SimulationInfo;
        [RTTI.Member(11, 0x108, "MeshDescription")] public Ref<KJPRenderEffectSwapper> RenderEffectSwapper;
        [RTTI.Member(20, 0x110, "Lighting")] public bool DisableSunCascade1;
        public StreamHandle StreamInfo;

        public void DeserializeExtraData(BinaryReader reader)
        {
            StreamInfo = StreamHandle.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            StreamInfo.ToData(writer);
        }
    }
}
namespace Decima.HZD
{
    [RTTI.Serializable(0xEC711C9C5BD00A78, GameType.HZD)]
    public class StaticMeshResource : MeshResourceBase
    {
        [RTTI.Member(18, 0x0, "Lighting", true)] public EDrawPartType RenderType;
        [RTTI.Member(17, 0x0, "Lighting", true)] public bool CastDynamicShadows;
        [RTTI.Member(16, 0x0, "Lighting", true)] public bool CastShadows;
        [RTTI.Member(20, 0x0, "Lighting", true)] public EShadowCull ShadowCullMode;
        [RTTI.Member(19, 0x0, "Lighting", true)] public EViewLayer ViewLayer;
        [RTTI.Member(14, 0x80, "Lighting")] public DrawFlags DrawFlags;
        [RTTI.Member(9, 0x88, "MeshDescription")] public Array<Ref<PrimitiveResource>> Primitives;
        [RTTI.Member(10, 0xA8, "MeshDescription")] public Array<Ref<RenderEffectResource>> RenderEffects;
        [RTTI.Member(11, 0xB8, "MeshDescription")] public Ref<SkeletonHelpers> OrientationHelpers;
        [RTTI.Member(12, 0xC0, "MeshDescription")] public Ref<StaticMeshSimulationInfo> SimulationInfo;
        [RTTI.Member(15, 0xD0, "Lighting")] public bool SupportsInstanceRendering;
    }
}
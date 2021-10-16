namespace Decima.HZD
{
    [RTTI.Serializable(0x98681A4BAF459D6E, GameType.HZD)]
    public class RegularSkinnedMeshResource : RegularSkinnedMeshResourceBase
    {
        [RTTI.Member(25, 0xC0)] public Vec3 PositionBoundsScale;
        [RTTI.Member(26, 0xD0)] public Vec3 PositionBoundsOffset;
        [RTTI.Member(22, 0xE0)] public Ref<RegularSkinnedMeshResourceSkinInfo> SkinInfo;
        [RTTI.Member(23, 0xE8)] public Array<Ref<PrimitiveResource>> Primitives;
        [RTTI.Member(24, 0xF8)] public Array<Ref<RenderEffectResource>> RenderFxResources;
    }
}
using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xE2A812418ABC2172, GameType.DS)]
    public class RegularSkinnedMeshResource : RegularSkinnedMeshResourceBase, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(24, 0xC0)] public Vec3 PositionBoundsScale;
        [RTTI.Member(25, 0xD0)] public Vec3 PositionBoundsOffset;
        [RTTI.Member(22, 0xE0)] public Ref<RegularSkinnedMeshResourceSkinInfo> SkinInfo;
        [RTTI.Member(23, 0xE8)] public Array<Ref<PrimitiveResource>> Primitives;
        [RTTI.Member(21, 0xF8)] public Array<Ref<ShadingGroup>> ShadingGroups;
        [RTTI.Member(26, 0x140)] public Ref<KJPRenderEffectSwapper> RenderEffectSwapper;
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
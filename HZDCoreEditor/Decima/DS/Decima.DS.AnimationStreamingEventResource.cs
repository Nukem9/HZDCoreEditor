using System.IO;

namespace Decima.DS
{
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x8DFB7682791AD065, GameType.DS)]
    public class AnimationStreamingEventResource : TimedEventResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(7, 0x30)] public Array<Ref<AnimationStreamingActorResource>> Actors;
        [RTTI.Member(8, 0x40)] public Array<AnimationStreamingSkipBlockInfo> SkipBlockInfo;
        [RTTI.Member(9, 0x50)] public uint32 BlockFrameLength;
        [RTTI.Member(10, 0x54)] public uint32 MaxBlockSize;
        [RTTI.Member(11, 0x58)] public uint32 TotalBlockSize;
        [RTTI.Member(14, 0x5C, "AnimationCompression")] public uint32 RotationPrecision;
        [RTTI.Member(12, 0x60)] public Ref<AnimationStreamingBlockResource> FirstBlock;
        [RTTI.Member(15, 0x70, "AnimationCompression")] public bool DebugFullKey;
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
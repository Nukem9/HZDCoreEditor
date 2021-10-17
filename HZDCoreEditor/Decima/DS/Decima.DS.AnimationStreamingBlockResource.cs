using HZDCoreEditor.Util;
using System.IO;

namespace Decima.DS
{
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x86927412B5C57A62, GameType.DS)]
    public class AnimationStreamingBlockResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(2, 0x20)] public uint32 BlockSize;
        public byte[] UnknownData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            UnknownData = reader.ReadBytesStrict(BlockSize);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write(UnknownData);
        }
    }
}
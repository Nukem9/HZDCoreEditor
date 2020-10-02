using System.IO;
using Utility;

namespace Decima.DS
{
    [RTTI.Serializable(0x56A2D872D9734B33, GameType.DS)]
    public class MorphemeAnimationResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        public byte[] MorphemeData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint morphemeDataLength = reader.ReadUInt32();
            MorphemeData = reader.ReadBytesStrict(morphemeDataLength);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)MorphemeData.Length);
            writer.Write(MorphemeData);
        }
    }
}
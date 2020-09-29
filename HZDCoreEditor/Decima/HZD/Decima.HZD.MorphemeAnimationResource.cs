using Utility;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x985D2CD65C07569F, GameType.HZD)]
    public class MorphemeAnimationResource : ResourceWithoutLegacyName, RTTI.IExtraBinaryDataCallback
    {
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

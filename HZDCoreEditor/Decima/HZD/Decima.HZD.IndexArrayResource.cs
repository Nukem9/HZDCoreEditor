using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xA94A831CA5252531, GameType.HZD)]
    public class IndexArrayResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public HwIndexArray Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwIndexArray.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);
        }
    }
}

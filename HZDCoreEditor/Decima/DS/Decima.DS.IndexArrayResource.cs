using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x5FE633B37CEDBF84, GameType.DS)]
    public class IndexArrayResource : Resource, RTTI.IExtraBinaryDataCallback
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

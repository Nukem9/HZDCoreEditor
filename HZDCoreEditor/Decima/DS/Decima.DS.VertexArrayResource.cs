using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x3AC29A123FAABAB4, GameType.DS)]
    public class VertexArrayResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwVertexArray Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwVertexArray.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);
        }
    }
}

using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xBBAB0E0254767A94, GameType.HZD)]
    public class VertexArrayResource : BaseResource, RTTI.IExtraBinaryDataCallback
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

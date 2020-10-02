using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8E09340D7626AFAE, GameType.HZD)]
    public class DataBufferResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public HwDataBuffer Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwDataBuffer.FromData(reader, GameType.HZD);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);
        }
    }
}
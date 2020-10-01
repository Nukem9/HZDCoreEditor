using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xA4341E94120AA306, GameType.DS)]
    public class DataBufferResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwDataBuffer Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwDataBuffer.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);
        }
    }
}

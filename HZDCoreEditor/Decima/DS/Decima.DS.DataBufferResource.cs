using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xA4341E94120AA306, GameType.DS)]
    public class DataBufferResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwDataBuffer Buffer;
        public StreamHandle StreamInfo;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwDataBuffer.FromData(reader, GameType.DS);

            if (Buffer.Buffer.Streaming)
                StreamInfo = StreamHandle.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);

            if (Buffer.Buffer.Streaming)
                StreamInfo.ToData(writer);
        }
    }
}
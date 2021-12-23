using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xA4341E94120AA306, GameType.DS)]
    public class DataBufferResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwDataBuffer DataBuffer;
        public StreamHandle StreamInfo;

        public void DeserializeExtraData(BinaryReader reader)
        {
            DataBuffer = HwDataBuffer.FromData(reader, GameType.DS);

            if (DataBuffer.Buffer.StreamingMode == BaseRenderDataStreamingMode.Streaming)
                StreamInfo = StreamHandle.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            DataBuffer.ToData(writer);

            if (DataBuffer.Buffer.StreamingMode == BaseRenderDataStreamingMode.Streaming)
                StreamInfo.ToData(writer);
        }
    }
}
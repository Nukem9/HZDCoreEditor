using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8E09340D7626AFAE)]
    public class DataBufferResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public HwBuffer Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint bufferElementCount = reader.ReadUInt32();

            if (bufferElementCount > 0)
            {
                uint isStreaming = reader.ReadUInt32();
                uint flags = reader.ReadUInt32();
                var format = (EDataBufferFormat)reader.ReadUInt32();
                uint bufferStride = reader.ReadUInt32();

                if (isStreaming != 0 && isStreaming != 1)
                    throw new InvalidDataException("Must be true or false");

                if (format != EDataBufferFormat.Structured)
                    bufferStride = HwBuffer.GetStrideForFormat(format);

                Buffer = HwBuffer.FromData(reader, format, isStreaming != 0, bufferStride, bufferElementCount);
            }
        }
    }
}
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8E09340D7626AFAE)]
    public class DataBufferResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        uint Flags;
        public HwBuffer Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint bufferElementCount = reader.ReadUInt32();

            if (bufferElementCount > 0)
            {
                uint isStreaming = reader.ReadUInt32();
                Flags = reader.ReadUInt32();
                var format = (EDataBufferFormat)reader.ReadUInt32();
                uint bufferStride = reader.ReadUInt32();

                if (isStreaming != 0 && isStreaming != 1)
                    throw new InvalidDataException("Must be true or false");

                if (format != EDataBufferFormat.Structured)
                    bufferStride = HwBuffer.GetStrideForFormat(format);

                Buffer = HwBuffer.FromData(reader, format, isStreaming != 0, bufferStride, bufferElementCount);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            uint elementCount = Buffer?.ElementCount ?? 1;
            writer.Write(elementCount);

            if (elementCount > 0)
            {
                writer.Write((uint)(Buffer.IsStreamed() ? 1 : 0));
                writer.Write(Flags);
                writer.Write((uint)Buffer.Format);
                writer.Write(Buffer.ElementStride);
                Buffer.ToData(writer);
            }
        }
    }
}
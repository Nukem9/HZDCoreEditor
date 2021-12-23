using System.IO;

namespace Decima
{
    public class HwDataBuffer
    {
        public uint Flags;
        public HwBuffer Buffer;

        public void ToData(BinaryWriter writer)
        {
            uint elementCount = Buffer?.ElementCount ?? 0;
            writer.Write(elementCount);

            if (elementCount > 0)
            {
                writer.Write((uint)Buffer.StreamingMode);
                writer.Write(Flags);
                writer.Write((uint)Buffer.Format);
                writer.Write(Buffer.ElementStride);
                Buffer.ToData(writer);
            }
        }

        public static HwDataBuffer FromData(BinaryReader reader, GameType gameType)
        {
            var buffer = new HwDataBuffer();
            uint bufferElementCount = reader.ReadUInt32();

            if (bufferElementCount > 0)
            {
                var streamingMode = (BaseRenderDataStreamingMode)reader.ReadUInt32();
                buffer.Flags = reader.ReadUInt32();
                var format = (BaseDataBufferFormat)reader.ReadUInt32();
                uint bufferStride = reader.ReadUInt32();

                if (format != BaseDataBufferFormat.Structured)
                    bufferStride = HwBuffer.GetStrideForFormat(format);

                buffer.Buffer = HwBuffer.FromData(reader, gameType, format, streamingMode, bufferStride, bufferElementCount);
            }

            return buffer;
        }
    }
}
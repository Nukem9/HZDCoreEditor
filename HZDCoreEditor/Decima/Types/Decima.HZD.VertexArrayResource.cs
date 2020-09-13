using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xBBAB0E0254767A94)]
    public class VertexArrayResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public uint VertexElementCount; // TODO: Determine from HwBuffer entries (what happens if there's none?)
        public VertexStream[] Streams;
        public bool IsStreaming;        // TODO: Determine from HwBuffer entries (what happens if there's none?)

        public class VertexStream
        {
            public uint Flags;
            public byte[][] UnknownData;
            public GGUUID GUID;
            public HwBuffer Buffer;
        }

        public void DeserializeExtraData(BinaryReader reader)
        {
            VertexElementCount = reader.ReadUInt32();
            uint vertexStreamCount = reader.ReadUInt32();
            IsStreaming = reader.ReadBooleanStrict();

            Streams = new VertexStream[vertexStreamCount];

            for (int i = 0; i < Streams.Length; i++)
            {
                var stream = new VertexStream();
                Streams[i] = stream;

                stream.Flags = reader.ReadUInt32();
                uint vertexByteStride = reader.ReadUInt32();
                stream.UnknownData = new byte[reader.ReadUInt32()][];

                for (uint j = 0; j < stream.UnknownData.Length; j++)
                {
                    // 4 bytes read separately
                    stream.UnknownData[j] = reader.ReadBytesStrict(4);
                }

                stream.GUID = GGUUID.FromData(reader);
                stream.Buffer = HwBuffer.FromVertexData(reader, IsStreaming, vertexByteStride, VertexElementCount);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write(VertexElementCount);
            writer.Write((uint)Streams.Length);
            writer.Write(IsStreaming);

            foreach (VertexStream stream in Streams)
            {
                writer.Write(stream.Flags);
                writer.Write(stream.Buffer.ElementStride);
                writer.Write((uint)stream.UnknownData.Length);

                foreach (var bytes in stream.UnknownData)
                    writer.Write(bytes);

                stream.GUID.ToData(writer);
                stream.Buffer.ToData(writer);
            }
        }
    }
}

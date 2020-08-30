using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xBBAB0E0254767A94)]
    public class VertexArrayResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public HwBuffer[] Buffers;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint vertexElementCount = reader.ReadUInt32();
            uint vertexStreamCount = reader.ReadUInt32();
            bool isStreaming = reader.ReadBooleanStrict();

            Buffers = new HwBuffer[vertexStreamCount];

            for (uint i = 0; i < vertexStreamCount; i++)
            {
                uint unknownFlags = reader.ReadUInt32();
                uint vertexByteStride = reader.ReadUInt32();
                uint unknownCounter = reader.ReadUInt32();

                for (uint j = 0; j < unknownCounter; j++)
                {
                    // 4 bytes read separately
                    var packedData = reader.ReadBytesStrict(4);
                }

                // Likely GUID or at least an identifier
                var resourceGUID = reader.ReadBytesStrict(16);

                Buffers[i] = HwBuffer.FromVertexData(reader, isStreaming, vertexByteStride, vertexElementCount);
            }
        }
    }
}

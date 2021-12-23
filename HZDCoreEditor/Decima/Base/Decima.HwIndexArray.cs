using System;
using System.IO;

namespace Decima
{
    public class HwIndexArray
    {
        public uint Flags;
        public BaseGGUUID ResourceDataHash;// Actually MurmurHashValue
        public HwBuffer Buffer;

        public void ToData(BinaryWriter writer)
        {
            var dataFormat = Buffer.Format switch
            {
                BaseDataBufferFormat.R_UINT_16 => BaseIndexFormat.Index16,
                BaseDataBufferFormat.R_UINT_32 => BaseIndexFormat.Index32,
                _ => throw new NotSupportedException("Unknown index buffer type"),
            };

            uint elementCount = Buffer?.ElementCount ?? 0;
            writer.Write(elementCount);

            if (elementCount > 0)
            {
                writer.Write(Flags);
                writer.Write((uint)dataFormat);
                writer.Write((uint)Buffer.StreamingMode);
                ResourceDataHash.ToData(writer);
                Buffer.ToData(writer);
            }
        }

        public static HwIndexArray FromData(BinaryReader reader, GameType gameType)
        {
            var array = new HwIndexArray();
            uint indexElementCount = reader.ReadUInt32();

            if (indexElementCount > 0)
            {
                array.Flags = reader.ReadUInt32();
                var format = (BaseIndexFormat)reader.ReadUInt32();
                var streamingMode = (BaseRenderDataStreamingMode)reader.ReadUInt32();

                array.ResourceDataHash = BaseGGUUID.FromData(reader);
                array.Buffer = HwBuffer.FromIndexData(reader, gameType, format, streamingMode, indexElementCount);
            }

            return array;
        }
    }
}
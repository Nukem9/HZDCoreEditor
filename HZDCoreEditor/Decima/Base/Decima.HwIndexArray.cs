using System;
using System.IO;

namespace Decima
{
    public class HwIndexArray
    {
        public uint Flags;
        public BaseGGUUID ResourceGUID;
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
                writer.Write((uint)(Buffer.Streaming ? 1 : 0));
                ResourceGUID.ToData(writer);
                Buffer.ToData(writer);
            }
        }

        public static HwIndexArray FromData(BinaryReader reader, GameType gameType)
        {
            var x = new HwIndexArray();
            uint indexElementCount = reader.ReadUInt32();

            if (indexElementCount > 0)
            {
                x.Flags = reader.ReadUInt32();
                var format = (BaseIndexFormat)reader.ReadUInt32();
                uint isStreaming = reader.ReadUInt32();

                if (isStreaming != 0 && isStreaming != 1)
                    throw new InvalidDataException("Must be true or false");

                x.ResourceGUID = BaseGGUUID.FromData(reader);
                x.Buffer = HwBuffer.FromIndexData(reader, gameType, format, isStreaming != 0, indexElementCount);
            }

            return x;
        }
    }
}
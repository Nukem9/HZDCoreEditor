using System;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xA94A831CA5252531)]
    public class IndexArrayResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public uint Flags;
        public GGUUID ResourceGUID;
        public HwBuffer Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint indexElementCount = reader.ReadUInt32();

            if (indexElementCount > 0)
            {
                Flags = reader.ReadUInt32();
                var format = (EIndexFormat)reader.ReadUInt32();
                uint isStreaming = reader.ReadUInt32();

                if (isStreaming != 0 && isStreaming != 1)
                    throw new InvalidDataException("Must be true or false");

                ResourceGUID = GGUUID.FromData(reader);
                Buffer = HwBuffer.FromIndexData(reader, format, isStreaming != 0, indexElementCount);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            var dataFormat = Buffer.Format switch
            {
                EDataBufferFormat.R_UINT_16 => EIndexFormat.Index16,
                EDataBufferFormat.R_UINT_32 => EIndexFormat.Index32,
                _ => throw new NotSupportedException("Unknown index buffer type"),
            };

            uint elementCount = Buffer?.ElementCount ?? 1;
            writer.Write(elementCount);

            if (elementCount > 0)
            {
                writer.Write(Flags);
                writer.Write((uint)dataFormat);
                writer.Write((uint)(Buffer.IsStreamed() ? 1 : 0));
                ResourceGUID.ToData(writer);
                Buffer.ToData(writer);
            }
        }
    }
}

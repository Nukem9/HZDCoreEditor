using BinaryStreamExtensions;
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
    }
}

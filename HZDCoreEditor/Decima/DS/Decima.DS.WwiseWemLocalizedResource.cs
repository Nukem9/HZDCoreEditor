using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x3DE345CD4B0C7E72, GameType.DS)]
    public class WwiseWemLocalizedResource : WwiseWemResource, RTTI.IExtraBinaryDataCallback
    {
        private static readonly int[] LanguageBitIndices = new int[]
        {
            0x1, 0x2, 0x5, 0x4,
            0x3, 0x7, 0x17, 0xB,
            0xA, 0x11, 0x12, 0x10
        };

        public uint LanguageBitFlags;
        public StreamHandle[] StreamHandles;
        public ulong[] UnknownStreamData;

        public override void DeserializeExtraData(BinaryReader reader)
        {
            LanguageBitFlags = reader.ReadUInt32();

            StreamHandles = new StreamHandle[LanguageBitIndices.Length];
            UnknownStreamData = new ulong[LanguageBitIndices.Length];

            for (uint i = 0; i < LanguageBitIndices.Length; i++)
            {
                if ((LanguageBitFlags & (1 << LanguageBitIndices[i])) != 0)
                {
                    StreamHandles[i] = StreamHandle.FromData(reader);
                    UnknownStreamData[i] = reader.ReadUInt64();
                }
            }
        }

        public override void SerializeExtraData(BinaryWriter writer)
        {
            // TODO: How are the bit flags calculated?
            writer.Write(LanguageBitFlags);

            for (uint i = 0; i < LanguageBitIndices.Length; i++)
            {
                if ((LanguageBitFlags & (1 << LanguageBitIndices[i])) != 0)
                {
                    StreamHandles[i].ToData(writer);
                    writer.Write(UnknownStreamData[i]);
                }
            }
        }
    }
}
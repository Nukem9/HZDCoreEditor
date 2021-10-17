using System.Collections.Generic;
using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x3DE345CD4B0C7E72, GameType.DS)]
    public class WwiseWemLocalizedResource : WwiseWemResource, RTTI.IExtraBinaryDataCallback
    {
        private static readonly int[] UnknownBitIndices = new int[]
        {
            0x1, 0x2, 0x5, 0x4,
            0x3, 0x7, 0x17, 0xB,
            0xA, 0x11, 0x12, 0x10
        };

        public List<(StreamHandle, ulong)> WemStreamHandles;

        public override void DeserializeExtraData(BinaryReader reader)
        {
            uint bitFlags = reader.ReadUInt32();
            WemStreamHandles = new List<(StreamHandle, ulong)>(UnknownBitIndices.Length);

            for (uint i = 0; i < UnknownBitIndices.Length; i++)
            {
                if ((bitFlags & (1 << UnknownBitIndices[i])) != 0)
                {
                    var handle = StreamHandle.FromData(reader);
                    var unknown = reader.ReadUInt64();

                    WemStreamHandles.Add((handle, unknown));
                }
            }
        }

        public override void SerializeExtraData(BinaryWriter writer)
        {
            foreach (var wemHandle in WemStreamHandles)
            {
                wemHandle.Item1.ToData(writer);
                writer.Write(wemHandle.Item2);
            }
        }
    }
}
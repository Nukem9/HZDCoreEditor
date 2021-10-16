using System;

namespace HZDCoreEditor.Util
{
    internal static class CRC32C
    {
        private static readonly uint[] LookupTable;

        static CRC32C()
        {
            // Castagnoli-CRC used by SSE4.2 instructions
            LookupTable = new uint[256];

            for (uint i = 0; i < LookupTable.Length; i++)
            {
                uint r = i;

                for (int j = 0; j < 8; j++)
                    r = (r & 1) != 0 ? ((r >> 1) ^ 0x82F63B78) : (r >> 1);

                LookupTable[i] = r;
            }
        }

        public static uint Checksum(ReadOnlySpan<byte> data, uint seed = 0)
        {
            for (int i = 0; i < data.Length; i++)
                seed = LookupTable[(byte)seed ^ data[i]] ^ (seed >> 8);

            return seed;
        }
    }
}

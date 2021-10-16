using System;

namespace HZDCoreEditor.Util
{
    internal static class BitOperations
    {
        private static readonly int[] _lookup =
        {
            32, 0, 1, 26, 2, 23, 27, 0, 3, 16, 24, 30, 28, 11, 0, 13, 4, 7, 17,
            0, 25, 22, 31, 15, 29, 10, 12, 6, 0, 21, 14, 9, 5, 20, 8, 19, 18
        };

        public static int Log2(uint value)
        {
            return (int)((BitConverter.DoubleToInt64Bits(value) >> 52) + 1) & 0xFF;
        }

        public static uint RotateLeft(uint value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }

        public static int TrailingZeroCount(int blockSize)
        {
            return _lookup[(blockSize & -blockSize) % 37];
        }
    }
}

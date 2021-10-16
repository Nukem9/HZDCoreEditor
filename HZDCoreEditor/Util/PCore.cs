using System;

namespace HZDCoreEditor.Util
{
    internal static class PCore
    {
        public static void Quicksort<T>(T[] array, Func<T, T, bool> compare)
        {
            uint pivotSeed = 0;
            QuicksortImpl(array, 0, array.Length - 1, compare, ref pivotSeed);
        }

        private static void QuicksortImpl<T>(T[] array, int left, int right, Func<T, T, bool> compare, ref uint pivotSeed)
        {
            if (left >= right)
                return;

            void swap(int i1, int i2)
            {
                var temp = array[i2];
                array[i2] = array[i1];
                array[i1] = temp;
            }

            pivotSeed = 0x19660D * pivotSeed + 0x3C6EF35F;
            int pivot = (int)((pivotSeed >> 8) % (right - left));

            swap(left + pivot, right + 0);

            int start = left - 1;
            int end = right;

            while (start < end)
            {
                // Partition left side
                do
                {
                    start++;

                    if (!compare(array[start], array[right]))
                        break;

                } while (start < end);

                if (end <= start)
                    break;

                // Partition right side
                do
                {
                    end--;

                    if (!compare(array[right], array[end]))
                        break;

                } while (end > start);

                if (start >= end)
                    break;

                swap(end, start);
            }

            swap(start, right + 0);

            QuicksortImpl(array, left, start - 1, compare, ref pivotSeed);
            QuicksortImpl(array, start + 1, right, compare, ref pivotSeed);
        }
    }
}
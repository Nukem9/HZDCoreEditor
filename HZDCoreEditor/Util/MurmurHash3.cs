//-----------------------------------------------------------------------------
// MurmurHash3 was written by Austin Appleby, and is placed in the public
// domain. The author hereby disclaims copyright to this source code.

// Note - The x86 and x64 versions do _not_ produce the same results, as the
// algorithms are optimized for their respective platforms. You can still
// compile and run any of them on any platform, but your performance with the
// non-native version will be less than optimal.

// Note - This C# translation is placed in the public domain. It has been ported from
// https://github.com/aappleby/smhasher/blob/master/src/MurmurHash3.cpp (SMHasher)

//
// string str1 = "The quick brown fox jumps over the lazy dog";
// string str2 = "";
// 
// MurmurHash3.MurmurHash3_x86_32(Encoding.ASCII.GetBytes(str1), 42, out uint[] hash32_1);
// MurmurHash3.MurmurHash3_x86_32(Encoding.ASCII.GetBytes(str2), 42, out uint[] hash32_2);
// 
// if (hash32_1[0] != 0x347CA102)
//     Debugger.Break();
// 
// if (hash32_2[0] != 0x87FCD5C)
//     Debugger.Break();
// 
// MurmurHash3.MurmurHash3_x86_128(Encoding.ASCII.GetBytes(str1), 42, out uint[] hash128_1);
// MurmurHash3.MurmurHash3_x86_128(Encoding.ASCII.GetBytes(str2), 42, out uint[] hash128_2);
// 
// if (hash128_1[0] != 0xB0C69C19 || hash128_1[1] != 0xB1FD95C7 || hash128_1[2] != 0x4C746BD || hash128_1[3] != 0xB64FCFEC)
//     Debugger.Break();
// 
// if (hash128_2[0] != 0xAF6D2CB6 || hash128_2[1] != 0x95C80CBA || hash128_2[2] != 0x95C80CBA || hash128_2[3] != 0x95C80CBA)
//     Debugger.Break();
// 
// MurmurHash3.MurmurHash3_x64_128(Encoding.ASCII.GetBytes(str1), 42, out ulong[] hash128_3);
// MurmurHash3.MurmurHash3_x64_128(Encoding.ASCII.GetBytes(str2), 42, out ulong[] hash128_4);
// 
// if (hash128_3[0] != 0x740DCF93FE0BD5D7 || hash128_3[1] != 0xC4546CF4EC705C8F)
//     Debugger.Break();
// 
// if (hash128_4[0] != 0xF02AA77DFA1B8523 || hash128_4[1] != 0xD1016610DA11CBB9)
//     Debugger.Break();
//
using System;

namespace HZDCoreEditor.Util
{
    internal static class SMHasher
    {
        private static ulong BIG_CONSTANT(ulong x)
        {
            return x;
        }

        private static uint ROTL32(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static ulong ROTL64(ulong x, byte r)
        {
            return (x << r) | (x >> (64 - r));
        }

        private static uint getblock32(ReadOnlySpan<byte> p, int i)
        {
            return BitConverter.ToUInt32(p.Slice(i * 4, 4));
        }

        private static ulong getblock64(ReadOnlySpan<byte> p, int i)
        {
            return BitConverter.ToUInt64(p.Slice(i * 8, 8));
        }

        //-----------------------------------------------------------------------------
        // Finalization mix - force all bits of a hash block to avalanche

        private static uint fmix32(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        //----------

        private static ulong fmix64(ulong k)
        {
            k ^= k >> 33;
            k *= BIG_CONSTANT(0xff51afd7ed558ccd);
            k ^= k >> 33;
            k *= BIG_CONSTANT(0xc4ceb9fe1a85ec53);
            k ^= k >> 33;

            return k;
        }

        //-----------------------------------------------------------------------------

        public static void MurmurHash3_x86_32(ReadOnlySpan<byte> key, uint seed, out uint[] hash)
        {
            var data = key;
            int len = data.Length;
            int nblocks = len / 4;

            uint h1 = seed;

            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            //----------
            // body

            //const uint* blocks = (const uint*)(data + nblocks * 4);
            ReadOnlySpan<byte> blocks = data;

            //for (int i = -nblocks; i != 0; i++)
            for (int i = 0; i < nblocks; i++)
            {
                uint _k1 = getblock32(blocks, i);

                _k1 *= c1;
                _k1 = ROTL32(_k1, 15);
                _k1 *= c2;

                h1 ^= _k1;
                h1 = ROTL32(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;
            }

            //----------
            // tail

            //const uint8_t* tail = (const uint8_t*)(data + nblocks * 4);
            ReadOnlySpan<byte> tail = data.Slice((nblocks * 4), len - (nblocks * 4));

            uint k1 = 0;

            switch (len & 3)
            {
                case 3: k1 ^= (uint)tail[2] << 16; goto case 2;
                case 2: k1 ^= (uint)tail[1] << 8; goto case 1;
                case 1:
                    k1 ^= tail[0];
                    k1 *= c1; k1 = ROTL32(k1, 15); k1 *= c2; h1 ^= k1;
                    break;
            }

            //----------
            // finalization

            h1 ^= (uint)len;

            h1 = fmix32(h1);

            hash = new uint[1];
            hash[0] = h1;
        }

        //-----------------------------------------------------------------------------

        public static void MurmurHash3_x86_128(ReadOnlySpan<byte> key, uint seed, out uint[] hash)
        {
            var data = key;
            int len = data.Length;
            int nblocks = len / 16;

            uint h1 = seed;
            uint h2 = seed;
            uint h3 = seed;
            uint h4 = seed;

            const uint c1 = 0x239b961b;
            const uint c2 = 0xab0e9789;
            const uint c3 = 0x38b34ae5;
            const uint c4 = 0xa1e38b93;

            //----------
            // body

            //const uint* blocks = (const uint*)(data + nblocks * 16);
            ReadOnlySpan<byte> blocks = data;

            //for (int i = -nblocks; i != 0; i++)
            for (int i = 0; i < nblocks; i++)
            {
                uint _k1 = getblock32(blocks, i * 4 + 0);
                uint _k2 = getblock32(blocks, i * 4 + 1);
                uint _k3 = getblock32(blocks, i * 4 + 2);
                uint _k4 = getblock32(blocks, i * 4 + 3);

                _k1 *= c1; _k1 = ROTL32(_k1, 15); _k1 *= c2; h1 ^= _k1;

                h1 = ROTL32(h1, 19); h1 += h2; h1 = h1 * 5 + 0x561ccd1b;

                _k2 *= c2; _k2 = ROTL32(_k2, 16); _k2 *= c3; h2 ^= _k2;

                h2 = ROTL32(h2, 17); h2 += h3; h2 = h2 * 5 + 0x0bcaa747;

                _k3 *= c3; _k3 = ROTL32(_k3, 17); _k3 *= c4; h3 ^= _k3;

                h3 = ROTL32(h3, 15); h3 += h4; h3 = h3 * 5 + 0x96cd1c35;

                _k4 *= c4; _k4 = ROTL32(_k4, 18); _k4 *= c1; h4 ^= _k4;

                h4 = ROTL32(h4, 13); h4 += h1; h4 = h4 * 5 + 0x32ac3b17;
            }

            //----------
            // tail

            //const uint8_t* tail = (const uint8_t*)(data + nblocks * 16);
            ReadOnlySpan<byte> tail = data.Slice((nblocks * 16), len - (nblocks * 16));

            uint k1 = 0;
            uint k2 = 0;
            uint k3 = 0;
            uint k4 = 0;

            switch (len & 15)
            {
                case 15: k4 ^= (uint)tail[14] << 16; goto case 14;
                case 14: k4 ^= (uint)tail[13] << 8; goto case 13;
                case 13:
                    k4 ^= (uint)tail[12] << 0;
                    k4 *= c4; k4 = ROTL32(k4, 18); k4 *= c1; h4 ^= k4;
                    goto case 12;

                case 12: k3 ^= (uint)tail[11] << 24; goto case 11;
                case 11: k3 ^= (uint)tail[10] << 16; goto case 10;
                case 10: k3 ^= (uint)tail[9] << 8; goto case 9;
                case 9:
                    k3 ^= (uint)tail[8] << 0;
                    k3 *= c3; k3 = ROTL32(k3, 17); k3 *= c4; h3 ^= k3;
                    goto case 8;

                case 8: k2 ^= (uint)tail[7] << 24; goto case 7;
                case 7: k2 ^= (uint)tail[6] << 16; goto case 6;
                case 6: k2 ^= (uint)tail[5] << 8; goto case 5;
                case 5:
                    k2 ^= (uint)tail[4] << 0;
                    k2 *= c2; k2 = ROTL32(k2, 16); k2 *= c3; h2 ^= k2;
                    goto case 4;

                case 4: k1 ^= (uint)tail[3] << 24; goto case 3;
                case 3: k1 ^= (uint)tail[2] << 16; goto case 2;
                case 2: k1 ^= (uint)tail[1] << 8; goto case 1;
                case 1:
                    k1 ^= (uint)tail[0] << 0;
                    k1 *= c1; k1 = ROTL32(k1, 15); k1 *= c2; h1 ^= k1;
                    break;
            };

            //----------
            // finalization

            h1 ^= (uint)len; h2 ^= (uint)len; h3 ^= (uint)len; h4 ^= (uint)len;

            h1 += h2; h1 += h3; h1 += h4;
            h2 += h1; h3 += h1; h4 += h1;

            h1 = fmix32(h1);
            h2 = fmix32(h2);
            h3 = fmix32(h3);
            h4 = fmix32(h4);

            h1 += h2; h1 += h3; h1 += h4;
            h2 += h1; h3 += h1; h4 += h1;

            hash = new uint[4];
            hash[0] = h1;
            hash[1] = h2;
            hash[2] = h3;
            hash[3] = h4;
        }

        //-----------------------------------------------------------------------------

        public static void MurmurHash3_x64_128(ReadOnlySpan<byte> key, uint seed, out ulong[] hash)
        {
            var data = key;
            int len = data.Length;
            int nblocks = len / 16;

            ulong h1 = seed;
            ulong h2 = seed;

            ulong c1 = BIG_CONSTANT(0x87c37b91114253d5);
            ulong c2 = BIG_CONSTANT(0x4cf5ad432745937f);

            //----------
            // body

            //const ulong* blocks = (const ulong*)(data);
            ReadOnlySpan<byte> blocks = data;

            for (int i = 0; i < nblocks; i++)
            {
                ulong _k1 = getblock64(blocks, i * 2 + 0);
                ulong _k2 = getblock64(blocks, i * 2 + 1);

                _k1 *= c1; _k1 = ROTL64(_k1, 31); _k1 *= c2; h1 ^= _k1;

                h1 = ROTL64(h1, 27); h1 += h2; h1 = h1 * 5 + 0x52dce729;

                _k2 *= c2; _k2 = ROTL64(_k2, 33); _k2 *= c1; h2 ^= _k2;

                h2 = ROTL64(h2, 31); h2 += h1; h2 = h2 * 5 + 0x38495ab5;
            }

            //----------
            // tail

            //const uint8_t* tail = (const uint8_t*)(data + nblocks * 16);
            ReadOnlySpan<byte> tail = data.Slice((nblocks * 16), len - (nblocks * 16));

            ulong k1 = 0;
            ulong k2 = 0;

            switch (len & 15)
            {
                case 15: k2 ^= ((ulong)tail[14]) << 48; goto case 14;
                case 14: k2 ^= ((ulong)tail[13]) << 40; goto case 13;
                case 13: k2 ^= ((ulong)tail[12]) << 32; goto case 12;
                case 12: k2 ^= ((ulong)tail[11]) << 24; goto case 11;
                case 11: k2 ^= ((ulong)tail[10]) << 16; goto case 10;
                case 10: k2 ^= ((ulong)tail[9]) << 8; goto case 9;
                case 9:
                    k2 ^= ((ulong)tail[8]) << 0;
                    k2 *= c2; k2 = ROTL64(k2, 33); k2 *= c1; h2 ^= k2;
                    goto case 8;

                case 8: k1 ^= ((ulong)tail[7]) << 56; goto case 7;
                case 7: k1 ^= ((ulong)tail[6]) << 48; goto case 6;
                case 6: k1 ^= ((ulong)tail[5]) << 40; goto case 5;
                case 5: k1 ^= ((ulong)tail[4]) << 32; goto case 4;
                case 4: k1 ^= ((ulong)tail[3]) << 24; goto case 3;
                case 3: k1 ^= ((ulong)tail[2]) << 16; goto case 2;
                case 2: k1 ^= ((ulong)tail[1]) << 8; goto case 1;
                case 1:
                    k1 ^= ((ulong)tail[0]) << 0;
                    k1 *= c1; k1 = ROTL64(k1, 31); k1 *= c2; h1 ^= k1;
                    break;
            }

            //----------
            // finalization

            h1 ^= (uint)len; h2 ^= (uint)len;

            h1 += h2;
            h2 += h1;

            h1 = fmix64(h1);
            h2 = fmix64(h2);

            h1 += h2;
            h2 += h1;

            hash = new ulong[2];
            hash[0] = h1;
            hash[1] = h2;
        }
    }
}

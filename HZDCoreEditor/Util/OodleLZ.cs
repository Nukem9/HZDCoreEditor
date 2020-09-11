using System;
using System.Runtime.InteropServices;

namespace Utility
{
    public static class OodleLZ
    {
        private enum OodleLZ_Compressor : int
        {
            LZH = 0,
            LZHLW = 1,
            LZNIB = 2,
            None = 3,
            LZB16 = 4,
            LZBLW = 5,
            LZA = 6,
            LZNA = 7,
            Kraken = 8,
            Mermaid = 9,
            BitKnit = 10,
            Selkie = 11,
            Akkorokamui = 12,
        }

        private enum OodleLZ_Compression : int
        {
            None = 0,
            SuperFast = 1,
            VertFast = 2,
            Fast = 3,
            Normal = 4,
            Optimal1 = 5,
            Optimal2 = 6,
            Optimal3 = 7,
            Optimal4 = 8,
            Optimal5 = 9,
        }

        private enum OodleLZ_FuzzSafe : int
        {
            No = 0,
            Yes = 1,
        }

        private enum OodleLZ_CheckCRC : int
        {
            No = 0,
            Yes = 1,
        }

        private enum OodleLZ_Verbosity : int
        {
            None = 0,
        }

        private enum OodleLZ_Decode : int
        {
            ThreadPhase1 = 1,
            ThreadPhase2 = 2,
            Unthreaded = 3,
        }

        [DllImport("oo2core_3_win64.dll")]
        private static unsafe extern long OodleLZ_Decompress(
            byte* compBuf,
            UIntPtr compBufSize,
            byte* decodeTo,
            UIntPtr decodeToSize,
            OodleLZ_FuzzSafe fuzzSafetyFlag,
            OodleLZ_CheckCRC crcCheckFlag,
            OodleLZ_Verbosity logVerbosityFlag,
            UIntPtr a8,
            UIntPtr a9,
            UIntPtr chunkDecodeCallback,
            UIntPtr chunkDecodeContext,
            byte* scratchBuf,
            UIntPtr scratchBufSize,
            OodleLZ_Decode threadPhase);

        [DllImport("oo2core_3_win64.dll")]
        private static unsafe extern long OodleLZ_Compress(
            OodleLZ_Compressor compressor,
            byte* dataBuf,
            UIntPtr dataBufSize,
            byte* encodeTo,
            OodleLZ_Compression compressLevel,
            UIntPtr compressOptions,
            byte* scratchBuf,
            UIntPtr scratchBufSize);

        public static bool Decompress(ReadOnlySpan<byte> inputBuffer, Span<byte> outputBuffer)
        {
            long result = -1;

            unsafe
            {
                fixed (byte* input = inputBuffer)
                fixed (byte* output = outputBuffer)
                {
                    result = OodleLZ_Decompress(
                        input,
                        (UIntPtr)inputBuffer.Length,
                        output,
                        (UIntPtr)outputBuffer.Length,
                        OodleLZ_FuzzSafe.Yes,
                        OodleLZ_CheckCRC.No,
                        OodleLZ_Verbosity.None,
                        UIntPtr.Zero,
                        UIntPtr.Zero,
                        UIntPtr.Zero,
                        UIntPtr.Zero,
                        null,
                        UIntPtr.Zero,
                        OodleLZ_Decode.Unthreaded);
                }
            }

            return result == 0;
        }

        public static bool Decompress(byte[] inputBuffer, byte[] outputBuffer)
        {
            return Decompress(inputBuffer.AsSpan(), outputBuffer.AsSpan());
        }

        public static long Compress(ReadOnlySpan<byte> inputBuffer, Span<byte> outputBuffer)
        {
            long result = -1;

            unsafe
            {
                fixed (byte* input = inputBuffer)
                fixed (byte* output = outputBuffer)
                {
                    result = OodleLZ_Compress(
                        OodleLZ_Compressor.Kraken,
                        input,
                        (UIntPtr)inputBuffer.Length,
                        output,
                        OodleLZ_Compression.Optimal1,
                        UIntPtr.Zero,
                        null,
                        UIntPtr.Zero);
                }
            }

            return result;
        }

        public static long Compress(byte[] inputBuffer, byte[] outputBuffer)
        {
            return Compress(inputBuffer.AsSpan(), outputBuffer.AsSpan());
        }
    }
}

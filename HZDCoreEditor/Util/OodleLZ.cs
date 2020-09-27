using System;
using System.Runtime.InteropServices;

namespace Utility
{
    public static class OodleLZ
    {
        private enum OodleLZ_Compressor_V3 : int
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

        private enum OodleLZ_Compressor_V7 : int
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
            Hydra = 12,
            Leviathan = 13,
        }

        private enum OodleLZ_Compression_V3 : int
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

        private enum OodleLZ_Compression_V7 : int
        {
            HyperFast4 = 0,
            HyperFast3 = 1,
            HyperFast2 = 2,
            HyperFast1 = 3,
            None = 4,
            SuperFast = 5,
            VertFast = 6,
            Fast = 7,
            Normal = 8,
            Optimal1 = 9,
            Optimal2 = 10,
            Optimal3 = 11,
            Optimal4 = 12,
            Optimal5 = 13,
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

        private unsafe delegate long OodleLZ_Decompress_Delegate(
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
            byte* decoderMemory,
            UIntPtr decoderMemorySize,
            OodleLZ_Decode threadPhase);

        private unsafe delegate long OodleLZ_Compress_Delegate(
            int compressor, // OodleLZ_Compressor_V#
            byte* dataBuf,
            UIntPtr dataBufSize,
            byte* encodeTo,
            int compressLevel, // OodleLZ_Compression_V#
            UIntPtr compressOptions,
            byte* encoderMemory,
            UIntPtr encoderMemorySize);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr LoadLibraryW(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private static readonly int OodleLZ_Version;
        private static readonly OodleLZ_Decompress_Delegate OodleLZ_Decompress;
        private static readonly OodleLZ_Compress_Delegate OodleLZ_Compress;

        static OodleLZ()
        {
            // Can't use DllImport if I want to support multiple DLL versions. Resolve the exports by hand.
            IntPtr oodleLibraryHandle = LoadLibraryW("oo2core_7_win64.dll");
            OodleLZ_Version = 7;

            if (oodleLibraryHandle == IntPtr.Zero)
            {
                oodleLibraryHandle = LoadLibraryW("oo2core_3_win64.dll");
                OodleLZ_Version = 3;
            }

            IntPtr decompressorFunc = GetProcAddress(oodleLibraryHandle, "OodleLZ_Decompress");
            IntPtr compressorFunc = GetProcAddress(oodleLibraryHandle, "OodleLZ_Compress");

            if (decompressorFunc == IntPtr.Zero || compressorFunc == IntPtr.Zero)
                throw new Exception("A valid oo2core DLL couldn't be found in the program directory (oo2core_3_win64.dll, oo2core_7_win64.dll)");

            OodleLZ_Decompress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Decompress_Delegate>(decompressorFunc);
            OodleLZ_Compress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Compress_Delegate>(compressorFunc);
        }

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

            int compressor = OodleLZ_Version switch
            {
                3 => (int)OodleLZ_Compressor_V3.Kraken,
                7 => (int)OodleLZ_Compressor_V7.Kraken,
                _ => throw new NotImplementedException(),
            };

            int compression = OodleLZ_Version switch
            {
                3 => (int)OodleLZ_Compression_V3.Optimal1,
                7 => (int)OodleLZ_Compression_V7.Optimal1,
                _ => throw new NotImplementedException(),
            };

            unsafe
            {
                fixed (byte* input = inputBuffer)
                fixed (byte* output = outputBuffer)
                {
                    result = OodleLZ_Compress(
                        compressor,
                        input,
                        (UIntPtr)inputBuffer.Length,
                        output,
                        compression,
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

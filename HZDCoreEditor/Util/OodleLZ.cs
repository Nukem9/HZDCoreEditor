using System;
using System.Runtime.InteropServices;

namespace HZDCoreEditor.Util
{
    internal static class OodleLZ
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
            UIntPtr encoderMemorySize,
            UIntPtr unknown1,
            UIntPtr unknown2);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr LoadLibraryW(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private static IntPtr _oodleLZ_Handle;
        private static int _oodleLZ_Version;
        private static OodleLZ_Decompress_Delegate _oodleLZ_Decompress;
        private static OodleLZ_Compress_Delegate _oodleLZ_Compress;

        private static void EnsureLoaded()
        {
            if (_oodleLZ_Handle != IntPtr.Zero)
                return;

            // Can't use DllImport if I want to support multiple DLL versions. Resolve the exports by hand.
            _oodleLZ_Handle = LoadLibraryW("oo2core_7_win64.dll");
            _oodleLZ_Version = 7;

            if (_oodleLZ_Handle == IntPtr.Zero)
            {
                _oodleLZ_Handle = LoadLibraryW("oo2core_3_win64.dll");
                _oodleLZ_Version = 3;
            }

            var decompressorFunc = GetProcAddress(_oodleLZ_Handle, "OodleLZ_Decompress");
            var compressorFunc = GetProcAddress(_oodleLZ_Handle, "OodleLZ_Compress");

            if (decompressorFunc == IntPtr.Zero || compressorFunc == IntPtr.Zero)
                throw new Exception("A valid oo2core DLL couldn't be found in the program directory (oo2core_3_win64.dll, oo2core_7_win64.dll)");

            _oodleLZ_Decompress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Decompress_Delegate>(decompressorFunc);
            _oodleLZ_Compress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Compress_Delegate>(compressorFunc);
        }

        public static void Unload()
        {
            if (_oodleLZ_Handle == IntPtr.Zero)
                return;
            if (FreeLibrary(_oodleLZ_Handle))
                _oodleLZ_Handle = IntPtr.Zero;
        }

        public static bool Decompress(byte[] inputBuffer, byte[] outputBuffer, uint decompressLength)
            => Decompress(inputBuffer.AsSpan(), outputBuffer.AsSpan(), decompressLength);

        public static bool Decompress(ReadOnlySpan<byte> inputBuffer, Span<byte> outputBuffer, uint decompressLength)
        {
            EnsureLoaded();

            long result = -1;

            unsafe
            {
                fixed (byte* input = inputBuffer)
                fixed (byte* output = outputBuffer)
                {
                    result = _oodleLZ_Decompress(
                        input,
                        (UIntPtr)inputBuffer.Length,
                        output,
                        (UIntPtr)decompressLength,
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

        public static long Compress(Span<byte> inputBuffer, Span<byte> outputBuffer)
            => Compress(inputBuffer, inputBuffer.Length, outputBuffer);

        public static long Compress(byte[] inputBuffer, byte[] outputBuffer)
            => Compress(inputBuffer.AsSpan(), outputBuffer.AsSpan());

        public static long Compress(Span<byte> inputBuffer, int inputBufferLength, Span<byte> outputBuffer)
        {
            EnsureLoaded();

            long result = -1;

            int compressor = _oodleLZ_Version switch
            {
                3 => (int)OodleLZ_Compressor_V3.Kraken,
                7 => (int)OodleLZ_Compressor_V7.Kraken,
                _ => throw new NotImplementedException(),
            };

            int compression = _oodleLZ_Version switch
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
                    result = _oodleLZ_Compress(
                        compressor,
                        input,
                        (UIntPtr)inputBufferLength,
                        output,
                        compression,
                        UIntPtr.Zero,
                        null,
                        UIntPtr.Zero,
                        UIntPtr.Zero,
                        UIntPtr.Zero);
                }
            }

            return result;
        }
    }
}

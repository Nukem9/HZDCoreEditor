using System;
using System.Runtime.InteropServices;

namespace HZDCoreEditor
{
    public static class OodleLZ
    {
        private enum OodleLZ_Decode : int
        {
            ThreadPhase1 = 1,
            ThreadPhase2 = 2,
            ThreadPhaseFinal = 3,
        }

        // TODO: Use a byte pointer instead of an array to allow Span<byte>. Might require unsafe attributes.
        [DllImport("oo2core_3_win64.dll")]
        private static extern int OodleLZ_Decompress(
            byte[] compBuf,
            UIntPtr compBufSize,
            byte[] decodeTo,
            UIntPtr decodeToSize,
            int fuzzSafetyFlag,
            int a6,
            int logLevel,
            UIntPtr a8,
            UIntPtr a9,
            UIntPtr chunkDecodeCallback,
            UIntPtr chunkDecodeUserData,
            byte[] threadPhaseBuf,
            UIntPtr threadPhaseBufSize,
            OodleLZ_Decode threadPhase);

        public static bool Decompress(byte[] inputBuffer, byte[] outputBuffer)
        {
            int result = OodleLZ_Decompress(
                inputBuffer,
                (UIntPtr)inputBuffer.Length,
                outputBuffer,
                (UIntPtr)outputBuffer.Length,
                1,
                0,
                0,
                UIntPtr.Zero,
                UIntPtr.Zero,
                UIntPtr.Zero,
                UIntPtr.Zero,
                null,
                UIntPtr.Zero,
                OodleLZ_Decode.ThreadPhaseFinal);

            return result == 0;
        }
    }
}

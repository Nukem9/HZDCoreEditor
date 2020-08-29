using BinaryStreamExtensions;
using System;
using System.IO;

namespace Decima
{
    static partial class GameData
    {
        public class HwBuffer
        {
            public byte[] Data;

            public static HwBuffer FromData(BinaryReader reader, EDataBufferFormat format, bool streaming, uint byteStride, uint elementCount)
            {
                if (streaming)
                {
                    ReadGenericThing(reader);
                    return null;
                }
                else
                {
                    return new HwBuffer { Data = reader.ReadBytes(elementCount * byteStride) };
                }
            }

            public static HwBuffer FromVertexData(BinaryReader reader, bool streaming, uint ByteStride, uint ElementCount)
            {
                return FromData(reader, EDataBufferFormat.Structured, streaming, ByteStride, ElementCount);
            }

            public static HwBuffer FromIndexData(BinaryReader reader, EIndexFormat format, bool streaming, uint elementCount)
            {
                var dataFormat = format switch
                {
                    EIndexFormat.Index16 => EDataBufferFormat.R_UINT_16,
                    EIndexFormat.Index32 => EDataBufferFormat.R_UINT_32,
                    _ => throw new NotSupportedException("Unknown index buffer type"),
                };

                return FromData(reader, dataFormat, streaming, GetStrideForFormat(dataFormat), elementCount);
            }

            public static uint GetStrideForFormat(EDataBufferFormat format)
            {
                switch (format)
                {
                    case EDataBufferFormat.R_UINT_8:
                    case EDataBufferFormat.R_UNORM_8:
                        return 1;

                    case EDataBufferFormat.R_FLOAT_16:
                    case EDataBufferFormat.R_UINT_16:
                    case EDataBufferFormat.R_UNORM_16:
                        return 2;

                    case EDataBufferFormat.R_FLOAT_32:
                    case EDataBufferFormat.R_UINT_32:
                    case EDataBufferFormat.R_INT_32:
                    case EDataBufferFormat.RGBA_UNORM_8:
                    case EDataBufferFormat.RGBA_UINT_8:
                    case EDataBufferFormat.RG_UINT_16:
                    case EDataBufferFormat.RGBA_INT_8:
                        return 4;

                    case EDataBufferFormat.RG_FLOAT_32:
                    case EDataBufferFormat.RG_UINT_32:
                    case EDataBufferFormat.RG_INT_32:
                    case EDataBufferFormat.RGBA_UINT_16:
                        return 8;

                    case EDataBufferFormat.RGB_FLOAT_32:
                    case EDataBufferFormat.RGB_UINT_32:
                    case EDataBufferFormat.RGB_INT_32:
                        return 12;

                    case EDataBufferFormat.RGBA_FLOAT_32:
                    case EDataBufferFormat.RGBA_UINT_32:
                    case EDataBufferFormat.RGBA_INT_32:
                        return 16;
                }

                throw new NotSupportedException("Attempting to get the size of a dynamic or invalid stride format");
            }
        }
    }
}

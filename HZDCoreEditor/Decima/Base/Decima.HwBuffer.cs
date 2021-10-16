using HZDCoreEditor.Util;
using System;
using System.IO;

namespace Decima
{
    public class HwBuffer
    {
        public BaseDataBufferFormat Format;
        public uint ElementStride;
        public uint ElementCount;
        public bool Streaming { get; private set; }
        public BaseStreamHandle StreamInfo;
        public byte[] Data;

        public void ToData(BinaryWriter writer)
        {
            if (Streaming)
                StreamInfo?.ToData(writer);
            else
                writer.Write(Data);
        }

        public static HwBuffer FromData(BinaryReader reader, GameType gameType, BaseDataBufferFormat format, bool streaming, uint byteStride, uint elementCount)
        {
            var buffer = new HwBuffer
            {
                Format = format,
                ElementStride = byteStride,
                ElementCount = elementCount,
                Streaming = streaming,
            };

            if (buffer.Streaming)
            {
                if (gameType == GameType.HZD)
                    buffer.StreamInfo = BaseStreamHandle.FromData(reader, gameType);
                else
                    buffer.StreamInfo = null;
            }
            else
            {
                // Read raw data
                buffer.Data = reader.ReadBytesStrict(elementCount * byteStride);
            }

            return buffer;
        }

        public static HwBuffer FromVertexData(BinaryReader reader, GameType gameType, bool streaming, uint ByteStride, uint ElementCount)
        {
            return FromData(reader, gameType, BaseDataBufferFormat.Structured, streaming, ByteStride, ElementCount);
        }

        public static HwBuffer FromIndexData(BinaryReader reader, GameType gameType, BaseIndexFormat format, bool streaming, uint elementCount)
        {
            var dataFormat = format switch
            {
                BaseIndexFormat.Index16 => BaseDataBufferFormat.R_UINT_16,
                BaseIndexFormat.Index32 => BaseDataBufferFormat.R_UINT_32,
                _ => throw new NotSupportedException("Unknown index buffer type"),
            };

            return FromData(reader, gameType, dataFormat, streaming, GetStrideForFormat(dataFormat), elementCount);
        }

        public static uint GetStrideForFormat(BaseDataBufferFormat format)
        {
            switch (format)
            {
                case BaseDataBufferFormat.R_UINT_8:
                case BaseDataBufferFormat.R_UNORM_8:
                    return 1;

                case BaseDataBufferFormat.R_FLOAT_16:
                case BaseDataBufferFormat.R_UINT_16:
                case BaseDataBufferFormat.R_UNORM_16:
                    return 2;

                case BaseDataBufferFormat.R_FLOAT_32:
                case BaseDataBufferFormat.R_UINT_32:
                case BaseDataBufferFormat.R_INT_32:
                case BaseDataBufferFormat.RGBA_UNORM_8:
                case BaseDataBufferFormat.RGBA_UINT_8:
                case BaseDataBufferFormat.RG_UINT_16:
                case BaseDataBufferFormat.RGBA_INT_8:
                    return 4;

                case BaseDataBufferFormat.RG_FLOAT_32:
                case BaseDataBufferFormat.RG_UINT_32:
                case BaseDataBufferFormat.RG_INT_32:
                case BaseDataBufferFormat.RGBA_UINT_16:
                    return 8;

                case BaseDataBufferFormat.RGB_FLOAT_32:
                case BaseDataBufferFormat.RGB_UINT_32:
                case BaseDataBufferFormat.RGB_INT_32:
                    return 12;

                case BaseDataBufferFormat.RGBA_FLOAT_32:
                case BaseDataBufferFormat.RGBA_UINT_32:
                case BaseDataBufferFormat.RGBA_INT_32:
                    return 16;
            }

            throw new NotSupportedException("Attempting to get the size of a dynamic or invalid stride format");
        }
    }
}

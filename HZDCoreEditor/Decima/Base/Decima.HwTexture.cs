using HZDCoreEditor.Util;
using System;
using System.IO;

namespace Decima
{
    public class HwTexture
    {
        public TextureHeader Header;
        public byte[] HwTextureData;

        public class TextureHeader
        {
            public BaseTextureType Type;
            public ushort Width;
            public ushort Height;
            public uint TexArraySliceCount;
            public uint Tex3DDepth;
            public byte MipCount;
            public BasePixelFormat PixelFormat;
            public byte Unknown1;
            public byte Unknown2;
            public byte Unknown3;
            public byte Flags;
            public byte Unknown4;
            public byte Unknown5;
            public BaseGGUUID ResourceGUID;
        }

        public void ToData(BinaryWriter writer)
        {
            writer.Write((byte)Header.Type);
            writer.Write((byte)0);
            writer.Write(Header.Width);
            writer.Write(Header.Height);

            switch (Header.Type)
            {
                case BaseTextureType._2D:
                case BaseTextureType.CubeMap:
                    writer.Write((ushort)0);
                    break;

                case BaseTextureType._3D:
                    writer.Write((byte)BitOperations.Log2(Header.Tex3DDepth));
                    writer.Write((byte)0);
                    break;

                case BaseTextureType._2DArray:
                    writer.Write((ushort)Header.TexArraySliceCount);
                    break;

                default:
                    throw new NotImplementedException("Unknown texture type");
            }

            writer.Write(Header.MipCount);
            writer.Write((byte)Header.PixelFormat);
            writer.Write(Header.Unknown1);
            writer.Write(Header.Unknown2);
            writer.Write(Header.Unknown3);
            writer.Write(Header.Flags);
            writer.Write(Header.Unknown4);
            writer.Write(Header.Unknown5);
            Header.ResourceGUID.ToData(writer);

            writer.Write((uint)HwTextureData.Length);
            writer.Write(HwTextureData);
        }

        public static HwTexture FromData(BinaryReader reader)
        {
            var header = new TextureHeader();

            // 32 byte texture header
            header.Type = (BaseTextureType)reader.ReadByte();   // 0
            _ = reader.ReadByte();                              // 1
            header.Width = reader.ReadUInt16();                 // 2
            header.Height = reader.ReadUInt16();                // 4

            header.TexArraySliceCount = 1;
            header.Tex3DDepth = 1;

            switch (header.Type)
            {
                case BaseTextureType._2D:
                case BaseTextureType.CubeMap:
                    _ = reader.ReadUInt16();
                    break;

                case BaseTextureType._3D:
                    header.Tex3DDepth = 1u << reader.ReadByte();
                    _ = reader.ReadByte();
                    break;

                case BaseTextureType._2DArray:
                    header.TexArraySliceCount = reader.ReadUInt16();
                    break;

                default:
                    throw new NotImplementedException("Unknown texture type");
            }

            header.MipCount = reader.ReadByte();                    // 8
            header.PixelFormat = (BasePixelFormat)reader.ReadByte();// 9
            header.Unknown1 = reader.ReadByte();                    // 10
            header.Unknown2 = reader.ReadByte();                    // 11
            header.Unknown3 = reader.ReadByte();                    // 12
            header.Flags = reader.ReadByte();                       // 13
            header.Unknown4 = reader.ReadByte();                    // 14 Something to do with mips. Autogen?
            header.Unknown5 = reader.ReadByte();                    // 15
            header.ResourceGUID = new BaseGGUUID().FromData(reader);// 16

            var x = new HwTexture();
            x.Header = header;
            uint hwTextureSize = reader.ReadUInt32();
            x.HwTextureData = reader.ReadBytesStrict(hwTextureSize);

            return x;
        }
    }
}
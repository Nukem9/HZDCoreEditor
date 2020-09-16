using Utility;
using System;
using System.IO;
using System.Numerics;

namespace Decima.HZD
{
    [RTTI.Serializable(0xF2E1AFB7052B3866)]
    public class Texture : Resource, RTTI.IExtraBinaryDataCallback
    {
        public TextureHeader Header;
        public byte[] HwTextureData;

        public class TextureHeader
        {
            public ETextureType Type;
            public ushort Width;
            public ushort Height;
            public uint TexArraySliceCount;
            public uint Tex3DDepth;
            public byte MipCount;
            public EPixelFormat PixelFormat;
            public byte Unknown1;
            public byte Unknown2;
            public byte Unknown3;
            public byte Flags;
            public byte Unknown4;
            public byte Unknown5;
            public GGUUID ResourceGUID;
        }

        public void DeserializeExtraData(BinaryReader reader)
        {
            Header = new TextureHeader();

            // 32 byte texture header
            Header.Type = (ETextureType)reader.ReadByte();  // 0
            _ = reader.ReadByte();                          // 1
            Header.Width = reader.ReadUInt16();             // 2
            Header.Height = reader.ReadUInt16();            // 4

            Header.TexArraySliceCount = 1;
            Header.Tex3DDepth = 1;

            switch (Header.Type)
            {
                case ETextureType._2D:
                case ETextureType.CubeMap:
                    _ = reader.ReadUInt16();
                    break;

                case ETextureType._3D:
                    Header.Tex3DDepth = 1u << reader.ReadByte();
                    _ = reader.ReadByte();
                    break;

                case ETextureType._2DArray:
                    Header.TexArraySliceCount = reader.ReadUInt16();
                    break;

                default:
                    throw new NotImplementedException("Unknown texture type");
            }

            Header.MipCount = reader.ReadByte();                    // 8
            Header.PixelFormat = (EPixelFormat)reader.ReadByte();   // 9
            Header.Unknown1 = reader.ReadByte();                    // 10
            Header.Unknown2 = reader.ReadByte();                    // 11
            Header.Unknown3 = reader.ReadByte();                    // 12
            Header.Flags = reader.ReadByte();                       // 13
            Header.Unknown4 = reader.ReadByte();                    // 14 Something to do with mips. Autogen?
            Header.Unknown5 = reader.ReadByte();                    // 15
            Header.ResourceGUID = GGUUID.FromData(reader);          // 16

            uint hwTextureSize = reader.ReadUInt32();
            HwTextureData = reader.ReadBytesStrict(hwTextureSize);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((byte)Header.Type);
            writer.Write((byte)0);
            writer.Write(Header.Width);
            writer.Write(Header.Height);

            switch (Header.Type)
            {
                case ETextureType._2D:
                case ETextureType.CubeMap:
                    writer.Write((ushort)0);
                    break;

                case ETextureType._3D:
                    writer.Write((byte)BitOperations.Log2(Header.Tex3DDepth));
                    writer.Write((byte)0);
                    break;

                case ETextureType._2DArray:
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
    }
}

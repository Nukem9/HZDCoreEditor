using HZDCoreEditor.Util;
using System;
using System.IO;

namespace Decima
{
    public class HwTexture
    {
        public TextureHeader Header;
        public BaseStreamHandle StreamHandle;
        public uint StreamedMipCount;
        public uint EmbeddedDataSize;
        public byte[] EmbeddedTextureData;

        public class TextureHeader
        {
            public BaseTextureType Type;
            public ushort Width;
            public ushort Height;
            public uint ArraySliceCount;
            public uint Depth3D;
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
                    writer.Write((byte)BitOperations.Log2(Header.Depth3D));
                    writer.Write((byte)0);
                    break;

                case BaseTextureType._2DArray:
                    writer.Write((ushort)Header.ArraySliceCount);
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

            // Raw pixel data handling
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            bw.Write(EmbeddedDataSize);
            bw.Write(StreamHandle?.ResourceSize() ?? 0);
            bw.Write(StreamedMipCount);

            StreamHandle?.ToData(bw);

            if (EmbeddedTextureData?.Length > 0)
                bw.Write(EmbeddedTextureData);

            writer.Write((uint)ms.Length);
            ms.WriteTo(writer.BaseStream);
        }

        public static HwTexture FromData(BinaryReader reader, GameType gameType)
        {
            var texture = new HwTexture();
            var header = new TextureHeader();

            // 32 byte texture header
            header.Type = (BaseTextureType)reader.ReadByte();   // 0
            _ = reader.ReadByte();                              // 1
            header.Width = reader.ReadUInt16();                 // 2
            header.Height = reader.ReadUInt16();                // 4

            header.ArraySliceCount = 0;
            header.Depth3D = 0;

            switch (header.Type)
            {
                case BaseTextureType._2D:
                case BaseTextureType.CubeMap:
                    _ = reader.ReadUInt16();
                    break;

                case BaseTextureType._3D:
                    header.Depth3D = 1u << reader.ReadByte();
                    _ = reader.ReadByte();
                    break;

                case BaseTextureType._2DArray:
                    header.ArraySliceCount = reader.ReadUInt16();
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
            header.ResourceGUID = BaseGGUUID.FromData(reader);      // 16
            texture.Header = header;

            // Raw pixel data handling
            uint containerSize = reader.ReadUInt32();               // Size of the remaining data being read
            long containerEndPosition = reader.BaseStream.Position + containerSize;

            texture.EmbeddedDataSize = reader.ReadUInt32();         // Size of pixel data in this core object entry. This value is never used. if (a - b > 0) only.
            uint streamedDataSize = reader.ReadUInt32();            // Size of pixel data in external file. This value is never used. if (a > 0) only.
            texture.StreamedMipCount = reader.ReadUInt32();         // Number of mipmaps in external file

            if (streamedDataSize > 0)
                texture.StreamHandle = BaseStreamHandle.FromData(reader, gameType);

            // TODO: Something is wrong here. EmbeddedDataSize doesn't always match the actual length of the byte array. Why? Does it depend on header flags?
            if (texture.EmbeddedDataSize > 0)
                texture.EmbeddedTextureData = reader.ReadBytesStrict((uint)(containerEndPosition - reader.BaseStream.Position));

            return texture;
        }
    }
}
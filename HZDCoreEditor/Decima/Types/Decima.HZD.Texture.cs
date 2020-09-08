using BinaryStreamExtensions;
using System;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xF2E1AFB7052B3866)]
    public class Texture : Resource, RTTI.IExtraBinaryDataCallback
    {
        public GGUUID ResourceGUID;

        public void DeserializeExtraData(BinaryReader reader)
        {
            // 32 byte texture header
            var textureType = (ETextureType)reader.ReadByte();  // 0
            _ = reader.ReadByte();                              // 1
            ushort width = reader.ReadUInt16();                 // 2
            ushort height = reader.ReadUInt16();                // 4

            uint texArraySliceCount = 1;
            uint tex3DDepth = 1;

            switch (textureType)
            {
                case ETextureType._2D:
                case ETextureType.CubeMap:
                    _ = reader.ReadUInt16();
                    break;

                case ETextureType._3D:
                    tex3DDepth = 1u << reader.ReadByte();
                    _ = reader.ReadByte();
                    break;

                case ETextureType._2DArray:
                    texArraySliceCount = reader.ReadUInt16();
                    break;

                default:
                    throw new NotImplementedException("Unknown texture type");
            }

            byte mipCount = reader.ReadByte();                      // 8
            var pixelFormat = (EPixelFormat)reader.ReadByte();      // 9
            byte unknown2 = reader.ReadByte();                      // 10
            byte unknown3 = reader.ReadByte();                      // 11
            byte unknown4 = reader.ReadByte();                      // 12
            byte flags = reader.ReadByte();                         // 13
            byte unknown6 = reader.ReadByte();                      // 14 Something to do with mips
            byte unknown7 = reader.ReadByte();                      // 15
            ResourceGUID = GGUUID.FromData(reader);                 // 16

            uint hwTextureSize = reader.ReadUInt32();
            var hwTextureData = reader.ReadBytesStrict(hwTextureSize);
        }
    }
}

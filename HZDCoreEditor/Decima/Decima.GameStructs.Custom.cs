using BinaryStreamExtensions;
using System;
using System.IO;
using System.Text;

namespace Decima
{
    static partial class GameData
    {
        public static void ReadGenericThing(BinaryReader reader)
        {
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
            {
                // Stream resource file path
                var str = Encoding.UTF8.GetString(reader.ReadBytes(stringLength));
            }

            // Likely file offsets or length
            var unknown1 = reader.ReadUInt64();
            var unknown2 = reader.ReadUInt64();
        }

        public partial class IndexArrayResource
        {
            public uint Flags;
            public HwBuffer Buffer;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint indexElementCount = reader.ReadUInt32();

                if (indexElementCount > 0)
                {
                    Flags = reader.ReadUInt32();
                    var format = (EIndexFormat)reader.ReadUInt32();
                    uint isStreaming = reader.ReadUInt32();

                    if (isStreaming != 0 && isStreaming != 1)
                        throw new Exception("???");

                    // Likely GUID or at least an identifier
                    var resourceGUID = reader.ReadBytes(16);

                    Buffer = HwBuffer.FromIndexData(reader, format, isStreaming != 0, indexElementCount);
                }
            }
        }

        public partial class LocalizedSimpleSoundResource
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                // ANNOYING
            }
        }

        public partial class LocalizedTextResource
        {
            private const uint LanguageCount = (uint)ELanguage.Chinese_Simplified - 1;

            public byte[][] TextData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                // Keep this as an array of bytes for the time being since I don't know the encoding type
                TextData = new byte[LanguageCount][];

                for (uint i = 0; i < LanguageCount; i++)
                {
                    ushort stringLength = reader.ReadUInt16();

                    if (stringLength > 0)
                        TextData[i] = reader.ReadBytes(stringLength);
                }
            }

            public string GetStringForLanguage(ELanguage language)
            {
                if (language == ELanguage.Unknown)
                    throw new ArgumentException("Invalid language", nameof(language));

                return Encoding.UTF8.GetString(TextData[(int)language - 1]);
            }
        }

        public partial class Pose
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                bool hasExtraData = reader.ReadBooleanWithCheck();

                if (hasExtraData)
                {
                    uint count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData1 = reader.ReadBytes(count * 48);
                        var unknownData2 = reader.ReadBytes(count * 64);
                    }

                    count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData3 = reader.ReadBytes(count * 4);
                    }
                }
            }
        }

        public partial class PhysicsShapeResource
        {
            public byte[] HavokData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint havokDataLength = reader.ReadUInt32();

                if (havokDataLength > 0)
                {
                    // Havok 2014 HKX file ("hk_2014.2.0-r1")
                    HavokData = reader.ReadBytes(havokDataLength);
                }
            }
        }

        public partial class ShaderResource
        {
            public byte[] ShaderData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint shaderDataLength = reader.ReadUInt32();

                // Likely a GUID read as 4 uints
                uint unknown1 = reader.ReadUInt32();
                uint unknown2 = reader.ReadUInt32();
                uint unknown3 = reader.ReadUInt32();
                uint unknown4 = reader.ReadUInt32();

                // Contains the actual DXBC/DXIL shader
                ShaderData = reader.ReadBytes(shaderDataLength);
            }
        }

        public partial class Texture
        {
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
                var resourceGUID = reader.ReadBytes(16);                // 16

                uint hwTextureSize = reader.ReadUInt32();
                var hwTextureData = reader.ReadBytes(hwTextureSize);
            }
        }

        public partial class UITexture
        {
            Texture HiResTexture;   // Screen res >  1920x1080 (Default if low res not present)
            Texture LowResTexture;  // Screen res <= 1920x1080

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint hiResDataSize = reader.ReadUInt32();
                uint lowResDataSize = reader.ReadUInt32();

                HiResTexture = new Texture();
                HiResTexture.DeserializeExtraData(reader);

                if (lowResDataSize > 0)
                {
                    LowResTexture = new Texture();
                    LowResTexture.DeserializeExtraData(reader);
                }
            }
        }

        public partial class VertexArrayResource
        {
            public HwBuffer[] Buffers;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint vertexElementCount = reader.ReadUInt32();
                uint vertexStreamCount = reader.ReadUInt32();
                bool isStreaming = reader.ReadBooleanWithCheck();

                Buffers = new HwBuffer[vertexStreamCount];

                for (uint i = 0; i < vertexStreamCount; i++)
                {
                    uint unknown4 = reader.ReadUInt32();
                    uint vertexByteStride = reader.ReadUInt32();
                    uint unknownCounter = reader.ReadUInt32();

                    for (uint j = 0; j < unknownCounter; j++)
                    {
                        // 4 bytes read separately
                        var packedData = reader.ReadBytes(4);
                    }

                    // Likely GUID or at least an identifier
                    var resourceGUID = reader.ReadBytes(16);

                    Buffers[i] = HwBuffer.FromVertexData(reader, isStreaming, vertexByteStride, vertexElementCount);
                }
            }
        }

        public partial class WaveResource
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                if (IsStreaming)
                    ReadGenericThing(reader);
            }
        }
    }
}

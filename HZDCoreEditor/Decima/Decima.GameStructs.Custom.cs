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
                var str = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));
            }

            // Likely file offsets or length
            var unknown1 = reader.ReadUInt64();
            var unknown2 = reader.ReadUInt64();
        }

        public partial class DataBufferResource
        {
            HwBuffer Buffer;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint bufferElementCount = reader.ReadUInt32();

                if (bufferElementCount > 0)
                {
                    uint isStreaming = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    var format = (EDataBufferFormat)reader.ReadUInt32();
                    uint bufferStride = reader.ReadUInt32();

                    if (isStreaming != 0 && isStreaming != 1)
                        throw new Exception("???");

                    if (format != EDataBufferFormat.Structured)
                        bufferStride = HwBuffer.GetStrideForFormat(format);

                    Buffer = HwBuffer.FromData(reader, format, isStreaming != 0, bufferStride, bufferElementCount);
                }
            }
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
                    var resourceGUID = reader.ReadBytesStrict(16);

                    Buffer = HwBuffer.FromIndexData(reader, format, isStreaming != 0, indexElementCount);
                }
            }
        }

        public partial class LocalizedSimpleSoundResource
        {
            string SoundDataFilePath;
            WaveResource SoundFormat;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint stringLength = reader.ReadUInt32();

                if (stringLength > 0)
                    SoundDataFilePath = Encoding.UTF8.GetString(reader.ReadBytes(stringLength));

                ushort languageBits = reader.ReadUInt16();
                byte dataLength = reader.ReadByte();

                SoundFormat = new WaveResource();
                SoundFormat.DecodeFlags(reader.ReadByte());
                SoundFormat.FrameSize = reader.ReadUInt16();
                SoundFormat.Encoding = (EWaveDataEncoding)reader.ReadByte();
                SoundFormat.ChannelCount = reader.ReadByte();
                SoundFormat.SampleRate = reader.ReadInt32();
                SoundFormat.BitsPerSample = reader.ReadUInt16();
                SoundFormat.BitsPerSecond = reader.ReadUInt32();
                SoundFormat.BlockAlignment = reader.ReadUInt16();
                SoundFormat.FormatTag = reader.ReadUInt16();

                uint currentLanguageBit = 1;
                for (uint i = 1; i < 22; i++)
                {
                    if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                        continue;

                    if ((currentLanguageBit & languageBits) != 0)
                    {
                        var unknownData = reader.ReadBytesStrict(dataLength);
                    }

                    // Bit rotate left
                    currentLanguageBit = (currentLanguageBit << 1) | (currentLanguageBit >> 31);
                }
            }

            public static byte GetLanguageSpecificFlags(ELanguage language)
            {
                switch (language)
                {
                    case ELanguage.English:
                        return 7;

                    case ELanguage.French:
                    case ELanguage.Spanish:
                    case ELanguage.German:
                    case ELanguage.Italian:
                    case ELanguage.Portugese:
                    case ELanguage.Russian:
                    case ELanguage.Polish:
                    case ELanguage.Japanese:
                    case ELanguage.LATAMSP:
                    case ELanguage.LATAMPOR:
                    case ELanguage.Arabic:
                        return 3;
                }

                return 1;
            }
        }

        public partial class LocalizedTextResource
        {
            private const uint LanguageCount = (uint)ELanguage.Chinese_Simplified;

            public byte[][] TextData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                // Keep this as an array of bytes for the time being since I don't know the encoding type
                TextData = new byte[LanguageCount][];

                for (uint i = 0; i < LanguageCount; i++)
                {
                    ushort stringLength = reader.ReadUInt16();

                    if (stringLength > 0)
                        TextData[i] = reader.ReadBytesStrict(stringLength);
                }
            }

            public string GetStringForLanguage(ELanguage language)
            {
                if (language == ELanguage.Unknown)
                    throw new ArgumentException("Invalid language", nameof(language));

                return Encoding.UTF8.GetString(TextData[(int)language - 1]);
            }
        }

        public partial class MorphemeAnimationResource
        {
            public byte[] MorphemeData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint morphemeDataLength = reader.ReadUInt32();

                if (morphemeDataLength > 0)
                    MorphemeData = reader.ReadBytesStrict(morphemeDataLength);
            }
        }

        public partial class MusicResource
        {
            public byte[] MusicData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint dataLength = reader.ReadUInt32();

                if (dataLength > 0)
                    MusicData = reader.ReadBytesStrict(dataLength);

                for (uint i = 0; i < StreamingBankNames.Count; i++)
                {
                    ReadGenericThing(reader);
                }
            }
        }

        public partial class Pose
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                bool hasExtraData = reader.ReadBooleanStrict();

                if (hasExtraData)
                {
                    uint count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData1 = reader.ReadBytesStrict(count * 48);
                        var unknownData2 = reader.ReadBytesStrict(count * 64);
                    }

                    count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData3 = reader.ReadBytesStrict(count * 4);
                    }
                }
            }
        }

        public partial class PhysicsRagdollResource
        {
            public byte[] HavokData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                // Generic havok reader
                uint havokDataLength = reader.ReadUInt32();

                if (havokDataLength > 0)
                {
                    // Havok 2014 HKX file ("hk_2014.2.0-r1")
                    HavokData = reader.ReadBytesStrict(havokDataLength);
                }
            }
        }

        public partial class PhysicsShapeResource
        {
            public byte[] HavokData;

            public void DeserializeExtraData(BinaryReader reader)
            {
                // Generic havok reader
                uint havokDataLength = reader.ReadUInt32();

                if (havokDataLength > 0)
                {
                    // Havok 2014 HKX file ("hk_2014.2.0-r1")
                    HavokData = reader.ReadBytesStrict(havokDataLength);
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
                ShaderData = reader.ReadBytesStrict(shaderDataLength);
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
                var resourceGUID = reader.ReadBytesStrict(16);          // 16

                uint hwTextureSize = reader.ReadUInt32();
                var hwTextureData = reader.ReadBytesStrict(hwTextureSize);
            }
        }

        public partial class TextureList
        {
            Texture[] Textures;

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint textureCount = reader.ReadUInt32();

                if (textureCount > 0)
                {
                    Textures = new Texture[textureCount];

                    for (uint i = 0; i < textureCount; i++)
                    {
                        Textures[i] = new Texture();
                        Textures[i].DeserializeExtraData(reader);
                    }
                }
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
                bool isStreaming = reader.ReadBooleanStrict();

                Buffers = new HwBuffer[vertexStreamCount];

                for (uint i = 0; i < vertexStreamCount; i++)
                {
                    uint unknownFlags = reader.ReadUInt32();
                    uint vertexByteStride = reader.ReadUInt32();
                    uint unknownCounter = reader.ReadUInt32();

                    for (uint j = 0; j < unknownCounter; j++)
                    {
                        // 4 bytes read separately
                        var packedData = reader.ReadBytesStrict(4);
                    }

                    // Likely GUID or at least an identifier
                    var resourceGUID = reader.ReadBytesStrict(16);

                    Buffers[i] = HwBuffer.FromVertexData(reader, isStreaming, vertexByteStride, vertexElementCount);
                }
            }
        }

        public partial class WaveResource
        {
            [Flags]
            public enum Flags : byte
            {
                Streaming = 1,
                VBR = 2,
                EncodingQualityMask = 15,
            }

            public void DeserializeExtraData(BinaryReader reader)
            {
                if (IsStreaming)
                    ReadGenericThing(reader);
            }

            public byte EncodeFlags()
            {
                byte flags = 0;
                flags |= (byte)(IsStreaming ? Flags.Streaming : 0);
                flags |= (byte)(UseVBR ? Flags.VBR : 0);
                flags |= (byte)(((byte)EncodingQuality & (byte)Flags.EncodingQualityMask) << 2);

                return flags;
            }

            public void DecodeFlags(byte value)
            {
                IsStreaming = (value & (byte)Flags.Streaming) != 0;
                UseVBR = (value & (byte)Flags.VBR) != 0;
                EncodingQuality = (EWaveDataEncodingQuality)((value >> 2) & (byte)Flags.EncodingQualityMask);
            }
        }
    }
}

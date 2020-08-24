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
                var str = Encoding.UTF8.GetString(reader.ReadBytes((int)stringLength));
            }

            // Likely file offsets or length
            var unknown1 = reader.ReadUInt64();
            var unknown2 = reader.ReadUInt64();
        }

        public static void HwBuffer_Deserialize(BinaryReader reader)
        {
            ReadGenericThing(reader);
        }

        public partial class IndexArrayResource
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                uint count = reader.ReadUInt32();

                if (count > 0)
                {
                    uint flags = reader.ReadUInt32();
                    uint type = reader.ReadUInt32();
                    uint unknown = reader.ReadUInt32();
                    var unknownData = reader.ReadBytes(16);

                    HwBuffer_Deserialize(reader);
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
            public void DeserializeExtraData(BinaryReader reader)
            {
                for (var lang = ELanguage.English; lang <= ELanguage.Chinese_Simplified; lang++)
                {
                    ushort stringLength = reader.ReadUInt16();

                    if (stringLength > 0)
                    {
                        var str = Encoding.UTF8.GetString(reader.ReadBytes(stringLength));
                    }
                }
            }
        }

        public partial class Pose
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                bool hasExtraData = reader.ReadBoolean();

                if (hasExtraData)
                {
                    uint count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData1 = reader.ReadBytes((int)count * 48);
                        var unknownData2 = reader.ReadBytes((int)count * 64);
                    }

                    count = reader.ReadUInt32();

                    if (count > 0)
                    {
                        var unknownData3 = reader.ReadBytes((int)count * 4);
                    }
                }
            }
        }

        public partial class PhysicsShapeResource
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                uint havokDataLength = reader.ReadUInt32();

                if (havokDataLength > 0)
                {
                    // Havok 2014 HKX file ("hk_2014.2.0-r1")
                    var havokData = reader.ReadBytes((int)havokDataLength);
                }
            }
        }

        public partial class ShaderResource
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
                uint shaderDataLength = reader.ReadUInt32();

                // Likely a hash
                uint unknown1 = reader.ReadUInt32();
                uint unknown2 = reader.ReadUInt32();
                uint unknown3 = reader.ReadUInt32();
                uint unknown4 = reader.ReadUInt32();

                // Contains the actual DXBC/DXIL shader
                var shaderData = reader.ReadBytes((int)shaderDataLength);
            }
        }

        public partial class VertexArrayResource
        {
            private struct UnknownStruct1
            {
                public uint Unknown;
                public uint Counter;
            }

            public void DeserializeExtraData(BinaryReader reader)
            {
                uint unknown1 = reader.ReadUInt32();
                uint unknown2 = reader.ReadUInt32();
                byte unknown3 = reader.ReadByte();

                for (int i = 0; i < unknown2; i++)
                {
                    uint unknown4 = reader.ReadUInt32();

                    var unknownStruct1 = new UnknownStruct1();
                    unknownStruct1.Unknown = reader.ReadUInt32();
                    unknownStruct1.Counter = reader.ReadUInt32();

                    for (int j = 0; j < unknownStruct1.Counter; j++)
                    {
                        // 4 bytes read separately
                        var packedData = reader.ReadBytes(4);
                    }

                    // 16 byte structure
                    var unknownData = reader.ReadBytes(16);

                    HwBuffer_Deserialize(reader);
                }
            }
        }
    }
}

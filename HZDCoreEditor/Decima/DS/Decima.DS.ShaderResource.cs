using Utility;
using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x16BB69A9E5AA0D9E, GameType.DS)]
    public class ShaderResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public byte[] ShaderData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint shaderDataLength = reader.ReadUInt32();

            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
            Unknown3 = reader.ReadUInt32();
            Unknown4 = reader.ReadUInt32();

            ShaderData = reader.ReadBytesStrict(shaderDataLength);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)ShaderData.Length);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);

            writer.Write(ShaderData);
        }
    }
}
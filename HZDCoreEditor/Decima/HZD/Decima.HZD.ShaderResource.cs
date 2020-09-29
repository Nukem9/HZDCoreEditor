using Utility;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x4D2EE6C0D0D06791, GameType.HZD)]
    public class ShaderResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public byte[] ShaderData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint shaderDataLength = reader.ReadUInt32();

            // Likely a GUID read as 4 uints
            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
            Unknown3 = reader.ReadUInt32();
            Unknown4 = reader.ReadUInt32();

            // Contains the actual DXBC/DXIL shader
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

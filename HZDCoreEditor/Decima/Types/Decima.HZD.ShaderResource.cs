using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x4D2EE6C0D0D06791)]
    public class ShaderResource : Resource, RTTI.IExtraBinaryDataCallback
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
}

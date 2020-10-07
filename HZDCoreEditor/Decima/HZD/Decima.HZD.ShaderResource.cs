using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x4D2EE6C0D0D06791, GameType.HZD)]
    public class ShaderResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwShader Shader;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Shader = HwShader.FromData(reader, GameType.HZD);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Shader.ToData(writer, GameType.HZD);
        }
    }
}

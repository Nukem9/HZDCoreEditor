using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x16BB69A9E5AA0D9E, GameType.DS)]
    public class ShaderResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwShader Shader;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Shader = HwShader.FromData(reader, GameType.DS);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Shader.ToData(writer, GameType.DS);
        }
    }
}
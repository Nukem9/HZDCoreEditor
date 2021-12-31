using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0xA664164D69FD2B38, GameType.DS)]
    public class Texture : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwTexture Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwTexture.FromData(reader, GameType.DS);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer, GameType.DS);
        }
    }
}

using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xF2E1AFB7052B3866, GameType.HZD)]
    public class Texture : Resource, RTTI.IExtraBinaryDataCallback
    {
        public HwTexture Buffer;

        public void DeserializeExtraData(BinaryReader reader)
        {
            Buffer = HwTexture.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            Buffer.ToData(writer);
        }
    }
}

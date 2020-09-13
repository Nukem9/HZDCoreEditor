using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x9C78E9FDC6042A60)]
    public class UITexture : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(0, 0x30, "Logic")] public String TextureName;
        [RTTI.Member(1, 0x38, "Logic")] public ISize Size;
        public uint HiResDataSize;
        public uint LowResDataSize;
        public Texture HiResTexture;   // Screen res >  1920x1080 (Default if low res not present)
        public Texture LowResTexture;  // Screen res <= 1920x1080

        public void DeserializeExtraData(BinaryReader reader)
        {
            HiResDataSize = reader.ReadUInt32();
            LowResDataSize = reader.ReadUInt32();

            HiResTexture = new Texture();
            HiResTexture.DeserializeExtraData(reader);

            if (LowResDataSize > 0)
            {
                LowResTexture = new Texture();
                LowResTexture.DeserializeExtraData(reader);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write(HiResDataSize);
            writer.Write(LowResDataSize);

            HiResTexture.SerializeExtraData(writer);

            if (LowResTexture != null)
                LowResTexture.SerializeExtraData(writer);
        }
    }
}

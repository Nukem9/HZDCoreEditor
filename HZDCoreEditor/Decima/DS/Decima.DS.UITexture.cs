using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x292F8B9447AF20A8, GameType.DS)]
    public class UITexture : Resource, RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        [RTTI.BaseClass(0x20)] public StreamingRefTarget @StreamingRefTarget;
        [RTTI.Member(6, 0x40, "Logic")] public String TextureName;
        [RTTI.Member(7, 0x48, "Logic")] public ISize Size;
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
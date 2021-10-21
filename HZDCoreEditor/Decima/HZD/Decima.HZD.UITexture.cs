using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x9C78E9FDC6042A60, GameType.HZD)]
    public class UITexture : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(5, 0x30, "Logic")] public String TextureName;
        [RTTI.Member(6, 0x38, "Logic")] public ISize Size;
        public Texture LowResTexture;  // Screen res <= 1920x1080 (Default if high res not present)
        public Texture HiResTexture;   // Screen res >  1920x1080

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint lowResDataSize = reader.ReadUInt32();
            uint hiResDataSize = reader.ReadUInt32();

            LowResTexture = new Texture();
            LowResTexture.DeserializeExtraData(reader);

            if (hiResDataSize > 0)
            {
                HiResTexture = new Texture();
                HiResTexture.DeserializeExtraData(reader);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            // Write to a separate buffer in order to get the block lengths
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            LowResTexture.SerializeExtraData(bw);
            long lowResDataSize = ms.Length;

            if (HiResTexture != null)
                HiResTexture.SerializeExtraData(bw);

            long hiResDataSize = ms.Length - lowResDataSize;

            writer.Write((uint)lowResDataSize);
            writer.Write((uint)hiResDataSize);
            ms.WriteTo(writer.BaseStream);
        }
    }
}

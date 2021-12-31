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
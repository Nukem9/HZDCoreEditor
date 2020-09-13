using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x321F4B133D40A266)]
    public class TextureList : Resource, RTTI.IExtraBinaryDataCallback
    {
        public Texture[] Textures;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint textureCount = reader.ReadUInt32();

            if (textureCount > 0)
            {
                Textures = new Texture[textureCount];

                for (uint i = 0; i < Textures.Length; i++)
                {
                    Textures[i] = new Texture();
                    Textures[i].DeserializeExtraData(reader);
                }
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)Textures.Length);

            if (Textures.Length > 0)
            {
                for (uint i = 0; i < Textures.Length; i++)
                    Textures[i].SerializeExtraData(writer);
            }
        }
    }
}

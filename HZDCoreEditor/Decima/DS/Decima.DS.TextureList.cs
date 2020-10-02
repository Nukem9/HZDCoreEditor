using System.Collections.Generic;
using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x2CD10F6FF48962C9, GameType.DS)]
    public class TextureList : Resource, RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        public List<Texture> Textures;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint textureCount = reader.ReadUInt32();
            Textures = new List<Texture>((int)textureCount);

            for (uint i = 0; i < textureCount; i++)
            {
                var x = new Texture();
                x.DeserializeExtraData(reader);

                Textures.Add(x);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)Textures.Count);

            foreach (var texture in Textures)
                texture.SerializeExtraData(writer);
        }
    }
}
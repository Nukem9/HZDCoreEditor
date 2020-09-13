using BinaryStreamExtensions;
using System;
using System.IO;
using System.Text;

namespace Decima.HZD
{
    [RTTI.Serializable(0xB89A596B420BB2E2)]
    public class LocalizedTextResource : ResourceWithoutLegacyName, RTTI.IExtraBinaryDataCallback
    {
        private const uint LanguageCount = (uint)ELanguage.Chinese_Simplified;

        public byte[][] TextData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            // Keep this as an array of bytes for the time being since I don't know the encoding type
            TextData = new byte[LanguageCount][];

            for (uint i = 0; i < TextData.Length; i++)
            {
                ushort stringLength = reader.ReadUInt16();
                TextData[i] = reader.ReadBytesStrict(stringLength);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            for (uint i = 0; i < TextData.Length; i++)
            {
                writer.Write((ushort)TextData[i].Length);

                if (TextData[i].Length > 0)
                    writer.Write(TextData[i]);
            }
        }

        public string GetStringForLanguage(ELanguage language)
        {
            if (language == ELanguage.Unknown)
                throw new ArgumentException("Invalid language", nameof(language));

            return Encoding.UTF8.GetString(TextData[(int)language - 1]);
        }
    }
}

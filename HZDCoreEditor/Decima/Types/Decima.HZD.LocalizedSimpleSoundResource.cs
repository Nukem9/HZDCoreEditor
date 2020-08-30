using BinaryStreamExtensions;
using System.IO;
using System.Text;

namespace Decima.HZD
{
    [RTTI.Serializable(0xF73C79417552568E)]
    public class LocalizedSimpleSoundResource : SimpleSoundResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(0, 0x100, "General")] public Ref<SoundMixStateResource> SoundMixState;
        [RTTI.Member(1, 0x108, "General")] public Ref<LocalizedSoundPreset> Preset;
        string SoundDataFilePath;
        WaveResource SoundFormat;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                SoundDataFilePath = Encoding.UTF8.GetString(reader.ReadBytes(stringLength));

            ushort languageBits = reader.ReadUInt16();
            byte dataLength = reader.ReadByte();

            // Yes, they really did this manually
            SoundFormat = new WaveResource();
            SoundFormat.DecodeFlags(reader.ReadByte());
            SoundFormat.FrameSize = reader.ReadUInt16();
            SoundFormat.Encoding = (EWaveDataEncoding)reader.ReadByte();
            SoundFormat.ChannelCount = reader.ReadByte();
            SoundFormat.SampleRate = reader.ReadInt32();
            SoundFormat.BitsPerSample = reader.ReadUInt16();
            SoundFormat.BitsPerSecond = reader.ReadUInt32();
            SoundFormat.BlockAlignment = reader.ReadUInt16();
            SoundFormat.FormatTag = reader.ReadUInt16();

            uint currentLanguageBit = 1;
            for (uint i = 1; i < 22; i++)
            {
                if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                    continue;

                if ((currentLanguageBit & languageBits) != 0)
                {
                    var unknownData = reader.ReadBytesStrict(dataLength);
                }

                // Bit rotate left
                currentLanguageBit = (currentLanguageBit << 1) | (currentLanguageBit >> 31);
            }
        }

        public static byte GetLanguageSpecificFlags(ELanguage language)
        {
            switch (language)
            {
                case ELanguage.English:
                    return 7;

                case ELanguage.French:
                case ELanguage.Spanish:
                case ELanguage.German:
                case ELanguage.Italian:
                case ELanguage.Portugese:
                case ELanguage.Russian:
                case ELanguage.Polish:
                case ELanguage.Japanese:
                case ELanguage.LATAMSP:
                case ELanguage.LATAMPOR:
                case ELanguage.Arabic:
                    return 3;
            }

            return 1;
        }
    }
}

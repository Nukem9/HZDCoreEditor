using Utility;
using System.IO;
using System.Numerics;
using System.Text;
using HZDCoreEditor.Util;

namespace Decima.HZD
{
    [RTTI.Serializable(0xF73C79417552568E, GameType.HZD)]
    public class LocalizedSimpleSoundResource : SimpleSoundResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(39, 0x100, "General")] public Ref<SoundMixStateResource> SoundMixState;
        [RTTI.Member(40, 0x108, "General")] public Ref<LocalizedSoundPreset> Preset;
        public string SoundDataFilePath;
        public ushort LanguageBits;
        public byte DataLength;
        public WaveResource SoundFormat;
        public byte[][] UnknownData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                SoundDataFilePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));

            LanguageBits = reader.ReadUInt16();
            DataLength = reader.ReadByte();

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

            UnknownData = new byte[22][];
            uint currentLanguageBit = 1;
            for (uint i = 1; i < 22; i++)
            {
                if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                    continue;

                if ((currentLanguageBit & LanguageBits) != 0)
                {
                    UnknownData[i] = reader.ReadBytesStrict(DataLength);
                }

                currentLanguageBit = BitOperations.RotateLeft(currentLanguageBit, 1);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)SoundDataFilePath.Length);

            if (SoundDataFilePath.Length > 0)
                writer.Write(Encoding.UTF8.GetBytes(SoundDataFilePath));

            writer.Write(LanguageBits);
            writer.Write(DataLength);

            writer.Write(SoundFormat.EncodeFlags());
            writer.Write(SoundFormat.FrameSize);
            writer.Write((byte)SoundFormat.Encoding);
            writer.Write(SoundFormat.ChannelCount);
            writer.Write(SoundFormat.SampleRate);
            writer.Write(SoundFormat.BitsPerSample);
            writer.Write(SoundFormat.BitsPerSecond);
            writer.Write(SoundFormat.BlockAlignment);
            writer.Write(SoundFormat.FormatTag);

            uint currentLanguageBit = 1;
            for (uint i = 1; i < 22; i++)
            {
                if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                    continue;

                if ((currentLanguageBit & LanguageBits) != 0)
                    writer.Write(UnknownData[i]);

                currentLanguageBit = BitOperations.RotateLeft(currentLanguageBit, 1);
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

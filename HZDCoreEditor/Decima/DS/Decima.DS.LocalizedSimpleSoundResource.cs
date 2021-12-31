using HZDCoreEditor.Util;
using System.IO;

namespace Decima.DS
{
    using uint32 = System.UInt32;

    [RTTI.Serializable(0xC726DF870437D774, GameType.DS)]
    public class LocalizedSimpleSoundResource : WwiseSimpleSoundResource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(44, 0x120, "General")] public Ref<SoundMixStateResource> SoundMixState;
        [RTTI.Member(45, 0x128, "General")] public Ref<LocalizedSoundPreset> Preset;
        [RTTI.Member(46, 0x130, "General")] public Array<float> LengthInSeconds;
        [RTTI.Member(47, 0x140, "General")] public Array<uint32> WemIDs;
        [RTTI.Member(48, 0x150, "General")] public float VolumeCorrectionRTPCValue;
        public ushort LanguageBits;
        public WaveResource SoundFormat;
        public byte[] UnknownStreamBytes;
        public StreamHandle[] StreamHandles;

        public void DeserializeExtraData(BinaryReader reader)
        {
            LanguageBits = reader.ReadUInt16();

            // Fields serialized manually
            SoundFormat = new WaveResource();
            SoundFormat.IsStreaming = reader.ReadBooleanStrict();
            SoundFormat.UseVBR = reader.ReadBooleanStrict();
            SoundFormat.EncodingQuality = (EWaveDataEncodingQuality)reader.ReadByte();
            SoundFormat.FrameSize = reader.ReadUInt16();
            SoundFormat.Encoding = (EWaveDataEncoding)reader.ReadByte();
            SoundFormat.ChannelCount = reader.ReadByte();
            SoundFormat.SampleRate = reader.ReadInt32();
            SoundFormat.BitsPerSample = reader.ReadUInt16();
            SoundFormat.BitsPerSecond = reader.ReadUInt32();
            SoundFormat.BlockAlignment = reader.ReadUInt16();
            SoundFormat.FormatTag = reader.ReadUInt16();

            UnknownStreamBytes = new byte[26];
            StreamHandles = new StreamHandle[26];
            uint currentLanguageBit = 1;

            for (uint i = 1; i < 26; i++)
            {
                if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                    continue;

                if ((currentLanguageBit & LanguageBits) != 0)
                {
                    UnknownStreamBytes[i] = reader.ReadByte();
                    StreamHandles[i] = StreamHandle.FromData(reader);
                }

                currentLanguageBit = BitOperations.RotateLeft(currentLanguageBit, 1);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write(LanguageBits);

            writer.Write(SoundFormat.IsStreaming);
            writer.Write(SoundFormat.UseVBR);
            writer.Write((byte)SoundFormat.EncodingQuality);
            writer.Write(SoundFormat.FrameSize);
            writer.Write((byte)SoundFormat.Encoding);
            writer.Write(SoundFormat.ChannelCount);
            writer.Write(SoundFormat.SampleRate);
            writer.Write(SoundFormat.BitsPerSample);
            writer.Write(SoundFormat.BitsPerSecond);
            writer.Write(SoundFormat.BlockAlignment);
            writer.Write(SoundFormat.FormatTag);

            uint currentLanguageBit = 1;
            for (uint i = 1; i < 26; i++)
            {
                if ((GetLanguageSpecificFlags((ELanguage)i) & 2) == 0)
                    continue;

                if ((currentLanguageBit & LanguageBits) != 0)
                {
                    writer.Write(UnknownStreamBytes[i]);
                    StreamHandles[i].ToData(writer);
                }

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
                case ELanguage.Portuguese:
                case ELanguage.Russian:
                case ELanguage.Polish:
                case ELanguage.Japanese:
                case ELanguage.LATAMSP:
                case ELanguage.LATAMPOR:
                case ELanguage.Greek:
                    return 3;
            }

            return 1;
        }
    }
}
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
    }
}
namespace Decima.DS
{
    using uint8 = System.Byte;
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x4C7DBDD040598FCC, GameType.DS)]
    public class MusicResource : Resource
    {
        [RTTI.Member(8, 0x20)] public Array<uint8> WorkspaceBuffer;
        [RTTI.Member(7, 0x30)] public Array<StreamingDataSource> StreamingDataSources;
        [RTTI.Member(6, 0x40)] public Array<uint32> StreamingDataHash;
        [RTTI.Member(2, 0x50)] public int BitRate;
        [RTTI.Member(3, 0x54)] public bool StripSilence;
        [RTTI.Member(4, 0x58)] public int StripSilenceThreshold;
        [RTTI.Member(5, 0x60)] public Array<MusicSubmixBinding> SubmixBindings;
    }
}
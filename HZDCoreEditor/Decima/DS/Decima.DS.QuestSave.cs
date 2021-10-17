namespace Decima.DS
{
    [RTTI.Serializable(0x3B616D6260E90151, GameType.DS)]
    public class QuestSave : RTTIObject
    {
        [RTTI.Member(1, 0x10, "StateSaving", true)] public GGUUID QuestResourceUUID;
        [RTTI.Member(2, 0x20, "StateSaving", true)] public EQuestState State;
        [RTTI.Member(3, 0x24, "StateSaving", true)] public bool Tracked;
        [RTTI.Member(4, 0x25, "StateSaving", true)] public bool TrackingEnabled;
        [RTTI.Member(7, 0x26, "StateSaving", true)] public EQuestRunState RunState;
        [RTTI.Member(5, 0x28, "StateSaving", true)] public int StartTime;
        [RTTI.Member(6, 0x2C, "StateSaving", true)] public int LastProgressTime;
        [RTTI.Member(8, 0x30, "StateSaving", true)] public bool RewindCounter;
        [RTTI.Member(9, 0x58, "StateSaving", true)] public int Version;
    }
}
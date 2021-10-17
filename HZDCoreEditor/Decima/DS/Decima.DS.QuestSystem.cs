namespace Decima.DS
{
    [RTTI.Serializable(0x8856458E7579B20F, GameType.DS)]
    public class QuestSystem : RTTIObject
    {
        [RTTI.Member(1, 0x38, "StateSaving", true)] public CPtr<Story> Story;
        [RTTI.Member(3, 0x40, "StateSaving", true)] public CPtr<QuestSectionUpdateQueue> UpdateQueue;
        [RTTI.Member(4, 0x48, "StateSaving", true)] public CPtr<DynamicQuestManager> DynamicQuestManager;
        [RTTI.Member(2, 0x60, "StateSaving", true)] public Array<GGUUID> TrackedQuestHistory;
    }
}
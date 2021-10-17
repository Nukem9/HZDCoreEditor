namespace Decima.DS
{
    [RTTI.Serializable(0x88F7BBB436CC46F3, GameType.DS)]
    public class MissionSettings : RTTIObject
    {
        [RTTI.Member(0, 0x8)] public EMissionType Type;
        [RTTI.Member(2, 0xC)] public float TimeLimit;
        [RTTI.Member(1, 0x10)] public int ObjectiveLimit;
    }
}
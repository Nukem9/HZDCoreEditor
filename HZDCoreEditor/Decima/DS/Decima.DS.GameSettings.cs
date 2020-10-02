namespace Decima.DS
{
    [RTTI.Serializable(0x7C86466AA739E334, GameType.DS)]
    public class GameSettings : PlaylistData
    {
        [RTTI.Member(3, 0x28)] public Array<Ref<GameRoundSettings>> RoundSettings;
    }
}
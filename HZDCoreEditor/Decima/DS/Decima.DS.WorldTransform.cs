namespace Decima.DS
{
    [RTTI.Serializable(0x23C32AD4512B105E, GameType.DS)]
    public class WorldTransform
    {
        [RTTI.Member(0, 0x0)] public WorldPosition Position;
        [RTTI.Member(1, 0x18)] public RotMatrix Orientation;
    }
}
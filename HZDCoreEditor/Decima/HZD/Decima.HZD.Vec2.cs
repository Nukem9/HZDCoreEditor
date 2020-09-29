namespace Decima.HZD
{
    [RTTI.Serializable(0xEF5867B44F765A66, GameType.HZD)]
    public class Vec2 : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public float X;
        [RTTI.Member(1, 0x4)] public float Y;

        public void DeserializeStateObject(SaveState state)
        {
            X = state.Reader.ReadSingle();
            Y = state.Reader.ReadSingle();
        }
    }
}

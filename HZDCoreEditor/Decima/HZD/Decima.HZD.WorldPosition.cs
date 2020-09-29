namespace Decima.HZD
{
    [RTTI.Serializable(0xB7FA97B1A5E636C9, GameType.HZD)]
    public class WorldPosition : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public double X;
        [RTTI.Member(1, 0x8)] public double Y;
        [RTTI.Member(2, 0x10)] public double Z;

        public void DeserializeStateObject(SaveState state)
        {
            X = state.Reader.ReadDouble();
            Y = state.Reader.ReadDouble();
            Z = state.Reader.ReadDouble();
        }
    }
}
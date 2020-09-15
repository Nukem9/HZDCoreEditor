namespace Decima.HZD
{
    [RTTI.Serializable(0x4760E98FA8545BCF)]
    public class Vec3 : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public float X;
        [RTTI.Member(1, 0x4)] public float Y;
        [RTTI.Member(2, 0x8)] public float Z;

        public void DeserializeStateObject(SaveState state)
        {
            X = state.Reader.ReadSingle();
            Y = state.Reader.ReadSingle();
            Z = state.Reader.ReadSingle();
        }
    }
}
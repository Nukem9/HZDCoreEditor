namespace Decima.HZD
{
    [RTTI.Serializable(0xC0E251CE6C81D04D)]
    public class RotMatrix : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public Vec3Pack Col0;
        [RTTI.Member(1, 0xC)] public Vec3Pack Col1;
        [RTTI.Member(2, 0x18)] public Vec3Pack Col2;

        public void DeserializeStateObject(SaveState state)
        {
            Col0 = new Vec3Pack()
            {
                X = state.Reader.ReadSingle(),
                Y = state.Reader.ReadSingle(),
                Z = state.Reader.ReadSingle(),
            };

            Col1 = new Vec3Pack()
            {
                X = state.Reader.ReadSingle(),
                Y = state.Reader.ReadSingle(),
                Z = state.Reader.ReadSingle(),
            };

            Col2 = new Vec3Pack()
            {
                X = state.Reader.ReadSingle(),
                Y = state.Reader.ReadSingle(),
                Z = state.Reader.ReadSingle(),
            };
        }
    }
}
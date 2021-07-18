using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0xD010735A1623DE72, GameType.HZD)]
    public class IVec2 : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public int X;
        [RTTI.Member(1, 0x4)] public int Y;

        public void DeserializeStateObject(SaveState state)
        {
            X = state.Reader.ReadInt32();
            Y = state.Reader.ReadInt32();
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}

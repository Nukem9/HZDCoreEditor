using HZDCoreEditor.Util;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3723258333B81F86, GameType.HZD)]
    public class WorldState : RTTIRefObject
    {
        [RTTI.Member(3, 0x98, "TimeOfDay")] public float TimeOfDay;
        [RTTI.Member(4, 0xA4, "TimeOfDay")] public bool EnableDayNightCycle;
        public int Unknown1;// Offset 0xA0
        public float Unknown2;// Offset 0xA8
        public float Unknown3;// Offset 0xAC

        public void ReadSave(SaveState state)
        {
            TimeOfDay = state.Reader.ReadSingle();
            Unknown1 = state.Reader.ReadInt32();
            EnableDayNightCycle = state.Reader.ReadBooleanStrict();
            Unknown2 = state.Reader.ReadSingle();
            Unknown3 = state.Reader.ReadSingle();
        }
    }
}
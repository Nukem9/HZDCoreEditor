using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3723258333B81F86)]
    public class WorldState : RTTIRefObject
    {
        [RTTI.Member(0, 0x98, "TimeOfDay")] public float TimeOfDay;
        [RTTI.Member(1, 0xA4, "TimeOfDay")] public bool EnableDayNightCycle;
        int Unknown1;// Offset 0xA0
        float Unknown2;// Offset 0xA8
        float Unknown3;// Offset 0xAC

        public void ReadSave(SaveDataSerializer serializer)
        {
            TimeOfDay = serializer.Reader.ReadSingle();
            Unknown1 = serializer.Reader.ReadInt32();
            EnableDayNightCycle = serializer.Reader.ReadBooleanStrict();
            Unknown2 = serializer.Reader.ReadSingle();
            Unknown3 = serializer.Reader.ReadSingle();
        }
    }
}
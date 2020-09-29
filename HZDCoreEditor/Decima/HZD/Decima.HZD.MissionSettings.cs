namespace Decima.HZD
{
    [RTTI.Serializable(0x96AAE0EC4A77AB0E, GameType.HZD)]
    public class MissionSettings : RTTIObject
    {
        [RTTI.Member(0, 0x8)] public EMissionType Type;
        [RTTI.Member(1, 0xC)] public float TimeLimit;
        [RTTI.Member(2, 0x10)] public int ObjectiveLimit;
        public int Unknown;// Offset 0x14

        public void ReadSave(SaveState state)
        {
            Type = (EMissionType)state.Reader.ReadByte();
            TimeLimit = state.Reader.ReadSingle();
            ObjectiveLimit = state.Reader.ReadInt32();
            Unknown = state.Reader.ReadInt32();
        }
    }
}
namespace Decima.HZD
{
    [RTTI.Serializable(0x96AAE0EC4A77AB0E)]
    public class MissionSettings : RTTIObject
    {
        [RTTI.Member(0, 0x8)] public EMissionType Type;
        [RTTI.Member(1, 0xC)] public float TimeLimit;
        [RTTI.Member(2, 0x10)] public int ObjectiveLimit;
        public int Unknown;// Offset 0x14

        public void ReadSave(SaveDataSerializer serializer)
        {
            Type = (EMissionType)serializer.Reader.ReadByte();
            TimeLimit = serializer.Reader.ReadSingle();
            ObjectiveLimit = serializer.Reader.ReadInt32();
            Unknown = serializer.Reader.ReadInt32();
        }
    }
}
using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3B616D6260E90151)]
    public class QuestSave : RTTIObject, RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x10, "StateSaving", true)] public GGUUID QuestResourceUUID;
        [RTTI.Member(1, 0x20, "StateSaving", true)] public EQuestState State;
        [RTTI.Member(2, 0x24, "StateSaving", true)] public bool Tracked;
        [RTTI.Member(3, 0x25, "StateSaving", true)] public bool TrackingEnabled;
        [RTTI.Member(4, 0x26, "StateSaving", true)] public EQuestRunState RunState;
        [RTTI.Member(5, 0x28, "StateSaving", true)] public int StartTime;
        [RTTI.Member(6, 0x2C, "StateSaving", true)] public int LastProgressTime;
        [RTTI.Member(7, 0x30, "StateSaving", true)] public bool RewindCounter;
        [RTTI.Member(8, 0x58, "StateSaving", true)] public int Version;

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            // NOTE: There's a flaw in their serialization code. RewindCounter will occasionally have invalid values (non-0/1) which breaks
            // some type checking assumptions. Fields are also out of order. I guess this has to be done manually now.
            int unusedTypeIndex = serializer.ReadVariableLengthInt();

            QuestResourceUUID = serializer.ReadIndexedGUID();
            State = (EQuestState)serializer.Reader.ReadInt32();
            Tracked = serializer.Reader.ReadBooleanStrict();
            TrackingEnabled = serializer.Reader.ReadBooleanStrict();
            StartTime = serializer.ReadVariableLengthInt();
            LastProgressTime = serializer.ReadVariableLengthInt();
            RunState = (EQuestRunState)serializer.Reader.ReadSByte();
            RewindCounter = serializer.Reader.ReadByte() > 0;//RewindCounter = serializer.Reader.ReadBooleanStrict();
            Version = serializer.ReadVariableLengthInt();
        }
    }
}
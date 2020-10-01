using Utility;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3B616D6260E90151, GameType.HZD)]
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

        public void DeserializeStateObject(SaveState state)
        {
            // NOTE: There's a flaw in their serialization code. RewindCounter will occasionally have invalid values (non-0/1) which breaks
            // some type checking assumptions. Fields are also out of order. I guess this has to be done manually now.
            int unusedTypeIndex = state.ReadVariableLengthInt();

            QuestResourceUUID = new GGUUID(state.ReadIndexedGUID());
            State = (EQuestState)state.Reader.ReadInt32();
            Tracked = state.Reader.ReadBooleanStrict();
            TrackingEnabled = state.Reader.ReadBooleanStrict();
            StartTime = state.ReadVariableLengthInt();
            LastProgressTime = state.ReadVariableLengthInt();
            RunState = (EQuestRunState)state.Reader.ReadSByte();
            RewindCounter = state.Reader.ReadByte() > 0;//RewindCounter = state.Reader.ReadBooleanStrict();
            Version = state.ReadVariableLengthInt();
        }
    }
}
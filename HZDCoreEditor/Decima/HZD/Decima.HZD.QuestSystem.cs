using System;
using Utility;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8856458E7579B20F, GameType.HZD)]
    public class QuestSystem : RTTIObject, RTTI.ISaveSerializable
    {
        [RTTI.Member(1, 0x38, "StateSaving", true)] public CPtr<Story> Story;
        [RTTI.Member(3, 0x40, "StateSaving", true)] public CPtr<QuestSectionUpdateQueue> UpdateQueue;
        [RTTI.Member(4, 0x48, "StateSaving", true)] public CPtr<DynamicQuestManager> DynamicQuestManager;
        [RTTI.Member(2, 0x60, "StateSaving", true)] public Array<GGUUID> TrackedQuestHistory;
        public UnknownEntry[] UnknownArray;

        public class UnknownEntry
        {
            public List<(BaseGGUUID, List<BaseGGUUID>)> UnknownList;
        }

        public void DeserializeStateObject(SaveState state)
        {
            bool skipReading = false;

            if (state.SaveVersion >= 21)
                skipReading = state.Reader.ReadBooleanStrict();

            if (skipReading)
                return;

            state.DeserializeObjectClassMembers(typeof(QuestSystem), this);

            // Hardcoded number of entries
            UnknownArray = new UnknownEntry[4];

            for (int i = 0; i < UnknownArray.Length; i++)
            {
                UnknownArray[i] = new UnknownEntry();

                UnknownArray[i].UnknownList = state.ReadVariableItemList((ref (BaseGGUUID GUID, List<BaseGGUUID> GUIDList) e) =>
                {
                    e.GUID = state.ReadIndexedGUID();
                    e.GUIDList = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
                    {
                        GUID = state.ReadIndexedGUID();
                    });
                });
            }
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
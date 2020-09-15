using BinaryStreamExtensions;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8856458E7579B20F)]
    public class QuestSystem : RTTIObject, RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x38, "StateSaving", true)] public CPtr<Story> Story;
        [RTTI.Member(1, 0x40, "StateSaving", true)] public CPtr<QuestSectionUpdateQueue> UpdateQueue;
        [RTTI.Member(2, 0x48, "StateSaving", true)] public CPtr<DynamicQuestManager> DynamicQuestManager;
        [RTTI.Member(3, 0x60, "StateSaving", true)] public Array<GGUUID> TrackedQuestHistory;
        public UnknownEntry[] UnknownArray;

        public class UnknownEntry
        {
            public List<(GGUUID, List<GGUUID>)> UnknownList;
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

                UnknownArray[i].UnknownList = state.ReadVariableItemList((int i, ref (GGUUID GUID, List<GGUUID> GUIDList) e) =>
                {
                    e.GUID = state.ReadIndexedGUID();
                    e.GUIDList = state.ReadVariableItemList((int i, ref GGUUID GUID) =>
                    {
                        GUID = state.ReadIndexedGUID();
                    });
                });
            }
        }
    }
}
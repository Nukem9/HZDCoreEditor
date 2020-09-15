using System;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x70DDECB5743C9A59)]
    public class Story : RTTIObject, RTTI.ISaveSerializable
    {
        public List<QuestSaveInstance> QuestSaveInstances;

        public class QuestSaveInstance
        {
            public QuestSave SaveObject;
            public List<UnknownEntry1> UnknownList1;
            public List<UnknownEntry2> UnknownList2;

            public class UnknownEntry1
            {
                public GGUUID GUID1;
                public GGUUID GUID2;
                public int Unknown1;
                public int Unknown2;
            }

            public class UnknownEntry2
            {
                public GGUUID GUID;
                public QuestObjectiveSave ObjectiveSaveObject;
            }
        }

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(Story), this);

            QuestSaveInstances = state.ReadVariableItemList((int i, ref QuestSaveInstance instance) =>
            {
                instance.SaveObject = state.ReadObjectHandle() as QuestSave;

                instance.UnknownList1 = state.ReadVariableItemList((int i, ref QuestSaveInstance.UnknownEntry1 e) =>
                {
                    e.GUID1 = state.ReadIndexedGUID();
                    e.Unknown1 = state.ReadVariableLengthInt();// EQuestSectionState?

                    if (e.Unknown1 > 3)
                        throw new Exception();

                    e.GUID2 = state.ReadIndexedGUID();
                    e.Unknown2 = state.Reader.ReadInt32();
                });

                instance.UnknownList2 = state.ReadVariableItemList((int i, ref QuestSaveInstance.UnknownEntry2 e) =>
                {
                    e.GUID = state.ReadIndexedGUID();
                    e.ObjectiveSaveObject = state.ReadObjectHandle() as QuestObjectiveSave;
                });
            });
        }
    }
}
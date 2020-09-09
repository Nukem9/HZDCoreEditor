using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8856458E7579B20F)]
    public class QuestSystem : RTTIObject, RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x38, "StateSaving", true)] public Ptr<Story> Story;
        [RTTI.Member(1, 0x40, "StateSaving", true)] public Ptr<QuestSectionUpdateQueue> UpdateQueue;
        [RTTI.Member(2, 0x48, "StateSaving", true)] public Ptr<DynamicQuestManager> DynamicQuestManager;
        [RTTI.Member(3, 0x60, "StateSaving", true)] public Array<GGUUID> TrackedQuestHistory;

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            bool skipReading = false;

            if (serializer.FileDataVersion >= 21)
                skipReading = serializer.Reader.ReadBooleanStrict();

            if (skipReading)
                return;

            serializer.ManuallyResolveClassMembers(typeof(QuestSystem), this);

            for (int i = 0; i < 4; i++)
            {
                int count = serializer.ReadVariableLengthOffset();

                for (int j = 0; j < count; j++)
                {
                    var unknownGUID = serializer.ReadIndexedGUID();
                    int count2 = serializer.ReadVariableLengthInt();

                    for (int k = 0; k < count2; k++)
                    {
                        var unknownGUID2 = serializer.ReadIndexedGUID();
                    }
                }
            }
        }
    }
}
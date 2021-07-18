using System;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x7520F5D9A86D15E3, GameType.HZD)]
    public class GeneratedQuestSave : RTTIObject, RTTI.ISaveSerializable
    {
        [RTTI.Member(1, 0x10, "Saving", true)] public GGUUID QuestUUID;
        [RTTI.Member(2, 0x20, "Saving", true)] public GGUUID StartUUID;
        [RTTI.Member(3, 0x30, "Saving", true)] public GGUUID EndUUID;
        [RTTI.Member(4, 0x40, "Saving", true)] public GGUUID SubSectionUUID;
        [RTTI.Member(6, 0x50, "Saving", true)] public GGUUID MainObjectiveUUID;
        [RTTI.Member(7, 0x60, "Saving", true)] public GGUUID FinishObjectiveUUID;
        [RTTI.Member(5, 0x70, "Saving", true)] public GGUUID TriggerUUID;
        [RTTI.Member(8, 0x90, "Saving", true)] public GGUUID Recipe;
        [RTTI.Member(9, 0xA0, "Saving", true)] public GGUUID TradingItem;
        [RTTI.Member(10, 0xB0, "Saving", true)] public StreamingRef<EntityResource> ItemToBuy;
        [RTTI.Member(11, 0xD0, "Saving", true)] public GGUUID TurnInLocationUUID;
        [RTTI.Member(12, 0xE0, "Saving", true)] public GGUUID MerchantSpawnSetupUUID;
        [RTTI.Member(13, 0xF0, "Saving", true)] public Array<StreamingRef<EntityResource>> ItemsToTradeIn;
        [RTTI.Member(14, 0x100, "Saving", true)] public Array<int> AmountOfItemsToTradeIn;
        public List<(BaseGGUUID, BaseGGUUID)> UnknownList;

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(GeneratedQuestSave), this);

            UnknownList = state.ReadVariableItemList((ref (BaseGGUUID, BaseGGUUID) e) =>
            {
                e.Item1 = state.ReadIndexedGUID();
                e.Item2 = state.ReadIndexedGUID();
            });
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
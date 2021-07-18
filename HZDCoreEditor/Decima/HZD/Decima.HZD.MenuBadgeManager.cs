using System;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xEB18FAD2ABAD4F96, GameType.HZD)]
    public class MenuBadgeManager : StateObject, RTTI.ISaveSerializable
    {
        public List<BaseGGUUID> UnknownList1;
        public List<List<BaseGGUUID>> UnknownList2;

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(MenuBadgeManager), this);

            // MBMB
            UnknownList1 = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });

            UnknownList2 = state.ReadVariableItemList((ref List<BaseGGUUID> list) =>
            {
                list = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
                {
                    GUID = state.ReadIndexedGUID();
                });
            });
            // MBME
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
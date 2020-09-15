using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xEB18FAD2ABAD4F96)]
    public class MenuBadgeManager : StateObject, RTTI.ISaveSerializable
    {
        public List<GGUUID> UnknownList1;
        public List<List<GGUUID>> UnknownList2;

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(MenuBadgeManager), this);

            // MBMB
            UnknownList1 = state.ReadVariableItemList((int i, ref GGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });

            UnknownList2 = state.ReadVariableItemList((int i, ref List<GGUUID> list) =>
            {
                list = state.ReadVariableItemList((int i, ref GGUUID GUID) =>
                {
                    GUID = state.ReadIndexedGUID();
                });
            });
            // MBME
        }
    }
}
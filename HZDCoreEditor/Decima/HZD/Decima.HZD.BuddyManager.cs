using HZDCoreEditor.Util;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x7374806C6ED90678, GameType.HZD)]
    public class BuddyManager : RTTIObject
    {
        public List<(BaseGGUUID, byte, byte[])> UnknownList;

        public void ReadSave(SaveState state)
        {
            UnknownList = state.ReadVariableItemList((ref (BaseGGUUID GUID, byte, byte[]) e) =>
            {
                e.GUID = state.ReadIndexedGUID();
                e.Item2 = state.Reader.ReadByte();
                e.Item3 = state.Reader.ReadBytesStrict(24);
            });
        }
    }
}
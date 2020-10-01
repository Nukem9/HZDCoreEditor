using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8FB0349003667A31, GameType.HZD)]
    public class TileBasedStreamingStrategyInstance : StreamingStrategyInstance
    {
        public List<(BaseGGUUID, BaseGGUUID)> UnknownList1;
        public List<(BaseGGUUID, BaseGGUUID, byte)> UnknownList2;

        public override void ReadSave(SaveState state)
        {
            UnknownList1 = state.ReadVariableItemList((ref (BaseGGUUID Link1, BaseGGUUID Link2) e) =>
            {
                e.Link1 = state.ReadIndexedGUID();// ?
                e.Link2 = state.ReadIndexedGUID();// ?
            });

            UnknownList2 = state.ReadVariableItemList((ref (BaseGGUUID Link1, BaseGGUUID Link2, byte) e) =>
            {
                e.Link1 = state.ReadIndexedGUID();// ?
                e.Item3 = state.Reader.ReadByte();// ?

                if (state.SaveVersion > 17)
                    e.Link2 = state.ReadIndexedGUID();// ?
            });
        }
    }
}
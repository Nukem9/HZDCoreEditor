using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8FB0349003667A31)]
    public class TileBasedStreamingStrategyInstance : StreamingStrategyInstance
    {
        public List<(GGUUID, GGUUID)> UnknownList1;
        public List<(GGUUID, GGUUID, byte)> UnknownList2;

        public override void ReadSave(SaveState state)
        {
            UnknownList1 = state.ReadVariableItemList((ref (GGUUID Link1, GGUUID Link2) e) =>
            {
                e.Link1 = state.ReadIndexedGUID();// ?
                e.Link2 = state.ReadIndexedGUID();// ?
            });

            UnknownList2 = state.ReadVariableItemList((ref (GGUUID Link1, GGUUID Link2, byte) e) =>
            {
                e.Link1 = state.ReadIndexedGUID();// ?
                e.Item3 = state.Reader.ReadByte();// ?

                if (state.SaveVersion > 17)
                    e.Link2 = state.ReadIndexedGUID();// ?
            });
        }
    }
}
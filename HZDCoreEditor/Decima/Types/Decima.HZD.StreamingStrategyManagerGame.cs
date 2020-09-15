using System.Collections.Generic;

namespace Decima.HZD
{
    // No reflection
    public class StreamingStrategyManagerGame
    {
        public List<(GGUUID, StreamingStrategyInstance)> UnknownList1;

        public void ReadSave(SaveState state)
        {
            UnknownList1 = state.ReadVariableItemList((int i, ref (GGUUID GUID, StreamingStrategyInstance Instance) e) =>
            {
                string objectType = state.ReadIndexedString();
                e.GUID = state.ReadIndexedGUID();

                // See comments in StreamingStrategyInstance
                e.Instance = RTTI.CreateObjectInstance(RTTI.GetTypeByName(objectType)) as StreamingStrategyInstance;
                e.Instance.ReadSave(state);
            });
        }
    }
}
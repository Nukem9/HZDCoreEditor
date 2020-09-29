using System.Collections.Generic;

namespace Decima.HZD
{
    // No reflection
    public class PickUpDatabaseGame
    {
        public List<(GGUUID, int, int)> UnknownList;

        public void ReadSave(SaveState state)
        {
            // pudb
            UnknownList = state.ReadVariableItemList((ref (GGUUID GUID, int, int) e) =>
            {
                e.GUID = state.ReadIndexedGUID();
                e.Item2 = state.ReadVariableLengthInt();
                e.Item3 = state.ReadVariableLengthInt();
            });
        }
    }
}
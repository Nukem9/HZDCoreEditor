using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xD1174F74B8550B27, GameType.HZD)]
    public class CollectableManager : RTTIObject
    {
        public List<BaseGGUUID> CollectableGUIDs;

        public void ReadSave(SaveState state)
        {
            // CLMB
            CollectableGUIDs = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });
            // CLME
        }
    }
}
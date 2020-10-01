using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xABE0D6FFA740D4E, GameType.HZD)]
    public class SceneManagerGame : SceneManager
    {
        public List<BaseGGUUID> UnknownGUIDs;

        public void ReadSave(SaveState state)
        {
            UnknownGUIDs = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });
        }
    }
}
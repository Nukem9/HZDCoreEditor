using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xABE0D6FFA740D4E)]
    public class SceneManagerGame : SceneManager
    {
        public List<GGUUID> UnknownGUIDs;

        public void ReadSave(SaveState state)
        {
            UnknownGUIDs = state.ReadVariableItemList((ref GGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });
        }
    }
}
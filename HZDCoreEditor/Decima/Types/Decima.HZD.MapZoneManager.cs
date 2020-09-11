using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x9BECF2413418709C)]
    public class MapZoneManager : ObjectManager
    {
        public List<GGUUID> MapGUIDs;

        public void ReadSave(SaveState state)
        {
            int guidCount = state.Reader.ReadInt32();
            MapGUIDs = new List<GGUUID>(guidCount);

            // TODO: Figure out types
            for (int i = 0; i < guidCount; i++)
                MapGUIDs.Add(state.ReadIndexedGUID());
        }
    }
}
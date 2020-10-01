using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x34C334F5B7898842, GameType.HZD)]
    public class LocationMarkerManager : ObjectManager
    {
        public List<(BaseGGUUID, byte)> UnknownList;

        public void ReadSave(SaveState state)
        {
            int counter = state.Reader.ReadInt32();
            UnknownList = new List<(BaseGGUUID, byte)>();

            for (int i = 0; i < counter; i++)
            {
                var guid = state.ReadIndexedGUID();
                byte unknown = state.Reader.ReadByte();

                UnknownList.Add((guid, unknown));
            }
        }
    }
}
using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x34C334F5B7898842)]
    public class LocationMarkerManager : ObjectManager
    {
        public void ReadSave(SaveState state)
        {
            int counter = state.Reader.ReadInt32();

            for (int i = 0; i < counter; i++)
            {
                var guid = state.ReadIndexedGUID();
                byte unknown = state.Reader.ReadByte();
            }
        }
    }
}
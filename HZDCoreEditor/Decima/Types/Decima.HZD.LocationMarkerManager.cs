using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x34C334F5B7898842)]
    public class LocationMarkerManager : ObjectManager
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            int counter = serializer.Reader.ReadInt32();

            for (int i = 0; i < counter; i++)
            {
                var guid = serializer.ReadIndexedGUID();
                byte unknown = serializer.Reader.ReadByte();
            }
        }
    }
}
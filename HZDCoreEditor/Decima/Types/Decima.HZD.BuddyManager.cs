using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x7374806C6ED90678)]
    public class BuddyManager : RTTIObject
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            int count = serializer.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                var guid = serializer.ReadIndexedGUID();
                byte unknown = serializer.Reader.ReadByte();

                var unknownData = serializer.Reader.ReadBytesStrict(24);
            }
        }
    }
}
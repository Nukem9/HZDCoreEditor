using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x7374806C6ED90678)]
    public class BuddyManager : RTTIObject
    {
        public void ReadSave(SaveState state)
        {
            int count = state.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                var guid = state.ReadIndexedGUID();
                byte unknown = state.Reader.ReadByte();

                var unknownData = state.Reader.ReadBytesStrict(24);
            }
        }
    }
}
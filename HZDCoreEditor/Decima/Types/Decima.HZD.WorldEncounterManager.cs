using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x1EF6CB2A7BC832CB)]
    public class WorldEncounterManager : ObjectManager, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(WorldEncounterManager), this);

            // WEMB
            var unknownData = state.Reader.ReadBytesStrict(24);
            int counter1 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var guid = state.ReadIndexedGUID();
            }

            int counter2 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                var guid = state.ReadIndexedGUID();
                ulong timestamp = state.Reader.ReadUInt64();// Guessed
            }

            // WEME
        }
    }
}
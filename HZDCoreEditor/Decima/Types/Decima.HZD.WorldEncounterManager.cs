using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x1EF6CB2A7BC832CB)]
    public class WorldEncounterManager : ObjectManager, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            serializer.ManuallyResolveClassMembers(typeof(WorldEncounterManager), this);

            // WEMB
            var unknownData = serializer.Reader.ReadBytesStrict(24);
            int counter1 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var guid = serializer.ReadIndexedGUID();
            }

            int counter2 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                var guid = serializer.ReadIndexedGUID();
                ulong timestamp = serializer.Reader.ReadUInt64();// Guessed
            }

            // WEME
        }
    }
}
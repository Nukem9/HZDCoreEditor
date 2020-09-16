using Utility;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x1EF6CB2A7BC832CB)]
    public class WorldEncounterManager : ObjectManager, RTTI.ISaveSerializable
    {
        public byte[] UnknownData;
        public List<GGUUID> UnknownGUIDs;
        public List<(GGUUID GUID, ulong Timestamp)> UnknownList;

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(WorldEncounterManager), this);

            // WEMB
            UnknownData = state.Reader.ReadBytesStrict(24);

            UnknownGUIDs = state.ReadVariableItemList((ref GGUUID guid) =>
            {
                guid = state.ReadIndexedGUID();
            });

            UnknownList = state.ReadVariableItemList((ref (GGUUID GUID, ulong Timestamp) e) =>
            {
                e.GUID = state.ReadIndexedGUID();
                e.Timestamp = state.Reader.ReadUInt64();// Guessed, probably last encounter time
            });
            // WEME
        }
    }
}
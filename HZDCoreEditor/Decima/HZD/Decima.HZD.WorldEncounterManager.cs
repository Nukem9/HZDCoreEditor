using System;
using Utility;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0x1EF6CB2A7BC832CB, GameType.HZD)]
    public class WorldEncounterManager : ObjectManager, RTTI.ISaveSerializable
    {
        public byte[] UnknownData;
        public List<BaseGGUUID> UnknownGUIDs;
        public List<(BaseGGUUID GUID, ulong Timestamp)> UnknownList;

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(WorldEncounterManager), this);

            // WEMB
            UnknownData = state.Reader.ReadBytesStrict(24);

            UnknownGUIDs = state.ReadVariableItemList((ref BaseGGUUID GUID) =>
            {
                GUID = state.ReadIndexedGUID();
            });

            UnknownList = state.ReadVariableItemList((ref (BaseGGUUID GUID, ulong Timestamp) e) =>
            {
                e.GUID = state.ReadIndexedGUID();
                e.Timestamp = state.Reader.ReadUInt64();// Guessed, probably last encounter time
            });
            // WEME
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
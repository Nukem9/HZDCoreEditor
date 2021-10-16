using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xC3835A4A06E1473D, GameType.HZD)]
    public class FactDatabase : RTTIObject, RTTI.ISaveSerializable
    {
        public List<(BaseGGUUID, bool)> FactLifetimes;
        public List<FactContainer> FactList;

        public class FactContainer
        {
            public BaseGGUUID GUID;
            public List<(BaseGGUUID, float)> FloatFacts;
            public List<(BaseGGUUID, int)> IntegerFacts;
            public List<(BaseGGUUID, bool)> BooleanFacts;
            public List<(BaseGGUUID, BaseGGUUID)> EnumFacts;
        }

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(FactDatabase), this);

            // FDBB - Fact DataBase Begin
            FactLifetimes = state.ReadVariableItemList((ref (BaseGGUUID GUID, bool persistent) e) =>
            {
                // Code refers to this as "lifetimes" near TelemetryCorruptedFactDatabase events
                e.GUID = state.ReadIndexedGUID();
                e.persistent = state.Reader.ReadBooleanStrict();
            });

            FactList = state.ReadVariableItemList((ref FactContainer e) =>
            {
                e.GUID = state.ReadIndexedGUID();

                // Float
                int floatCount = state.Reader.ReadInt32();
                e.FloatFacts = new List<(BaseGGUUID, float)>(floatCount);

                for (int i = 0; i < floatCount; i++)
                    e.FloatFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadSingle()));

                // Integer
                int intCount = state.Reader.ReadInt32();
                e.IntegerFacts = new List<(BaseGGUUID, int)>(intCount);

                for (int i = 0; i < intCount; i++)
                    e.IntegerFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadInt32()));

                // Boolean
                int boolCount = state.Reader.ReadInt32();
                e.BooleanFacts = new List<(BaseGGUUID, bool)>(boolCount);

                for (int i = 0; i < boolCount; i++)
                    e.BooleanFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadBooleanStrict()));

                // Enum
                int enumCount = state.Reader.ReadInt32();
                e.EnumFacts = new List<(BaseGGUUID, BaseGGUUID)>(enumCount);

                for (int i = 0; i < enumCount; i++)
                    e.EnumFacts.Add((state.ReadIndexedGUID(), state.ReadIndexedGUID()));
            });
            // FDBE
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
using BinaryStreamExtensions;
using System.Collections.Generic;

namespace Decima.HZD
{
    [RTTI.Serializable(0xC3835A4A06E1473D)]
    public class FactDatabase : RTTIObject, RTTI.ISaveSerializable
    {
        public List<(GGUUID, byte)> FactLifetimes;
        public List<FactContainer> FactList;

        public class FactContainer
        {
            public GGUUID GUID;
            public List<(GGUUID, float)> FloatFacts;
            public List<(GGUUID, int)> IntegerFacts;
            public List<(GGUUID, bool)> BooleanFacts;
            public List<(GGUUID, GGUUID)> EnumFacts;
        }

        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(FactDatabase), this);

            // FDBB - Fact DataBase Begin
            FactLifetimes = state.ReadVariableItemList((int i, ref (GGUUID, byte) e) =>
            {
                // Code refers to this as "lifetimes" near TelemetryCorruptedFactDatabase events
                e.Item1 = state.ReadIndexedGUID();
                e.Item2 = state.Reader.ReadByte();
            });

            FactList = state.ReadVariableItemList((int i, ref FactContainer e) =>
            {
                e.GUID = state.ReadIndexedGUID();

                // Float
                int floatCount = state.Reader.ReadInt32();
                e.FloatFacts = new List<(GGUUID, float)>(floatCount);

                for (int j = 0; j < floatCount; j++)
                    e.FloatFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadSingle()));

                // Integer
                int intCount = state.Reader.ReadInt32();
                e.IntegerFacts = new List<(GGUUID, int)>(intCount);

                for (int j = 0; j < intCount; j++)
                    e.IntegerFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadInt32()));

                // Boolean
                int boolCount = state.Reader.ReadInt32();
                e.BooleanFacts = new List<(GGUUID, bool)>(boolCount);

                for (int j = 0; j < boolCount; j++)
                    e.BooleanFacts.Add((state.ReadIndexedGUID(), state.Reader.ReadBooleanStrict()));

                // Enum
                int enumCount = state.Reader.ReadInt32();
                e.EnumFacts = new List<(GGUUID, GGUUID)>(enumCount);

                for (int j = 0; j < enumCount; j++)
                    e.EnumFacts.Add((state.ReadIndexedGUID(), state.ReadIndexedGUID()));
            });
            // FDBE
        }
    }
}
namespace Decima.HZD
{
    [RTTI.Serializable(0xC3835A4A06E1473D)]
    public class FactDatabase : RTTIObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(FactDatabase), this);

            // FDBB - Fact DataBase Begin?
            int counter1 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var GUID = state.ReadIndexedGUID();
                byte unknown = state.Reader.ReadByte();
            }

            int counter2 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                var GUID = state.ReadIndexedGUID();

                // Float, integer, boolean, enum?
                int unknown = state.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = state.ReadIndexedGUID();
                    int anotherUnknown = state.Reader.ReadInt32();
                }

                unknown = state.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = state.ReadIndexedGUID();
                    int anotherUnknown = state.Reader.ReadInt32();
                }

                unknown = state.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = state.ReadIndexedGUID();
                    byte anotherUnknown = state.Reader.ReadByte();
                }

                unknown = state.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = state.ReadIndexedGUID();
                    var anotherGUID2 = state.ReadIndexedGUID();
                }
            }

            // FDBE
        }
    }
}
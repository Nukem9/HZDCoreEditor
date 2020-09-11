namespace Decima.HZD
{
    [RTTI.Serializable(0xEB18FAD2ABAD4F96)]
    public class MenuBadgeManager : StateObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(MenuBadgeManager), this);

            // MBMB
            int counter1 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var guid = state.ReadIndexedGUID();
            }

            int counter2 = state.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                int counter3 = state.ReadVariableLengthOffset();

                for (int j = 0; j < counter3; j++)
                {
                    var guid = state.ReadIndexedGUID();
                }
            }

            // MBME
        }
    }
}
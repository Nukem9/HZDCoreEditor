namespace Decima.HZD
{
    // No reflection
    public class PickUpDatabaseGame
    {
        public void ReadSave(SaveState state)
        {
            // pudb
            int count = state.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                var unknownGUID = state.ReadIndexedGUID();
                int unknown1 = state.ReadVariableLengthInt();
                int unknown2 = state.ReadVariableLengthInt();
            }
        }
    }
}
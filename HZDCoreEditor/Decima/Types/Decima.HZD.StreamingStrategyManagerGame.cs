namespace Decima.HZD
{
    // No reflection
    public class StreamingStrategyManagerGame
    {
        public void ReadSave(SaveState state)
        {
            int count = state.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                string objectType = state.ReadIndexedString();
                var guid = state.ReadIndexedGUID();

                // See comments in StreamingStrategyInstance
                var obj = RTTI.CreateObjectInstance(RTTI.GetTypeByName(objectType)) as StreamingStrategyInstance;

                obj.ReadSave(state);
            }
        }
    }
}
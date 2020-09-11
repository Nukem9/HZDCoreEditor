namespace Decima.HZD
{
    [RTTI.Serializable(0xE6A4D10BC69EFF20)]
    public class CountdownTimerManager : RTTIObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(CountdownTimerManager), this);

            int count = state.ReadVariableLengthOffset();

            for (int i = 0; i < count; i++)
            {
                var guid = state.ReadIndexedGUID();
                var timerSaveObject = state.ReadObjectHandle();
            }
        }
    }
}
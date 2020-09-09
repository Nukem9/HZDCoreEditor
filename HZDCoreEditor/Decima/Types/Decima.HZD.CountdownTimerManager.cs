namespace Decima.HZD
{
    [RTTI.Serializable(0xE6A4D10BC69EFF20)]
    public class CountdownTimerManager : RTTIObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            serializer.ManuallyResolveClassMembers(typeof(CountdownTimerManager), this);

            int count = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < count; i++)
            {
                var guid = serializer.ReadIndexedGUID();
                var timerSaveObject = serializer.ReadObjectHandle();
            }
        }
    }
}
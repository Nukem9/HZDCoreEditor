namespace Decima.HZD
{
    // No reflection
    public class PickUpDatabaseGame
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            // pudb
            int count = serializer.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                var unknownGUID = serializer.ReadIndexedGUID();
                int unknown1 = serializer.ReadVariableLengthInt();
                int unknown2 = serializer.ReadVariableLengthInt();
            }
        }
    }
}
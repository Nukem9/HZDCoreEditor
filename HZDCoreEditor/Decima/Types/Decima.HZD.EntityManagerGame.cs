namespace Decima.HZD
{
    // No reflection
    public class EntityManagerGame
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            ulong timestamp = serializer.Reader.ReadUInt64();
        }
    }
}
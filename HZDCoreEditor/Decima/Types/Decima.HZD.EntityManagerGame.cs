namespace Decima.HZD
{
    // No reflection
    public class EntityManagerGame
    {
        ulong Timestamp;

        public void ReadSave(SaveState state)
        {
            Timestamp = state.Reader.ReadUInt64();
        }
    }
}
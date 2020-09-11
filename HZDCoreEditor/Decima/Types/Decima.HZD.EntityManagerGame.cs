namespace Decima.HZD
{
    // No reflection
    public class EntityManagerGame
    {
        public void ReadSave(SaveState state)
        {
            ulong timestamp = state.Reader.ReadUInt64();
        }
    }
}
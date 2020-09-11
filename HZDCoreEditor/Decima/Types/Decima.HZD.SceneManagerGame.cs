namespace Decima.HZD
{
    [RTTI.Serializable(0xABE0D6FFA740D4E)]
    public class SceneManagerGame : SceneManager
    {
        public void ReadSave(SaveState state)
        {
            int count = state.ReadVariableLengthInt();

            for (int i = 0; i < count; i++)
            {
                var guid = state.ReadIndexedGUID();
            }
        }
    }
}
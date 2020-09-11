namespace Decima.HZD
{
    [RTTI.Serializable(0xD1174F74B8550B27)]
    public class CollectableManager : RTTIObject
    {
        public void ReadSave(SaveState state)
        {
            // CLMB
            int counter = state.ReadVariableLengthInt();

            for (int i = 0; i < counter; i++)
            {
                var guid = state.ReadIndexedGUID();
            }

            // CLME
        }
    }
}
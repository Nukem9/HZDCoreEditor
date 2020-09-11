namespace Decima.HZD
{
    [RTTI.Serializable(0x8FB0349003667A31)]
    public class TileBasedStreamingStrategyInstance : StreamingStrategyInstance
    {
        public override void ReadSave(SaveState state)
        {
            int count1 = state.ReadVariableLengthInt();

            for (int i = 0; i < count1; i++)
            {
                var guidLink1 = state.ReadIndexedGUID();
                var guidLink2 = state.ReadIndexedGUID();
            }

            int count2 = state.ReadVariableLengthInt();

            for (int i = 0; i < count2; i++)
            {
                var guidLink1 = state.ReadIndexedGUID();
                byte unknown = state.Reader.ReadByte();

                if (state.SaveVersion > 17)
                {
                    var guidLink2 = state.ReadIndexedGUID();
                }
            }
        }
    }
}
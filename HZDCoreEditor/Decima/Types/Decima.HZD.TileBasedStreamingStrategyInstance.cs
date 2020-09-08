namespace Decima.HZD
{
    [RTTI.Serializable(0x8FB0349003667A31)]
    public class TileBasedStreamingStrategyInstance : StreamingStrategyInstance
    {
        public override void DeserializeStateObject(SaveDataSerializer serializer)
        {
            int count1 = serializer.ReadVariableLengthInt();

            for (int i = 0; i < count1; i++)
            {
                var guidLink1 = serializer.ReadIndexedGUID();
                var guidLink2 = serializer.ReadIndexedGUID();
            }

            int count2 = serializer.ReadVariableLengthInt();

            for (int i = 0; i < count2; i++)
            {
                var guidLink1 = serializer.ReadIndexedGUID();
                byte unknown = serializer.Reader.ReadByte();

                if (serializer.FileDataVersion > 17)
                {
                    var guidLink2 = serializer.ReadIndexedGUID();
                }
            }
        }
    }
}
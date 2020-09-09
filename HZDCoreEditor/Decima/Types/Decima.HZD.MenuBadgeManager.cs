namespace Decima.HZD
{
    [RTTI.Serializable(0xEB18FAD2ABAD4F96)]
    public class MenuBadgeManager : StateObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            serializer.ManuallyResolveClassMembers(typeof(MenuBadgeManager), this);

            // MBMB
            int counter1 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var guid = serializer.ReadIndexedGUID();
            }

            int counter2 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                int counter3 = serializer.ReadVariableLengthOffset();

                for (int j = 0; j < counter3; j++)
                {
                    var guid = serializer.ReadIndexedGUID();
                }
            }

            // MBME
        }
    }
}
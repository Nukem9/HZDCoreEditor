using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0x70DDECB5743C9A59)]
    public class Story : RTTIObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            serializer.ManuallyResolveClassMembers(typeof(Story), this);

            int questSaveCount = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < questSaveCount; i++)
            {
                var questSaveObject = serializer.ReadObjectHandle();
                int counter1 = serializer.ReadVariableLengthOffset();

                for (int j = 0; j < counter1; j++)
                {
                    var guid1 = serializer.ReadIndexedGUID();
                    int unknown = serializer.ReadVariableLengthInt();

                    if (unknown > 3)
                        throw new Exception();

                    var guid2 = serializer.ReadIndexedGUID();
                    int unknown2 = serializer.Reader.ReadInt32();
                }

                int counter2 = serializer.ReadVariableLengthOffset();

                for (int j = 0; j < counter2; j++)
                {
                    var guid1 = serializer.ReadIndexedGUID();
                    var questObjectiveSave = serializer.ReadObjectHandle();
                }
            }
        }
    }
}
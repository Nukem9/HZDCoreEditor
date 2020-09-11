using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0x70DDECB5743C9A59)]
    public class Story : RTTIObject, RTTI.ISaveSerializable
    {
        public void DeserializeStateObject(SaveState state)
        {
            state.DeserializeObjectClassMembers(typeof(Story), this);

            int questSaveCount = state.ReadVariableLengthOffset();

            for (int i = 0; i < questSaveCount; i++)
            {
                var questSaveObject = state.ReadObjectHandle();
                int counter1 = state.ReadVariableLengthOffset();

                for (int j = 0; j < counter1; j++)
                {
                    var guid1 = state.ReadIndexedGUID();
                    int unknown = state.ReadVariableLengthInt();

                    if (unknown > 3)
                        throw new Exception();

                    var guid2 = state.ReadIndexedGUID();
                    int unknown2 = state.Reader.ReadInt32();
                }

                int counter2 = state.ReadVariableLengthOffset();

                for (int j = 0; j < counter2; j++)
                {
                    var guid1 = state.ReadIndexedGUID();
                    var questObjectiveSave = state.ReadObjectHandle();
                }
            }
        }
    }
}
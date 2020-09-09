using System;
using System.Collections.Generic;
using System.Text;

namespace Decima.HZD
{
    [RTTI.Serializable(0xC3835A4A06E1473D)]
    public class FactDatabase : RTTIObject, RTTI.ISaveExtraBinaryDataCallback
    {
        public void DeserializeStateObjectExtraData(SaveDataSerializer serializer)
        {
            // FDBB - Fact DataBase Begin?
            int counter1 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter1; i++)
            {
                var GUID = serializer.ReadIndexedGUID();
                byte unknown = serializer.Reader.ReadByte();
            }

            int counter2 = serializer.ReadVariableLengthOffset();

            for (int i = 0; i < counter2; i++)
            {
                var GUID = serializer.ReadIndexedGUID();

                // Float, integer, boolean, enum?
                int unknown = serializer.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = serializer.ReadIndexedGUID();
                    int anotherUnknown = serializer.Reader.ReadInt32();
                }

                unknown = serializer.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = serializer.ReadIndexedGUID();
                    int anotherUnknown = serializer.Reader.ReadInt32();
                }

                unknown = serializer.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = serializer.ReadIndexedGUID();
                    byte anotherUnknown = serializer.Reader.ReadByte();
                }

                unknown = serializer.Reader.ReadInt32();
                for (int j = 0; j < unknown; j++)
                {
                    var anotherGUID = serializer.ReadIndexedGUID();
                    var anotherGUID2 = serializer.ReadIndexedGUID();
                }
            }

            // FDBE
        }
    }
}
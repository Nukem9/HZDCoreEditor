using System;
using System.Collections.Generic;
using System.Text;

namespace Decima.HZD
{
    [RTTI.Serializable(0x7BE3172218BB86EF)]
    public class StreamingStrategyInstance : RTTIRefObject, RTTI.ISaveSerializable
    {
        public virtual void DeserializeStateObject(SaveDataSerializer serializer)
        {
            // Technically this isn't a callback in the game code, it's part of the virtual table. Need
            // to confirm if this is unique to StreamingStrategyInstance.
        }
    }
}
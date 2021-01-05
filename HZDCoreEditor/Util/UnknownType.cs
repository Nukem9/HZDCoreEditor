using System;
using System.Collections.Generic;
using System.Text;

namespace HZDCoreEditor.Util
{
    public class UnknownType
    {
        public ulong TypeId { get; }
        public byte[] Data { get; }

        public UnknownType(byte[] data, ulong typeId)
        {
            Data = data;
            TypeId = typeId;
        }

    }
}

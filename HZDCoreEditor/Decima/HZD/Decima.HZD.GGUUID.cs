﻿using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0x211FDC8FD3395464, GameType.HZD)]
    public class GGUUID : BaseGGUUID
    {
        public GGUUID()
        {
        }

        public GGUUID(BaseGGUUID value) : base(value)
        {
        }

        public static implicit operator GGUUID(string value)
        {
            return new GGUUID(FromString(value));
        }

        public static implicit operator GGUUID(Guid value)
        {
            return new GGUUID(FromData(value.ToByteArray()));
        }
    }
}
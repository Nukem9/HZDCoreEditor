using System;
using System.IO;

namespace Decima
{
    public abstract class BaseStreamHandle
    {
        public string ResourcePath;

        public abstract void ToData(BinaryWriter writer);

        public static BaseStreamHandle FromData(BinaryReader reader, GameType gameType)
        {
            switch (gameType)
            {
                case GameType.DS:
                    return DS.StreamHandle.FromData(reader);

                case GameType.HZD:
                    return HZD.StreamHandle.FromData(reader);
            }

            throw new NotImplementedException();
        }
    }
}
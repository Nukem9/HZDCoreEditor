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
            return gameType switch
            {
                GameType.DS => DS.StreamHandle.FromData(reader),
                GameType.HZD => HZD.StreamHandle.FromData(reader),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
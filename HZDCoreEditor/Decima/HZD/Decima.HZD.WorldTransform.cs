using HZDCoreEditor.Util;
using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0x23C32AD4512B105E, GameType.HZD)]
    public class WorldTransform : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public WorldPosition Position;
        [RTTI.Member(1, 0x18)] public RotMatrix Orientation;

        [Flags]
        private enum PackFlags : byte
        {
            ZeroAll = 1,        // Default to zeroed/identity matrix
            Truncated = 2,      // Certain components are optional
            SkipOrientation = 4,
            SkipPosition = 8,
        }

        public void DeserializeStateObject(SaveState state)
        {
            var flags = (PackFlags)state.Reader.ReadByte();

            // position = { 0, 0, 0 }
            // orientation = { identity matrix }
            Position = new WorldPosition();
            Orientation = new RotMatrix();

            if ((flags & PackFlags.ZeroAll) != 0)
                return;

            if ((flags & PackFlags.Truncated) == 0)
            {
                // Read/write directly to memory
                Position.DeserializeStateObject(state);
                Orientation.DeserializeStateObject(state);
            }
            else
            {
                if ((flags & PackFlags.SkipOrientation) == 0)
                {
                    var rot = state.Reader.ReadBytesStrict(16);

                    //throw new NotImplementedException("TODO: Need to figure out how data is packed");
                }

                if ((flags & PackFlags.SkipPosition) == 0)
                    Position.DeserializeStateObject(state);
            }
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
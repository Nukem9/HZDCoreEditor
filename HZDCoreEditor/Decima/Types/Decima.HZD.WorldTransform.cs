using BinaryStreamExtensions;
using System;

namespace Decima.HZD
{
    [RTTI.Serializable(0x23C32AD4512B105E)]
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

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            var flags = (PackFlags)serializer.Reader.ReadByte();

            // position = { 0, 0, 0 }
            // orientation = { identity matrix }
            if ((flags & PackFlags.ZeroAll) != 0)
                return;

            if ((flags & PackFlags.Truncated) == 0)
            {
                // Read/write directly to memory
                var pos = serializer.Reader.ReadBytesStrict(24);
                var rot = serializer.Reader.ReadBytesStrict(36);
            }
            else
            {
                if ((flags & PackFlags.SkipOrientation) == 0)
                {
                    var rot = serializer.Reader.ReadBytesStrict(16);
                }

                if ((flags & PackFlags.SkipPosition) == 0)
                {
                    var pos = serializer.Reader.ReadBytesStrict(24);
                }
            }
        }
    }
}
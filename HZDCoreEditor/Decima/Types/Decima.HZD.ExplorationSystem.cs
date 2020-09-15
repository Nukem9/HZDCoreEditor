using BinaryStreamExtensions;

namespace Decima.HZD
{
    // No reflection
    public class ExplorationSystem
    {
        public byte[] OverlayBitmapData;
        public byte[] UnknownData;
        public GGUUID UnknownGUID;

        public void ReadSave(SaveState state)
        {
            if (state.SaveVersion < 26)
            {
                int count = state.ReadVariableLengthOffset();

                OverlayBitmapData = state.Reader.ReadBytesStrict(count);
            }
            else
            {
                OverlayBitmapData = new byte[65536];

                for (int offset = 0; offset < OverlayBitmapData.Length;)
                {
                    int size = state.ReadVariableLengthInt();
                    byte visibility = state.Reader.ReadByte();

                    for (int i = offset; i < (offset + size); i++)
                        OverlayBitmapData[i] = visibility;

                    offset += size;
                }
            }

            bool unknown = state.Reader.ReadBooleanStrict();

            if (unknown)
            {
                UnknownData = state.Reader.ReadBytesStrict(24);
                UnknownGUID = state.ReadIndexedGUID();
            }
        }
    }
}
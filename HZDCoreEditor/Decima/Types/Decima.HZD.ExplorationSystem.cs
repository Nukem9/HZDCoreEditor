using BinaryStreamExtensions;

namespace Decima.HZD
{
    // No reflection
    public class ExplorationSystem
    {
        public void ReadSave(SaveState state)
        {
            if (state.SaveVersion < 26)
            {
                int count = state.ReadVariableLengthOffset();

                var overlayBitmapData = state.Reader.ReadBytesStrict(count);
            }
            else
            {
                var overlayBitmapData = new byte[65536];

                for (int offset = 0; offset < overlayBitmapData.Length;)
                {
                    int size = state.ReadVariableLengthInt();
                    byte visibility = state.Reader.ReadByte();

                    for (int i = offset; i < (offset + size); i++)
                        overlayBitmapData[i] = visibility;

                    offset += size;
                }
            }

            bool unknown = state.Reader.ReadBooleanStrict();

            if (unknown)
            {
                var data = state.Reader.ReadBytesStrict(24);
                var guid = state.ReadIndexedGUID();
            }
        }
    }
}
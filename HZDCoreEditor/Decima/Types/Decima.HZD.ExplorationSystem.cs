using BinaryStreamExtensions;

namespace Decima.HZD
{
    // No reflection
    public class ExplorationSystem
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            if (serializer.FileDataVersion < 26)
            {
                int count = serializer.ReadVariableLengthOffset();

                var overlayBitmapData = serializer.Reader.ReadBytesStrict(count);
            }
            else
            {
                var overlayBitmapData = new byte[65536];

                for (int offset = 0; offset < 65536;)
                {
                    int size = serializer.ReadVariableLengthInt();
                    byte visible = serializer.Reader.ReadByte();

                    for (int i = offset; i < (offset + size); i++)
                        overlayBitmapData[i] = visible;

                    offset += size;
                }
            }

            bool unknown = serializer.Reader.ReadBooleanStrict();

            if (unknown)
            {
                var data = serializer.Reader.ReadBytesStrict(24);
                var guid = serializer.ReadIndexedGUID();
            }
        }
    }
}
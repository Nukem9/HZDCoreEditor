using BinaryStreamExtensions;
using System.IO;
using System.Text;

namespace Decima.HZD
{
    public class StreamHandle
    {
        public static StreamHandle FromData(BinaryReader reader)
        {
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
            {
                // Stream resource file path
                var str = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));
            }

            // Likely file offsets or length
            var unknown1 = reader.ReadUInt64();
            var unknown2 = reader.ReadUInt64();

            return new StreamHandle();
        }
    }
}
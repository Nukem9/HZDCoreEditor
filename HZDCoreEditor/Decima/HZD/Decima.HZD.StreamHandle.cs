using HZDCoreEditor.Util;
using System.IO;
using System.Text;

namespace Decima.HZD
{
    public class StreamHandle : BaseStreamHandle
    {
        public ulong Unknown1;
        public ulong Unknown2;

        public override void ToData(BinaryWriter writer)
        {
            writer.Write((uint)ResourcePath.Length);

            if (ResourcePath.Length > 0)
                writer.Write(Encoding.UTF8.GetBytes(ResourcePath));

            writer.Write(Unknown1);
            writer.Write(Unknown2);
        }

        public static StreamHandle FromData(BinaryReader reader)
        {
            var x = new StreamHandle();
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                x.ResourcePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));

            x.Unknown1 = reader.ReadUInt64();
            x.Unknown2 = reader.ReadUInt64();

            return x;
        }
    }
}
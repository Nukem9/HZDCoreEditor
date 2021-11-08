using HZDCoreEditor.Util;
using System.IO;
using System.Text;

namespace Decima.HZD
{
    public class StreamHandle : BaseStreamHandle
    {
        public ulong ResourceOffset;
        public ulong ResourceLength;

        public override void ToData(BinaryWriter writer)
        {
            writer.Write((uint)ResourcePath.Length);

            if (ResourcePath.Length > 0)
                writer.Write(Encoding.UTF8.GetBytes(ResourcePath));

            writer.Write(ResourceOffset);
            writer.Write(ResourceLength);
        }

        public static StreamHandle FromData(BinaryReader reader)
        {
            var handle = new StreamHandle();
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                handle.ResourcePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));

            handle.ResourceOffset = reader.ReadUInt64();
            handle.ResourceLength = reader.ReadUInt64();

            return handle;
        }

        public override uint ResourceSize()
        {
            return (uint)ResourceLength;
        }
    }
}
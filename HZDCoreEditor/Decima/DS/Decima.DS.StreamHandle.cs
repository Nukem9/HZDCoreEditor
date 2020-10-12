using System.IO;
using System.Text;
using Utility;

namespace Decima.DS
{
    public class StreamHandle : BaseStreamHandle
    {
        public BaseGGUUID ResourceUUID;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        public override void ToData(BinaryWriter writer)
        {
            writer.Write((uint)ResourcePath.Length);

            if (ResourcePath.Length > 0)
                writer.Write(Encoding.UTF8.GetBytes(ResourcePath));

            ResourceUUID.ToData(writer);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
        }

        public static StreamHandle FromData(BinaryReader reader)
        {
            var x = new StreamHandle();
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                x.ResourcePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));

            x.ResourceUUID = new BaseGGUUID().FromData(reader);
            x.Unknown1 = reader.ReadUInt32();
            x.Unknown2 = reader.ReadUInt32();
            x.Unknown3 = reader.ReadUInt32();

            return x;
        }
    }
}
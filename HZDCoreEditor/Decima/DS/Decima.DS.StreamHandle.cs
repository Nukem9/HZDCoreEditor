using HZDCoreEditor.Util;
using System;
using System.IO;
using System.Text;

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
            var handle = new StreamHandle();
            uint stringLength = reader.ReadUInt32();

            if (stringLength > 0)
                handle.ResourcePath = Encoding.UTF8.GetString(reader.ReadBytesStrict(stringLength));

            handle.ResourceUUID = BaseGGUUID.FromData(reader);
            handle.Unknown1 = reader.ReadUInt32();
            handle.Unknown2 = reader.ReadUInt32();
            handle.Unknown3 = reader.ReadUInt32();

            return handle;
        }

        public override uint ResourceSize()
        {
            throw new NotImplementedException("Fields haven't been decoded yet");
        }
    }
}
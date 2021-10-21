using HZDCoreEditor.Util;
using System.Collections.Generic;
using System.IO;

namespace Decima
{
    public class HwVertexArray
    {
        public uint VertexCount;    // TODO: Determine from HwBuffer entries (what happens if there's none?)
        public List<VertexStream> Streams;
        public bool IsStreaming;    // TODO: Determine from HwBuffer entries (what happens if there's none?)

        public class VertexStream
        {
            public uint Flags;
            public List<VertexElementDesc> ElementInfo;
            public BaseGGUUID GUID;
            public HwBuffer Buffer;
        }

        public class VertexElementDesc
        {
            public byte ByteOffset;                         // Ex: 0
            public BaseVertexElementStorageType StorageType;// Ex: SignedShortNormalized
            public byte ComponentCount;                     // Ex: 3 (Pos -> X, Y, Z)
            public BaseVertexElement ElementType;           // Ex: EVertexElement.Pos
        }

        public void ToData(BinaryWriter writer)
        {
            writer.Write(VertexCount);
            writer.Write((uint)Streams.Count);
            writer.Write(IsStreaming);

            foreach (var stream in Streams)
            {
                writer.Write(stream.Flags);
                writer.Write(stream.Buffer.ElementStride);
                writer.Write((uint)stream.ElementInfo.Count);

                foreach (var desc in stream.ElementInfo)
                {
                    writer.Write((byte)desc.ByteOffset);
                    writer.Write((byte)desc.StorageType);
                    writer.Write((byte)desc.ComponentCount);
                    writer.Write((byte)desc.ElementType);
                }

                stream.GUID.ToData(writer);
                stream.Buffer.ToData(writer);
            }
        }

        public static HwVertexArray FromData(BinaryReader reader, GameType gameType)
        {
            var array = new HwVertexArray();

            array.VertexCount = reader.ReadUInt32();
            uint streamCount = reader.ReadUInt32();
            array.IsStreaming = reader.ReadBooleanStrict();

            array.Streams = new List<VertexStream>((int)streamCount);

            for (uint i = 0; i < streamCount; i++)
            {
                uint flags = reader.ReadUInt32();
                uint byteStride = reader.ReadUInt32();
                uint elementDescCount = reader.ReadUInt32();

                var stream = new VertexStream();
                stream.Flags = flags;
                stream.ElementInfo = new List<VertexElementDesc>((int)elementDescCount);

                for (uint j = 0; j < elementDescCount; j++)
                {
                    stream.ElementInfo.Add(new VertexElementDesc()
                    {
                        ByteOffset = reader.ReadByte(),
                        StorageType = (BaseVertexElementStorageType)reader.ReadByte(),
                        ComponentCount = reader.ReadByte(),
                        ElementType = (BaseVertexElement)reader.ReadByte(),
                    });
                }

                stream.GUID = BaseGGUUID.FromData(reader);
                stream.Buffer = HwBuffer.FromVertexData(reader, gameType, array.IsStreaming, byteStride, array.VertexCount);

                array.Streams.Add(stream);
            }

            return array;
        }
    }
}
using System.IO;
using Utility;

namespace Decima.DS
{
    [RTTI.Serializable(0xE9505B4C80665853, GameType.DS)]
    public class Pose : RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        [RTTI.Member(0, 0x20)] public Ref<Skeleton> Skeleton;
        public byte[] UnknownData1;
        public byte[] UnknownData2;
        public byte[] UnknownData3;

        public void DeserializeExtraData(BinaryReader reader)
        {
            bool hasExtraData = reader.ReadBooleanStrict();

            if (hasExtraData)
            {
                uint count = reader.ReadUInt32();
                UnknownData1 = reader.ReadBytesStrict(count * 48);
                UnknownData2 = reader.ReadBytesStrict(count * 64);

                count = reader.ReadUInt32();
                UnknownData3 = reader.ReadBytesStrict(count * 4);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            bool hasExtraData = UnknownData1 != null || UnknownData2 != null || UnknownData3 != null;

            writer.Write(hasExtraData);

            if (hasExtraData)
            {
                writer.Write((uint)UnknownData1.Length / 48);
                writer.Write(UnknownData1);
                writer.Write(UnknownData2);

                writer.Write((uint)UnknownData3.Length / 4);
                writer.Write(UnknownData3);
            }
        }
    }
}
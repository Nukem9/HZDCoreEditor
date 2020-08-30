using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0xE9505B4C80665853)]
    public class Pose : RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(0, 0x20)] public Ref<Skeleton> Skeleton;

        public void DeserializeExtraData(BinaryReader reader)
        {
            bool hasExtraData = reader.ReadBooleanStrict();

            if (hasExtraData)
            {
                uint count = reader.ReadUInt32();

                if (count > 0)
                {
                    var unknownData1 = reader.ReadBytesStrict(count * 48);
                    var unknownData2 = reader.ReadBytesStrict(count * 64);
                }

                count = reader.ReadUInt32();

                if (count > 0)
                {
                    var unknownData3 = reader.ReadBytesStrict(count * 4);
                }
            }
        }
    }
}

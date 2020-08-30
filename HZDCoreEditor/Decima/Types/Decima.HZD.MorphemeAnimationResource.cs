using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x985D2CD65C07569F)]
    public class MorphemeAnimationResource : ResourceWithoutLegacyName, RTTI.IExtraBinaryDataCallback
    {
        public byte[] MorphemeData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint morphemeDataLength = reader.ReadUInt32();

            if (morphemeDataLength > 0)
                MorphemeData = reader.ReadBytesStrict(morphemeDataLength);
        }
    }
}

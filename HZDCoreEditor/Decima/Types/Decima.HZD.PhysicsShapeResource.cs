using BinaryStreamExtensions;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8E897AA8B1F6FC19)]
    public class PhysicsShapeResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
        public byte[] HavokData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            // Generic havok reader
            uint havokDataLength = reader.ReadUInt32();

            if (havokDataLength > 0)
            {
                // Havok 2014 HKX file ("hk_2014.2.0-r1")
                HavokData = reader.ReadBytesStrict(havokDataLength);
            }
        }
    }
}

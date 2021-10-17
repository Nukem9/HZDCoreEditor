using HZDCoreEditor.Util;
using System.IO;

namespace Decima.DS
{
    [RTTI.Serializable(0x4BA680E05E04D0EE, GameType.DS)]
    public class PhysicsShapeResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        // Identical impl to HZD
        public byte[] HavokData;

        public void DeserializeExtraData(BinaryReader reader)
        {
            // Generic havok reader / Havok 2014 HKX file ("hk_2014.2.0-r1")
            uint havokDataLength = reader.ReadUInt32();
            HavokData = reader.ReadBytesStrict(havokDataLength);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)HavokData.Length);
            writer.Write(HavokData);
        }
    }
}
using HZDCoreEditor.Util;
using System.IO;

namespace Decima.HZD
{
    [RTTI.Serializable(0x8E897AA8B1F6FC19, GameType.HZD)]
    public class PhysicsShapeResource : BaseResource, RTTI.IExtraBinaryDataCallback
    {
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

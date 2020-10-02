using System.Collections.Generic;
using System.IO;

namespace Decima.DS
{
    using uint8 = System.Byte;
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x150C273BEB8F2D0C, GameType.DS)]
    public class WwiseBankResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(3, 0x20, "Data")] public uint32 BankID;
        [RTTI.Member(4, 0x24, "Data")] public uint32 BankSize;
        [RTTI.Member(5, 0x28, "Data")] public Array<uint8> BankData;
        [RTTI.Member(6, 0x38, "Data")] public Array<uint32> WemIDs;
        [RTTI.Member(7, 0x48, "Data", true)] public Array<Ref<WwiseWemResource>> Wems;
        [RTTI.Member(8, 0x60, "Data")] public bool IsExperimental;
        public List<WwiseWemResource> WemResources;// TODO: Game code inserts these into 'Wems' as references

        public void DeserializeExtraData(BinaryReader reader)
        {
            WemResources = new List<WwiseWemResource>(WemIDs.Count);

            for (int i = 0; i < WemIDs.Count; i++)
            {
                var x = new WwiseWemResource();

                x.IsStreaming = true;
                x.DeserializeExtraData(reader);

                WemResources.Add(x);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            foreach (var wemResource in WemResources)
                wemResource.SerializeExtraData(writer);
        }
    }
}
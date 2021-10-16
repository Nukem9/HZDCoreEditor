using HZDCoreEditor.Util;
using System.IO;

namespace Decima.HZD
{
    using uint32 = System.UInt32;

    [RTTI.Serializable(0x873D69111E6F7262, GameType.HZD)]
    public class MusicResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        [RTTI.Member(9, 0x48)] public Array<uint32> StreamingDataHash;
        [RTTI.Member(4, 0x58)] public int BitRate;
        [RTTI.Member(5, 0x5C)] public bool StripSilence;
        [RTTI.Member(6, 0x60)] public int StripSilenceThreshold;
        [RTTI.Member(7, 0x68)] public Array<MusicSubmixBinding> SubmixBindings;
        [RTTI.Member(8, 0x78)] public Array<String> StreamingBankNames;
        public byte[] MusicData;
        public StreamHandle[] StreamInfo;

        public void DeserializeExtraData(BinaryReader reader)
        {
            uint dataLength = reader.ReadUInt32();

            if (dataLength > 0)
                MusicData = reader.ReadBytesStrict(dataLength);

            StreamInfo = new StreamHandle[StreamingBankNames.Count];

            for (uint i = 0; i < StreamInfo.Length; i++)
                StreamInfo[i] = StreamHandle.FromData(reader);
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            writer.Write((uint)MusicData.Length);

            if (MusicData.Length > 0)
                writer.Write(MusicData);

            for (uint i = 0; i < StreamInfo.Length; i++)
                StreamInfo[i].ToData(writer);
        }
    }
}

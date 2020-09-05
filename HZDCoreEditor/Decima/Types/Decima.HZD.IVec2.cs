namespace Decima.HZD
{
    [RTTI.Serializable(0xD010735A1623DE72)]
    public class IVec2 : RTTI.ISaveSerializable
    {
        [RTTI.Member(0, 0x0)] public int X;
        [RTTI.Member(1, 0x4)] public int Y;

        public void DeserializeStateObject(SaveDataSerializer serializer)
        {
            X = serializer.Reader.ReadInt32();
            Y = serializer.Reader.ReadInt32();
        }
    }
}

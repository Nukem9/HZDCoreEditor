namespace HZDCoreEditor.Util
{
    public class UnknownType
    {
        public ulong TypeId { get; }
        public byte[] Data { get; }

        public UnknownType(ulong typeId, byte[] data)
        {
            TypeId = typeId;
            Data = data;
        }

    }
}

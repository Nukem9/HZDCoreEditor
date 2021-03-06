namespace Decima.DS
{
    [RTTI.Serializable(0x211FDC8FD3395464, GameType.DS)]
    public class GGUUID : BaseGGUUID
    {
        public GGUUID()
        {
        }

        public GGUUID(BaseGGUUID value)
        {
            AssignFromOther(value);
        }

        public static implicit operator GGUUID(string value)
        {
            return (GGUUID)(new GGUUID().FromString(value));
        }
    }
}
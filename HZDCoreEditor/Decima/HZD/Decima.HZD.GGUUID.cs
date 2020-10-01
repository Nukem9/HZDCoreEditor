namespace Decima.HZD
{
    [RTTI.Serializable(0x211FDC8FD3395464, GameType.HZD)]
    public class GGUUID : BaseGGUUID
    {
        public GGUUID()
        {
        }

        public GGUUID(BaseGGUUID GUID)
        {
            AssignFromOther(GUID);
        }
    }
}
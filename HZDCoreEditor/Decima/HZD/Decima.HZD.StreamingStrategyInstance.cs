namespace Decima.HZD
{
    [RTTI.Serializable(0x7BE3172218BB86EF, GameType.HZD)]
    public class StreamingStrategyInstance : RTTIRefObject
    {
        public virtual void ReadSave(SaveState state)
        {
            // Technically this isn't a callback in the game code, it's part of the virtual table. Need
            // to confirm if this is unique to StreamingStrategyInstance.
        }
    }
}
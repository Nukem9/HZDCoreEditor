using Utility;

namespace Decima.HZD
{
    [RTTI.Serializable(0x96C62803CC5314B, GameType.HZD)]
    public class PlayerGame : Player
    {
        public string Name;
        public object UnknownObject;
        public PlayerRestoreState RestoreState;

        public void ReadSave(SaveState state)
        {
            // This is part of Player::VFunc25 but I'm too lazy to add it
            Name = state.ReadIndexedWideString();
            UnknownObject = state.ReadObjectHandle();
            // end

            bool hasRestoreState = state.Reader.ReadBooleanStrict();

            if (hasRestoreState)
                RestoreState = state.DeserializeType<PlayerRestoreState>();
        }
    }
}
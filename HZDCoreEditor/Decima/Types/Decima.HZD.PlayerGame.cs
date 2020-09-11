using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x96C62803CC5314B)]
    public class PlayerGame : Player
    {
        string Name;
        object UnknownObject;
        PlayerRestoreState RestoreState;

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
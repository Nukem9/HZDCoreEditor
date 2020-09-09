using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x96C62803CC5314B)]
    public class PlayerGame : Player
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            // This is part of Player::VFunc25 but I'm too lazy to add it
            string playerName = serializer.ReadIndexedWideString();
            var unknownObject = serializer.ReadObjectHandle();
            // end

            bool hasRestoreState = serializer.Reader.ReadBooleanStrict();

            if (hasRestoreState)
            {
                var playerRestoreState = serializer.DeserializeType<PlayerRestoreState>();
            }
        }
    }
}
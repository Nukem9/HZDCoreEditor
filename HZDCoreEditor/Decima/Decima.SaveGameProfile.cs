using System;
using System.IO;
using System.Text;

namespace Decima
{
    public class SaveGameProfile
    {
        private const uint HardcodedMagic = 0x50504747; // 'GGPP'
        private const uint HardcodedGameVersion = 46;
        private const string HardcodedGameVersionString = "HRZ-PCR 46/5910010 09:47 - Thu Aug 27 2020";

        private FileStream _fileHandle;
        public HZD.PlayerProfile Profile;

        public SaveGameProfile(string savePath, FileMode mode = FileMode.Open)
        {
            if (mode == FileMode.Open)
                _fileHandle = File.Open(savePath, mode, FileAccess.Read, FileShare.Read);
            else if (mode == FileMode.Create || mode == FileMode.CreateNew)
                _fileHandle = File.Open(savePath, mode, FileAccess.Write, FileShare.None);
            else
                throw new NotImplementedException("File mode must be Open, Create, or CreateNew");
        }

        public void ReadProfile()
        {
            _fileHandle.Position = 0;

            using (var reader = new BinaryReader(_fileHandle, Encoding.UTF8, true))
            {
                uint magic = reader.ReadUInt32();

                if (magic != HardcodedMagic)
                    throw new Exception();

                uint playerProfileSize = reader.ReadUInt32();   // sizeof(class PlayerProfile) = 0x1AF
                uint savedDataChunkSize = reader.ReadUInt32();  // Length of all data minus the header (0xC)

                var state = new SaveState(reader, 0, (uint)reader.BaseStream.Position, savedDataChunkSize);

                // Handle game version
                int gameVersion = state.ReadVariableLengthOffset();
                string gameVersionString = state.ReadIndexedString();

                if (gameVersion != HardcodedGameVersion)
                    throw new Exception("Unknown profile version");

                if (gameVersionString != HardcodedGameVersionString)
                    throw new Exception("Unknown profile version");

                // Read the root structure (PlayerProfile)
                Profile = state.DeserializeType<HZD.PlayerProfile>();
            }
        }

        public void WriteProfile()
        {
            _fileHandle.Position = 0;

            using (var writer = new BinaryWriter(_fileHandle, Encoding.UTF8, true))
            {
                throw new NotImplementedException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decima
{
    class ProfileBinary
    {
        private const uint HardcodedMagic = 0x50504747; // 'GGPP'

        private uint PlayerProfileSize;     // sizeof(class PlayerProfile) = 0x1AF
        private uint SavedDataChunkLength;  // Length of all data minus the header (0xC)
        private uint HeaderEndOffset;       // 0xC

        private SaveDataSerializer BaseSerializer;

        public ProfileBinary(string savePath, FileMode mode = FileMode.Open)
        {
            if (mode == FileMode.Open)
            {
                var ArchiveFileHandle = File.Open(savePath, mode, FileAccess.Read, FileShare.Read);

                using (var reader = new BinaryReader(ArchiveFileHandle, Encoding.UTF8, true))
                {
                    uint magic = reader.ReadUInt32();

                    if (magic != HardcodedMagic)
                        throw new Exception();

                    PlayerProfileSize = reader.ReadUInt32();
                    SavedDataChunkLength = reader.ReadUInt32();
                    HeaderEndOffset = (uint)reader.BaseStream.Position;

                    BaseSerializer = new SaveDataSerializer(reader, 0);
                    BaseSerializer.ReadStringsAndRTTIFields(HeaderEndOffset, SavedDataChunkLength);

                    // Handle game version
                    int gameVersion = BaseSerializer.ReadVariableLengthOffset();
                    string gameVersionString = BaseSerializer.ReadIndexedString();

                    if (gameVersion != 46)
                        throw new Exception();

                    if (gameVersionString != "HRZ-PCR 46/5910010 09:47 - Thu Aug 27 2020")
                        throw new Exception();

                    // Read the root structure (PlayerProfile)
                    var profile = BaseSerializer.DeserializeType<HZD.PlayerProfile>();
                }
            }
            else if (mode == FileMode.Create || mode == FileMode.CreateNew)
            {
                throw new NotImplementedException("Writing archives is not supported at the moment");
            }
            else
            {
                throw new NotImplementedException("Archive file mode must be Open, Create, or CreateNew");
            }
        }
    }
}
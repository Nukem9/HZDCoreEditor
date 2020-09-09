using BinaryStreamExtensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Decima
{
    public class SaveGameSystem
    {
        private SaveDataSerializer BaseSerializer;

        public SaveGameSystem(string savePath, FileMode mode = FileMode.Open)
        {
            if (mode == FileMode.Open)
            {
                var fileHandle = File.Open(savePath, mode, FileAccess.Read, FileShare.Read);

                using (var reader = new BinaryReader(fileHandle, Encoding.UTF8, true))
                {
                    // Offset 0x0
                    string gameVersionString = Encoding.ASCII.GetString(reader.ReadBytesStrict(32));
                    uint gameVersion = reader.ReadUInt32();
                    byte saveVersion = reader.ReadByte();
                    byte saveFlags = reader.ReadByte();// { 0x80 = unknown, 0x1 = NG+, 0x2 = DLC entitlements }

                    ushort worldIdHash = reader.ReadUInt16();// CRC32-C xor'd - "World"
                    bool isCoopGameMode = reader.ReadBooleanStrict();
                    _ = reader.ReadBytesStrict(3);

                    // Offset 0x2C
                    uint gameStateBlockLength = reader.ReadUInt32();
                    var gameStateBlock = reader.ReadBytesStrict(256);

                    if (gameStateBlockLength != 84)
                        throw new Exception();

                    // Offset 0x130
                    int unknown1 = reader.ReadInt32();// Sign extended
                    int saveType = reader.ReadInt32();// Sign extended { 1 = manual, 2 = quick, 4 = auto, 8 = NG+ start point }

                    var gameModuleGUID = Decima.HZD.GGUUID.FromData(reader);    // Field from `class GameModule`
                    var uniqueSaveGUID = Decima.HZD.GGUUID.FromData(reader);    // CoCreateGuid() on save
                    var gameLoadGUID = Decima.HZD.GGUUID.FromData(reader);      // CoCreateGuid() on game start
                    var systemTypeGUID = Decima.HZD.GGUUID.FromData(reader);    // Possibly GUID for Win32System or physics

                    double playTimeInSeconds = reader.ReadDouble();
                    _ = reader.ReadBytesStrict(108);

                    // Offset 0x1EC
                    var dataBlockMD5 = reader.ReadBytesStrict(16);
                    uint dataBlockLength = reader.ReadUInt32();

                    // Parse actual save data
                    BaseSerializer = new SaveDataSerializer(reader, saveVersion);
                    BaseSerializer.ReadStringsAndRTTIFields(512, dataBlockLength);

                    var unknownData1 = BaseSerializer.Reader.ReadBytesStrict(24);
                    var unknownObject1 = BaseSerializer.ReadObjectHandle();
                    var unknownString1 = BaseSerializer.ReadIndexedString();// Likely entity RTTI name for the player's current mount. Instanced by AIManager.

                    var unknownData2 = BaseSerializer.Reader.ReadBytesStrict(24);
                    var unknownObject2 = BaseSerializer.ReadObjectHandle();

                    if (BaseSerializer.Reader.BaseStream.Position != 0x234)
                        Debugger.Break();

                    // GameModule info
                    {
                        byte unknownByte = BaseSerializer.Reader.ReadByte();

                        if (unknownByte != 0)
                        {
                            var unknownData = BaseSerializer.Reader.ReadBytesStrict(24);
                        }
                    }

                    // StreamingStrategyManagerGame
                    {
                        int count = BaseSerializer.ReadVariableLengthInt();

                        for (int i = 0; i < count; i++)
                        {
                            string objectType = BaseSerializer.ReadIndexedString();
                            var guid = BaseSerializer.ReadIndexedGUID();

                            // See comments in StreamingStrategyInstance
                            var obj = RTTI.CreateObjectInstance(RTTI.GetTypeByName(objectType));

                            (obj as RTTI.ISaveSerializable).DeserializeStateObject(BaseSerializer);
                        }
                    }

                    // SceneManagerGame
                    {
                        int count = BaseSerializer.ReadVariableLengthInt();

                        for (int i = 0; i < count; i++)
                        {
                            var guid = BaseSerializer.ReadIndexedGUID();
                        }
                    }

                    var factDB = BaseSerializer.DeserializeType<HZD.FactDatabase>();
                    var gameModule = new HZD.GameModule();
                    gameModule.ReadSave(BaseSerializer);
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
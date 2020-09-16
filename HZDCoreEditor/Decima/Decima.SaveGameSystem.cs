using Utility;
using Decima.HZD;
using System;
using System.IO;
using System.Text;

namespace Decima
{
    public class SaveGameSystem
    {
        public SaveState State { get; private set; }

        public GameModule GlobalGameModule;
        public StreamingStrategyManagerGame GlobalStreamingStrategyManagerGame;
        public SceneManagerGame GlobalSceneManagerGame;
        public FactDatabase GlobalFactDatabase;
        public GameSettings GlobalGameSettings;
        public WorldState GlobalWorldState;
        public MapZoneManager GlobalMapZoneManager;
        public PickUpDatabaseGame GlobalPickUpDatabaseGame;
        public QuestSystem GlobalQuestSystem;
        public CountdownTimerManager GlobalCountdownTimerManager;
        public WorldEncounterManager GlobalWorldEncounterManager;
        public EntityManagerGame GlobalEntityManagerGame;
        public MenuBadgeManager GlobalMenuBadgeManager;
        public CollectableManager GlobalCollectableManager;
        public PlayerGame GlobalPlayerGame;
        public LocationMarkerManager GlobalLocationMarkerManager;
        public ExplorationSystem GlobalExplorationSystem;
        public BuddyManager GlobalBuddyManager;
        public WeatherSystem GlobalWeatherSystem;

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

                    var gameModuleGUID = GGUUID.FromData(reader);    // Field from `class GameModule`
                    var uniqueSaveGUID = GGUUID.FromData(reader);    // CoCreateGuid() on save
                    var gameLoadGUID = GGUUID.FromData(reader);      // CoCreateGuid() on game start
                    var systemTypeGUID = GGUUID.FromData(reader);    // Possibly GUID for Win32System or physics

                    double playTimeInSeconds = reader.ReadDouble();
                    _ = reader.ReadBytesStrict(108);

                    // Offset 0x1EC
                    var dataBlockMD5 = reader.ReadBytesStrict(16);
                    uint dataBlockLength = reader.ReadUInt32();

                    // Parse actual save data
                    State = new SaveState(reader, saveVersion, (uint)reader.BaseStream.Position, dataBlockLength);

                    var unknownData1 = State.Reader.ReadBytesStrict(24);
                    var unknownObject1 = State.ReadObjectHandle();
                    var unknownString1 = State.ReadIndexedString();// Likely entity RTTI name for the player's current mount. Instanced by AIManager.

                    var unknownData2 = State.Reader.ReadBytesStrict(24);
                    var unknownObject2 = State.ReadObjectHandle();

                    GlobalGameModule = RTTI.CreateObjectInstance<GameModule>();
                    GlobalStreamingStrategyManagerGame = RTTI.CreateObjectInstance<StreamingStrategyManagerGame>();
                    GlobalSceneManagerGame = RTTI.CreateObjectInstance<SceneManagerGame>();

                    // GameModule info
                    {
                        byte unknownByte = State.Reader.ReadByte();

                        if (unknownByte != 0)
                        {
                            var unknownData = State.Reader.ReadBytesStrict(24);
                        }
                    }

                    GlobalStreamingStrategyManagerGame.ReadSave(State);
                    GlobalSceneManagerGame.ReadSave(State);
                    GlobalFactDatabase = State.DeserializeType<FactDatabase>();
                    GlobalGameModule.ReadSaveSystem(this);
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
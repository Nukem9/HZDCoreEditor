using BinaryStreamExtensions;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3CFC62664867B046)]
    public class GameModule : Module
    {
        public void ReadSave(SaveDataSerializer serializer)
        {
            // GMNB
            string unknownGUID = serializer.ReadIndexedString();// Possibly session GUID

            if (serializer.FileDataVersion < 28)
            {
                _ = serializer.Reader.ReadBytesStrict(8);
                _ = serializer.Reader.ReadBytesStrict(4);
            }

            // Version check due to slow motion bug that was patched?
            if (serializer.FileDataVersion < 28)
            {
                ulong oldGameTickCounter = serializer.Reader.ReadUInt64();
            }
            else
            {
                ulong gameTickCounter = serializer.Reader.ReadUInt64();
            }

            uint unknown1 = serializer.Reader.ReadUInt32();
            uint unknown2 = serializer.Reader.ReadUInt32();

            if (serializer.FileDataVersion < 28)
            {
                ulong unknownOldTickCounter = serializer.Reader.ReadUInt64();
            }
            else
            {
                ulong unknownTickCounter = serializer.Reader.ReadUInt64();
            }

            var gameSettings = new GameSettings();
            gameSettings.ReadSave(serializer);

            if (serializer.FileDataVersion >= 22)
            {
                int difficulty = serializer.ReadVariableLengthInt();

                // NOTE: difficulty is adjusted (+/- 1) if version is greater than 23
                // NOTE: -1 = skipped, otherwise applied to gPlayerProfile->PlayerParams[0].Difficulty
            }

            var worldState = new WorldState();
            worldState.ReadSave(serializer);

            var mapZoneManager = new MapZoneManager();
            mapZoneManager.ReadSave(serializer);

            var pickUpDatabase = new PickUpDatabaseGame();
            pickUpDatabase.ReadSave(serializer);

            var questSystem = new QuestSystem();
            questSystem.DeserializeStateObject(serializer);

            var countdownTimerManager = new CountdownTimerManager();
            countdownTimerManager.DeserializeStateObject(serializer);

            var worldEncounterManager = new WorldEncounterManager();
            worldEncounterManager.DeserializeStateObject(serializer);

            var entityManager = new EntityManagerGame();
            entityManager.ReadSave(serializer);

            var menuBadgeManager = new MenuBadgeManager();
            menuBadgeManager.DeserializeStateObject(serializer);

            var collectableManager = new CollectableManager();
            collectableManager.ReadSave(serializer);

            var playerGame = new PlayerGame();
            playerGame.ReadSave(serializer);

            var locationMarkerManager = new LocationMarkerManager();
            locationMarkerManager.ReadSave(serializer);

            var explorationSystem = new ExplorationSystem();
            explorationSystem.ReadSave(serializer);

            var buddyManager = new BuddyManager();
            buddyManager.ReadSave(serializer);

            var weatherSystem = new WeatherSystem();
            weatherSystem.ReadSave(serializer);

            // Unknown structure
            if (serializer.FileDataVersion >= 25)
            {
                int count = serializer.Reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var guid = serializer.ReadIndexedGUID();
                    int unknown = serializer.Reader.ReadInt32();
                }
            }
        }
    }
}
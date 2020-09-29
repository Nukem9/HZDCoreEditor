using Utility;

namespace Decima.HZD
{
    [RTTI.Serializable(0x3CFC62664867B046, GameType.HZD)]
    public class GameModule : Module
    {
        public void ReadSaveSystem(SaveGameSystem system)
        {
            SaveState state = system.State;

            system.GlobalGameSettings = RTTI.CreateObjectInstance<GameSettings>();
            system.GlobalWorldState = RTTI.CreateObjectInstance<WorldState>();
            system.GlobalMapZoneManager = RTTI.CreateObjectInstance<MapZoneManager>();
            system.GlobalPickUpDatabaseGame = RTTI.CreateObjectInstance<PickUpDatabaseGame>();
            system.GlobalQuestSystem = RTTI.CreateObjectInstance<QuestSystem>();
            system.GlobalCountdownTimerManager = RTTI.CreateObjectInstance<CountdownTimerManager>();
            system.GlobalWorldEncounterManager = RTTI.CreateObjectInstance<WorldEncounterManager>();
            system.GlobalEntityManagerGame = RTTI.CreateObjectInstance<EntityManagerGame>();
            system.GlobalMenuBadgeManager = RTTI.CreateObjectInstance<MenuBadgeManager>();
            system.GlobalCollectableManager = RTTI.CreateObjectInstance<CollectableManager>();
            system.GlobalPlayerGame = RTTI.CreateObjectInstance<PlayerGame>();
            system.GlobalLocationMarkerManager = RTTI.CreateObjectInstance<LocationMarkerManager>();
            system.GlobalExplorationSystem = RTTI.CreateObjectInstance<ExplorationSystem>();
            system.GlobalBuddyManager = RTTI.CreateObjectInstance<BuddyManager>();
            system.GlobalWeatherSystem = RTTI.CreateObjectInstance<WeatherSystem>();

            // GMNB
            string unknownGUID = state.ReadIndexedString();// Possibly session GUID

            if (state.SaveVersion < 28)
            {
                _ = state.Reader.ReadBytesStrict(8);
                _ = state.Reader.ReadBytesStrict(4);
            }

            // Version check due to slow motion bug that was patched?
            if (state.SaveVersion < 28)
            {
                ulong oldGameTickCounter = state.Reader.ReadUInt64();
            }
            else
            {
                ulong gameTickCounter = state.Reader.ReadUInt64();
            }

            uint unknown1 = state.Reader.ReadUInt32();
            uint unknown2 = state.Reader.ReadUInt32();

            if (state.SaveVersion < 28)
            {
                ulong unknownOldTickCounter = state.Reader.ReadUInt64();
            }
            else
            {
                ulong unknownTickCounter = state.Reader.ReadUInt64();
            }

            system.GlobalGameSettings.ReadSave(state);

            if (state.SaveVersion >= 22)
            {
                int difficulty = state.ReadVariableLengthInt();

                // NOTE: difficulty is adjusted (+/- 1) if version is greater than 23
                // NOTE: -1 = skipped, otherwise applied to gPlayerProfile->PlayerParams[0].Difficulty
            }

            system.GlobalWorldState.ReadSave(state);
            system.GlobalMapZoneManager.ReadSave(state);
            system.GlobalPickUpDatabaseGame.ReadSave(state);
            system.GlobalQuestSystem.DeserializeStateObject(state);
            system.GlobalCountdownTimerManager.DeserializeStateObject(state);
            system.GlobalWorldEncounterManager.DeserializeStateObject(state);
            system.GlobalEntityManagerGame.ReadSave(state);
            system.GlobalMenuBadgeManager.DeserializeStateObject(state);
            system.GlobalCollectableManager.ReadSave(state);
            system.GlobalPlayerGame.ReadSave(state);
            system.GlobalLocationMarkerManager.ReadSave(state);
            system.GlobalExplorationSystem.ReadSave(state);
            system.GlobalBuddyManager.ReadSave(state);
            system.GlobalWeatherSystem.ReadSave(state);

            // Unknown structure
            if (state.SaveVersion >= 25)
            {
                int count = state.Reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var guid = state.ReadIndexedGUID();
                    int unknown = state.Reader.ReadInt32();
                }
            }
        }
    }
}
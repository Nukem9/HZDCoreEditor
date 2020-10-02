using Utility;

namespace Decima.HZD
{
    using int64 = System.Int64;

    [RTTI.Serializable(0x4E58751A666F12C7, GameType.HZD)]
    public class GameSettings : PlaylistData
    {
        [RTTI.Member(3, 0x28)] public int VersionNumber;
        [RTTI.Member(4, 0x30)] public String Name;
        [RTTI.Member(5, 0x38)] public String Description;
        [RTTI.Member(6, 0x40)] public String Creator;
        [RTTI.Member(7, 0x48)] public int64 CreatedTimestamp;
        [RTTI.Member(53, 0x50, "PlayerSettings")] public bool LateJoinersAllowed;
        [RTTI.Member(8, 0x58)] public Array<Ref<GameRoundSettings>> RoundSettings;
        [RTTI.Member(19, 0x68, "Missions")] public Array<MissionSettings> MissionSetting;
        [RTTI.Member(40, 0x78, "PlayerSettings")] public bool EndGameWhenWinnerDecided;
        [RTTI.Member(41, 0x7C, "PlayerSettings")] public int SpawnLives;
        [RTTI.Member(11, 0x80, "Timers")] public bool UseSpawnWaves;
        [RTTI.Member(12, 0x84, "Timers")] public float DeathCamTime;
        [RTTI.Member(13, 0x88, "Timers")] public int RespawnTimer;
        [RTTI.Member(14, 0x8C, "Timers")] public int GracePeriod;
        [RTTI.Member(15, 0x90, "Timers")] public int InactivityKickTime;
        [RTTI.Member(16, 0x94, "Timers")] public int TimeBetweenMissions;
        [RTTI.Member(20, 0x98, "Missions")] public int BodycountQuota;
        [RTTI.Member(21, 0x9C, "Missions")] public int CaHCaptureTime;
        [RTTI.Member(22, 0xA0, "Missions")] public int CaCCaptureTimeInner;
        [RTTI.Member(23, 0xA4, "Missions")] public int CaCCaptureTimeMiddle;
        [RTTI.Member(24, 0xA8, "Missions")] public int CaCCaptureTimeOuter;
        [RTTI.Member(25, 0xAC, "Missions")] public int CaSCaptureTime;
        [RTTI.Member(26, 0xB0, "Missions")] public int ExplosivePlacing;
        [RTTI.Member(27, 0xB4, "Missions")] public int ExplosiveDefusing;
        [RTTI.Member(28, 0xB8, "Missions")] public int ExplosiveDetonationTime;
        [RTTI.Member(36, 0xBC, "Weapons")] public bool FriendlyFireEnabled;
        [RTTI.Member(57, 0xC0, "PlayerSettings")] public ECloseCombatSettings CloseCombatSettings;
        [RTTI.Member(58, 0xC4, "PlayerSettings")] public bool ShowEnemiesOnRadar;
        [RTTI.Member(59, 0xC5, "PlayerSettings")] public bool ShowAmmoCounter;
        [RTTI.Member(60, 0xC8, "PlayerSettings")] public EAmmoSettings AmmoSettings;
        [RTTI.Member(9, 0xCC)] public int MaxPlayerCount;
        [RTTI.Member(42, 0xD0, "PlayerSettings")] public int MaxPlayerSpectatorCount;
        [RTTI.Member(43, 0xD4, "PlayerSettings")] public int MaxAdminSpectatorCount;
        [RTTI.Member(44, 0xD8, "PlayerSettings")] public int MinClientCount;
        [RTTI.Member(45, 0xDC, "PlayerSettings")] public int ClansMinPlayerCount;
        [RTTI.Member(64, 0xE0, "PlayerSettings")] public int ClansForfeitTimer;
        [RTTI.Member(38, 0xE4, "PlayerSettings")] public EPlayerHealthSettings PlayerHealthSettings;
        [RTTI.Member(39, 0xE8, "PlayerSettings")] public EHealthRegenerationSettings HealthRegenerationSettings;
        [RTTI.Member(46, 0xEC, "PlayerSettings")] public int MaxBotCount;
        [RTTI.Member(47, 0xF0, "PlayerSettings")] public EFaction BotFaction;
        [RTTI.Member(48, 0xF4, "PlayerSettings")] public bool SplitScreenGame;
        [RTTI.Member(52, 0xF8, "PlayerSettings")] public EGameMode GameMode;
        [RTTI.Member(49, 0xFC, "PlayerSettings")] public bool BotzoneGame;
        [RTTI.Member(50, 0xFD, "PlayerSettings")] public bool PracticeGame;
        [RTTI.Member(51, 0xFE, "PlayerSettings")] public bool ClanGame;
        [RTTI.Member(54, 0xFF, "PlayerSettings")] public bool IsCustomGame;
        [RTTI.Member(55, 0x100, "PlayerSettings")] public bool IsAdminCreatedGame;
        [RTTI.Member(56, 0x101, "PlayerSettings")] public bool CampaignScoringEnabled;
        [RTTI.Member(30, 0x110, "Careers")] public Array<ECareerSettings> CareerSettings;
        [RTTI.Member(32, 0x120, "UnlockResources")] public Array<String> DisabledUnlockResources;
        [RTTI.Member(34, 0x130, "CombatHonors")] public bool CombatHonorsEnabled;
        [RTTI.Member(66, 0x138, "PlaylistLeaderboardStats")] public Array<String> TrackedLeaderboardStats;
        [RTTI.Member(61, 0x148, "PlayerSettings")] public String PlaylistPassword;
        [RTTI.Member(62, 0x150, "PlayerSettings")] public String SelectedChallengeRequirements;
        [RTTI.Member(70, 0x158, "VoiceChat")] public bool TeamVoiceChat;
        [RTTI.Member(63, 0x159, "PlayerSettings")] public bool PartiesAllowed;
        [RTTI.Member(17, 0x15C, "Timers")] public int PreGameLobbyWaitTime;

        public void ReadSave(SaveState state)
        {
            Name = state.ReadIndexedString();
            Description = state.ReadIndexedString();
            LateJoinersAllowed = state.Reader.ReadBooleanStrict();

            // RoundSettings
            int roundSettingCount = state.ReadVariableLengthInt();
            RoundSettings = new Array<Ref<GameRoundSettings>>(roundSettingCount);

            for (int i = 0; i < roundSettingCount; i++)
            {
                string settingName = state.ReadIndexedString();

                // TODO: Object/resource name is "World" with type "GameRoundSettings" - lookups not currently implemented
                RoundSettings.Add(new Ref<GameRoundSettings>());
            }

            // MissionSetting
            int missionSettingCount = state.ReadVariableLengthInt();
            MissionSetting = new Array<MissionSettings>(missionSettingCount);

            for (int i = 0; i < missionSettingCount; i++)
            {
                var settings = new MissionSettings();
                settings.ReadSave(state);

                MissionSetting.Add(settings);
            }

            // Let's serialize this entire structure even though reflection is available. Brilliant.
            EndGameWhenWinnerDecided = state.Reader.ReadBooleanStrict();
            SpawnLives = state.ReadVariableLengthInt();
            UseSpawnWaves = state.Reader.ReadBooleanStrict();
            DeathCamTime = state.Reader.ReadSingle();
            RespawnTimer = state.ReadVariableLengthInt();
            GracePeriod = state.ReadVariableLengthInt();
            InactivityKickTime = state.ReadVariableLengthInt();
            TimeBetweenMissions = state.ReadVariableLengthInt();
            BodycountQuota = state.ReadVariableLengthInt();
            CaHCaptureTime = state.ReadVariableLengthInt();
            CaCCaptureTimeInner = state.ReadVariableLengthInt();
            CaCCaptureTimeMiddle = state.ReadVariableLengthInt();
            CaCCaptureTimeOuter = state.ReadVariableLengthInt();
            CaSCaptureTime = state.ReadVariableLengthInt();
            ExplosivePlacing = state.ReadVariableLengthInt();
            ExplosiveDefusing = state.ReadVariableLengthInt();
            ExplosiveDetonationTime = state.ReadVariableLengthInt();
            FriendlyFireEnabled = state.Reader.ReadBooleanStrict();
            CloseCombatSettings = (ECloseCombatSettings)state.Reader.ReadByte();
            ShowEnemiesOnRadar = state.Reader.ReadBooleanStrict();
            ShowAmmoCounter = state.Reader.ReadBooleanStrict();
            AmmoSettings = (EAmmoSettings)state.Reader.ReadByte();
            MaxPlayerCount = state.ReadVariableLengthInt();
            MaxPlayerSpectatorCount = state.ReadVariableLengthInt();
            MaxAdminSpectatorCount = state.ReadVariableLengthInt();
            MinClientCount = state.ReadVariableLengthInt();
            ClansMinPlayerCount = state.ReadVariableLengthInt();
            ClansForfeitTimer = state.Reader.ReadInt32();// !
            PlayerHealthSettings = (EPlayerHealthSettings)state.Reader.ReadByte();
            HealthRegenerationSettings = (EHealthRegenerationSettings)state.Reader.ReadByte();
            MaxBotCount = state.ReadVariableLengthInt();
            BotFaction = (EFaction)state.Reader.ReadByte();
            SplitScreenGame = state.Reader.ReadBooleanStrict();
            GameMode = (EGameMode)state.Reader.ReadByte();
            BotzoneGame = state.Reader.ReadBooleanStrict();
            PracticeGame = state.Reader.ReadBooleanStrict();
            ClanGame = state.Reader.ReadBooleanStrict();
            IsCustomGame = state.Reader.ReadBooleanStrict();
            IsAdminCreatedGame = state.Reader.ReadBooleanStrict();
            CampaignScoringEnabled = state.Reader.ReadBooleanStrict();
            _ = state.Reader.ReadByte();// !

            PlaylistPassword = state.ReadIndexedString();
            SelectedChallengeRequirements = state.ReadIndexedString();

            // CareerSettings
            int careerSettingCount = state.ReadVariableLengthInt();
            CareerSettings = new Array<ECareerSettings>(careerSettingCount);

            for (int i = 0; i < careerSettingCount; i++)
                CareerSettings.Add((ECareerSettings)state.Reader.ReadByte());

            DisabledUnlockResources = new Array<String>();
            DisabledUnlockResources.DeserializeStateObject(state);

            TrackedLeaderboardStats = new Array<String>();
            TrackedLeaderboardStats.DeserializeStateObject(state);

            CombatHonorsEnabled = state.Reader.ReadBooleanStrict();
            Creator = state.ReadIndexedString();
            CreatedTimestamp = state.Reader.ReadInt64();
            TeamVoiceChat = state.Reader.ReadBooleanStrict();
            PartiesAllowed = state.Reader.ReadBooleanStrict();
            PreGameLobbyWaitTime = state.ReadVariableLengthInt();
        }
    }
}
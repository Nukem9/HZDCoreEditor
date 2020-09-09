using BinaryStreamExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decima.HZD
{
    using int64 = System.Int64;

    [RTTI.Serializable(0x4E58751A666F12C7)]
    public class GameSettings : PlaylistData
    {
        [RTTI.Member(0, 0x28)] public int VersionNumber;
        [RTTI.Member(1, 0x30)] public String Name;
        [RTTI.Member(2, 0x38)] public String Description;
        [RTTI.Member(3, 0x40)] public String Creator;
        [RTTI.Member(4, 0x48)] public int64 CreatedTimestamp;
        [RTTI.Member(5, 0x50, "PlayerSettings")] public bool LateJoinersAllowed;
        [RTTI.Member(6, 0x58)] public Array<Ref<GameRoundSettings>> RoundSettings;
        [RTTI.Member(7, 0x68, "Missions")] public Array<MissionSettings> MissionSetting;
        [RTTI.Member(8, 0x78, "PlayerSettings")] public bool EndGameWhenWinnerDecided;
        [RTTI.Member(9, 0x7C, "PlayerSettings")] public int SpawnLives;
        [RTTI.Member(10, 0x80, "Timers")] public bool UseSpawnWaves;
        [RTTI.Member(11, 0x84, "Timers")] public float DeathCamTime;
        [RTTI.Member(12, 0x88, "Timers")] public int RespawnTimer;
        [RTTI.Member(13, 0x8C, "Timers")] public int GracePeriod;
        [RTTI.Member(14, 0x90, "Timers")] public int InactivityKickTime;
        [RTTI.Member(15, 0x94, "Timers")] public int TimeBetweenMissions;
        [RTTI.Member(16, 0x98, "Missions")] public int BodycountQuota;
        [RTTI.Member(17, 0x9C, "Missions")] public int CaHCaptureTime;
        [RTTI.Member(18, 0xA0, "Missions")] public int CaCCaptureTimeInner;
        [RTTI.Member(19, 0xA4, "Missions")] public int CaCCaptureTimeMiddle;
        [RTTI.Member(20, 0xA8, "Missions")] public int CaCCaptureTimeOuter;
        [RTTI.Member(21, 0xAC, "Missions")] public int CaSCaptureTime;
        [RTTI.Member(22, 0xB0, "Missions")] public int ExplosivePlacing;
        [RTTI.Member(23, 0xB4, "Missions")] public int ExplosiveDefusing;
        [RTTI.Member(24, 0xB8, "Missions")] public int ExplosiveDetonationTime;
        [RTTI.Member(25, 0xBC, "Weapons")] public bool FriendlyFireEnabled;
        [RTTI.Member(26, 0xC0, "PlayerSettings")] public ECloseCombatSettings CloseCombatSettings;
        [RTTI.Member(27, 0xC4, "PlayerSettings")] public bool ShowEnemiesOnRadar;
        [RTTI.Member(28, 0xC5, "PlayerSettings")] public bool ShowAmmoCounter;
        [RTTI.Member(29, 0xC8, "PlayerSettings")] public EAmmoSettings AmmoSettings;
        [RTTI.Member(30, 0xCC)] public int MaxPlayerCount;
        [RTTI.Member(31, 0xD0, "PlayerSettings")] public int MaxPlayerSpectatorCount;
        [RTTI.Member(32, 0xD4, "PlayerSettings")] public int MaxAdminSpectatorCount;
        [RTTI.Member(33, 0xD8, "PlayerSettings")] public int MinClientCount;
        [RTTI.Member(34, 0xDC, "PlayerSettings")] public int ClansMinPlayerCount;
        [RTTI.Member(35, 0xE0, "PlayerSettings")] public int ClansForfeitTimer;
        [RTTI.Member(36, 0xE4, "PlayerSettings")] public EPlayerHealthSettings PlayerHealthSettings;
        [RTTI.Member(37, 0xE8, "PlayerSettings")] public EHealthRegenerationSettings HealthRegenerationSettings;
        [RTTI.Member(38, 0xEC, "PlayerSettings")] public int MaxBotCount;
        [RTTI.Member(39, 0xF0, "PlayerSettings")] public EFaction BotFaction;
        [RTTI.Member(40, 0xF4, "PlayerSettings")] public bool SplitScreenGame;
        [RTTI.Member(41, 0xF8, "PlayerSettings")] public EGameMode GameMode;
        [RTTI.Member(42, 0xFC, "PlayerSettings")] public bool BotzoneGame;
        [RTTI.Member(43, 0xFD, "PlayerSettings")] public bool PracticeGame;
        [RTTI.Member(44, 0xFE, "PlayerSettings")] public bool ClanGame;
        [RTTI.Member(45, 0xFF, "PlayerSettings")] public bool IsCustomGame;
        [RTTI.Member(46, 0x100, "PlayerSettings")] public bool IsAdminCreatedGame;
        [RTTI.Member(47, 0x101, "PlayerSettings")] public bool CampaignScoringEnabled;
        [RTTI.Member(48, 0x110, "Careers")] public Array<ECareerSettings> CareerSettings;
        [RTTI.Member(49, 0x120, "UnlockResources")] public Array<String> DisabledUnlockResources;
        [RTTI.Member(50, 0x130, "CombatHonors")] public bool CombatHonorsEnabled;
        [RTTI.Member(51, 0x138, "PlaylistLeaderboardStats")] public Array<String> TrackedLeaderboardStats;
        [RTTI.Member(52, 0x148, "PlayerSettings")] public String PlaylistPassword;
        [RTTI.Member(53, 0x150, "PlayerSettings")] public String SelectedChallengeRequirements;
        [RTTI.Member(54, 0x158, "VoiceChat")] public bool TeamVoiceChat;
        [RTTI.Member(55, 0x159, "PlayerSettings")] public bool PartiesAllowed;
        [RTTI.Member(56, 0x15C, "Timers")] public int PreGameLobbyWaitTime;

        public void ReadSave(SaveDataSerializer serializer)
        {
            Name = serializer.ReadIndexedString();
            Description = serializer.ReadIndexedString();
            LateJoinersAllowed = serializer.Reader.ReadBooleanStrict();

            // RoundSettings
            int roundSettingCount = serializer.ReadVariableLengthInt();
            RoundSettings = new Array<Ref<GameRoundSettings>>(roundSettingCount);

            for (int i = 0; i < roundSettingCount; i++)
            {
                string settingName = serializer.ReadIndexedString();

                // Object/resource name is "World" with type "GameRoundSettings" - lookups not currently implemented
                RoundSettings.Add(new Ref<GameRoundSettings>());
            }

            // MissionSetting
            int missionSettingCount = serializer.ReadVariableLengthInt();
            MissionSetting = new Array<MissionSettings>(missionSettingCount);

            for (int i = 0; i < missionSettingCount; i++)
            {
                var settings = new MissionSettings();
                settings.ReadSave(serializer);

                MissionSetting.Add(settings);
            }

            // Let's serialize this entire structure even though reflection is available. Brilliant.
            EndGameWhenWinnerDecided = serializer.Reader.ReadBooleanStrict();
            SpawnLives = serializer.ReadVariableLengthInt();
            UseSpawnWaves = serializer.Reader.ReadBooleanStrict();
            DeathCamTime = serializer.Reader.ReadSingle();
            RespawnTimer = serializer.ReadVariableLengthInt();
            GracePeriod = serializer.ReadVariableLengthInt();
            InactivityKickTime = serializer.ReadVariableLengthInt();
            TimeBetweenMissions = serializer.ReadVariableLengthInt();
            BodycountQuota = serializer.ReadVariableLengthInt();
            CaHCaptureTime = serializer.ReadVariableLengthInt();
            CaCCaptureTimeInner = serializer.ReadVariableLengthInt();
            CaCCaptureTimeMiddle = serializer.ReadVariableLengthInt();
            CaCCaptureTimeOuter = serializer.ReadVariableLengthInt();
            CaSCaptureTime = serializer.ReadVariableLengthInt();
            ExplosivePlacing = serializer.ReadVariableLengthInt();
            ExplosiveDefusing = serializer.ReadVariableLengthInt();
            ExplosiveDetonationTime = serializer.ReadVariableLengthInt();
            FriendlyFireEnabled = serializer.Reader.ReadBooleanStrict();
            CloseCombatSettings = (ECloseCombatSettings)serializer.Reader.ReadByte();
            ShowEnemiesOnRadar = serializer.Reader.ReadBooleanStrict();
            ShowAmmoCounter = serializer.Reader.ReadBooleanStrict();
            AmmoSettings = (EAmmoSettings)serializer.Reader.ReadByte();
            MaxPlayerCount = serializer.ReadVariableLengthInt();
            MaxPlayerSpectatorCount = serializer.ReadVariableLengthInt();
            MaxAdminSpectatorCount = serializer.ReadVariableLengthInt();
            MinClientCount = serializer.ReadVariableLengthInt();
            ClansMinPlayerCount = serializer.ReadVariableLengthInt();
            ClansForfeitTimer = serializer.Reader.ReadInt32();// !
            PlayerHealthSettings = (EPlayerHealthSettings)serializer.Reader.ReadByte();
            HealthRegenerationSettings = (EHealthRegenerationSettings)serializer.Reader.ReadByte();
            MaxBotCount = serializer.ReadVariableLengthInt();
            BotFaction = (EFaction)serializer.Reader.ReadByte();
            SplitScreenGame = serializer.Reader.ReadBooleanStrict();
            GameMode = (EGameMode)serializer.Reader.ReadByte();
            BotzoneGame = serializer.Reader.ReadBooleanStrict();
            PracticeGame = serializer.Reader.ReadBooleanStrict();
            ClanGame = serializer.Reader.ReadBooleanStrict();
            IsCustomGame = serializer.Reader.ReadBooleanStrict();
            IsAdminCreatedGame = serializer.Reader.ReadBooleanStrict();
            CampaignScoringEnabled = serializer.Reader.ReadBooleanStrict();
            _ = serializer.Reader.ReadByte();// !

            PlaylistPassword = serializer.ReadIndexedString();
            SelectedChallengeRequirements = serializer.ReadIndexedString();

            // CareerSettings
            int careerSettingCount = serializer.ReadVariableLengthInt();
            CareerSettings = new Array<ECareerSettings>(careerSettingCount);

            for (int i = 0; i < careerSettingCount; i++)
                CareerSettings.Add((ECareerSettings)serializer.Reader.ReadByte());

            // DisabledUnlockResources
            int disabledUnlockResourceCount = serializer.ReadVariableLengthInt();
            DisabledUnlockResources = new Array<String>(disabledUnlockResourceCount);

            for (int i = 0; i < disabledUnlockResourceCount; i++)
                DisabledUnlockResources.Add(serializer.ReadIndexedString());

            // TrackedLeaderboardStats
            int trackedLeaderboardStatsCount = serializer.ReadVariableLengthInt();
            TrackedLeaderboardStats = new Array<String>(trackedLeaderboardStatsCount);

            for (int i = 0; i < trackedLeaderboardStatsCount; i++)
                TrackedLeaderboardStats.Add(serializer.ReadIndexedString());

            CombatHonorsEnabled = serializer.Reader.ReadBooleanStrict();
            Creator = serializer.ReadIndexedString();
            CreatedTimestamp = serializer.Reader.ReadInt64();
            TeamVoiceChat = serializer.Reader.ReadBooleanStrict();
            PartiesAllowed = serializer.Reader.ReadBooleanStrict();
            PreGameLobbyWaitTime = serializer.ReadVariableLengthInt();
        }
    }
}
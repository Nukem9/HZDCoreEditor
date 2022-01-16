#pragma once

#include <string>
#include <string_view>
#include <vector>

namespace InternalModConfig
{

struct AssetOverride
{
	bool Enabled;
	std::string ObjectUUIDs;
	std::string ObjectTypes;
	std::string Path;
	std::string Value;
};

struct GlobalSettings
{
	// [General]
	bool EnableDebugMenu;
	bool EnableCoreLogging;
	bool EnableAssetLogging;
	bool EnableAssetOverrides;
	bool EnableDiscordRichPresence;
	float DebugMenuFontScale;

	// [Gameplay]
	bool SkipIntroLogos;
	bool UnlockNGPExtras;
	bool UnlockEntitlementExtras;
	std::string ForceBodyVariantUUID;

	// [Hotkeys]
	struct
	{
		int ToggleDebugUI;
		int TogglePauseGameLogic;
		int TogglePauseTimeOfDay;
		int ToggleFreeflyCamera;
		int ToggleNoclip;
		int SaveQuicksave;
		int LoadPreviousSave;
		int SpawnEntity;
		int IncreaseTimescale;
		int DecreaseTimescale;
	} Hotkeys;

	// [AssetOverrides]
	std::vector<AssetOverride> AssetOverrides;

	// [CoreObjectCache]
	std::vector<std::pair<std::string, std::string>> CachedSpawnSetups;
	std::vector<std::pair<std::string, std::string>> CachedWeatherSetups;
	std::vector<std::pair<std::string, std::string>> CachedBodyVariants;
};

bool InitializeDefault();
bool LoadFromFile(const std::string_view FilePath);

}

extern InternalModConfig::GlobalSettings ModConfiguration;
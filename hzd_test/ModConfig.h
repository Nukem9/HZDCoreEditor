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

	// [Hotkeys]
	struct
	{
		int ToggleDebugUI;
		int TogglePauseGameLogic;
		int ToggleFreeflyCamera;
		int ToggleNoclip;
		int SaveQuicksave;
		int LoadPreviousSave;
		int SpawnEntity;
	} Hotkeys;

	// [AssetOverrides]
	std::vector<AssetOverride> AssetOverrides;

	// [CoreObjectCache]
	std::vector<std::pair<std::string, std::string>> CachedSpawnSetups;
	std::vector<std::pair<std::string, std::string>> CachedWeatherSetups;
};

bool InitializeDefault();
bool LoadFromFile(const std::string_view FilePath);

}

extern InternalModConfig::GlobalSettings ModConfiguration;
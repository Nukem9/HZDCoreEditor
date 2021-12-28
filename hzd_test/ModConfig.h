#pragma once

#include <string>
#include <vector>

namespace ModConfig
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
	bool EnableAssetLogging;
	bool EnableDiscordRichPresence;
	bool OverrideGeographicalRegion;
	std::string GeographicalRegion;

	// [Gameplay]
	bool UnlockMapBorders;
	bool UnlockNGPExtras;
	bool UnlockEntitlementExtras;
	bool DisableTimeOfDay;
	bool DisableWeatherTransitions;

	// [AssetOverrides]
	std::vector<AssetOverride> AssetOverrides;
};

bool InitializeDefault();
bool LoadFromFile(const std::string_view FilePath);

}

extern ModConfig::GlobalSettings ModConfiguration;
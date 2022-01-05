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

	// [Gameplay]
	bool SkipIntroLogos;
	bool UnlockNGPExtras;
	bool UnlockEntitlementExtras;

	// [AssetOverrides]
	std::vector<AssetOverride> AssetOverrides;
};

bool InitializeDefault();
bool LoadFromFile(const std::string_view FilePath);

}

extern InternalModConfig::GlobalSettings ModConfiguration;
#include <toml++/toml.h>

#include "ModConfig.h"

InternalModConfig::GlobalSettings ModConfiguration;

namespace InternalModConfig
{

GlobalSettings ParseSettings(const toml::table& Table);
AssetOverride ParseOverride(const toml::table *Table, bool Enable);

bool InitializeDefault()
{
	return LoadFromFile("mod_config.toml");
}

bool LoadFromFile(const std::string_view FilePath)
{
	// Try to parse toml data from disk
	toml::table table;

	try
	{
		table = toml::parse_file("mod_config.toml");
	}
	catch (const toml::parse_error&)
	{
		return false;
	}

	ModConfiguration = ParseSettings(table);
	return true;
}

#define PARSE_TOML_MEMBER(obj, x) o.x = (*obj)[#x].value_or(decltype(o.x){})
#define PARSE_TOML_HOTKEY(obj, x) o.Hotkeys.x = (*obj)[#x].value_or(-1)

GlobalSettings ParseSettings(const toml::table& Table)
{
	GlobalSettings o;

	// [General]
	if (auto general = Table["General"].as_table())
	{
		PARSE_TOML_MEMBER(general, EnableDebugMenu);
		PARSE_TOML_MEMBER(general, EnableCoreLogging);
		PARSE_TOML_MEMBER(general, EnableAssetLogging);
		PARSE_TOML_MEMBER(general, EnableAssetOverrides);
		PARSE_TOML_MEMBER(general, EnableDiscordRichPresence);
		PARSE_TOML_MEMBER(general, DebugMenuFontScale);
	}

	if (o.DebugMenuFontScale <= 0)
		o.DebugMenuFontScale = 1.0f;

	// [Gameplay]
	if (auto gameplay = Table["Gameplay"].as_table())
	{
		PARSE_TOML_MEMBER(gameplay, SkipIntroLogos);
		PARSE_TOML_MEMBER(gameplay, UnlockNGPExtras);
		PARSE_TOML_MEMBER(gameplay, UnlockEntitlementExtras);
		PARSE_TOML_MEMBER(gameplay, ForceBodyVariantUUID);
	}

	// [Hotkeys]
	if (auto hotkeys = Table["Hotkeys"].as_table())
	{
		PARSE_TOML_HOTKEY(hotkeys, ToggleDebugUI);
		PARSE_TOML_HOTKEY(hotkeys, TogglePauseGameLogic);
		PARSE_TOML_HOTKEY(hotkeys, TogglePauseTimeOfDay);
		PARSE_TOML_HOTKEY(hotkeys, ToggleFreeflyCamera);
		PARSE_TOML_HOTKEY(hotkeys, ToggleNoclip);
		PARSE_TOML_HOTKEY(hotkeys, SaveQuicksave);
		PARSE_TOML_HOTKEY(hotkeys, LoadPreviousSave);
		PARSE_TOML_HOTKEY(hotkeys, SpawnEntity);
		PARSE_TOML_HOTKEY(hotkeys, IncreaseTimescale);
		PARSE_TOML_HOTKEY(hotkeys, DecreaseTimescale);
	}

	// [AssetOverrides]
	if (o.EnableAssetOverrides)
	{
		auto enabledOverrides = Table["AssetOverrides"]["Enabled"].as_array();
		auto disabledOverrides = Table["AssetOverrides"]["Disabled"].as_array();

		if (enabledOverrides)
		{
			for (auto& entry : *enabledOverrides)
				o.AssetOverrides.emplace_back(ParseOverride(entry.as_table(), true));
		}

		if (disabledOverrides)
		{
			for (auto& entry : *disabledOverrides)
				o.AssetOverrides.emplace_back(ParseOverride(entry.as_table(), false));
		}
	}

	// [CoreObjectCache]
	auto parseCoreObjectCacheTable = [&Table](const char *Name, auto& TargetVector)
	{
		auto cachedObjectArray = Table["CoreObjectCache"][Name].as_array();

		if (cachedObjectArray)
		{
			for (auto& entry : *cachedObjectArray)
			{
				auto& a = *entry.as_array();
				TargetVector.emplace_back(a[0].value_or(""), a[1].value_or(""), a[2].value_or(""));
			}
		}
	};

	parseCoreObjectCacheTable("CachedSpawnSetups", o.CachedSpawnSetups);
	parseCoreObjectCacheTable("CachedWeatherSetups", o.CachedWeatherSetups);
	parseCoreObjectCacheTable("CachedBodyVariants", o.CachedBodyVariants);

	return o;
}

AssetOverride ParseOverride(const toml::table *Table, bool Enable)
{
	AssetOverride o;

	o.Enabled = Enable;
	PARSE_TOML_MEMBER(Table, ObjectUUIDs);
	PARSE_TOML_MEMBER(Table, ObjectTypes);
	PARSE_TOML_MEMBER(Table, Path);
	PARSE_TOML_MEMBER(Table, Value);

	return o;
}

#undef PARSE_TOML_HOTKEY
#undef PARSE_TOML_MEMBER

}
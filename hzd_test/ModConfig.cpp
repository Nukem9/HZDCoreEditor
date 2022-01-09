#include <toml++/toml.h>

#include "ModConfig.h"

InternalModConfig::GlobalSettings ModConfiguration;

namespace InternalModConfig
{

GlobalSettings ParseSettings(const toml::table& Table);
AssetOverride ParseOverride(const toml::table& Table, bool Enable);

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

#define PARSE_TOML_MEMBER(obj, x) o.x = obj[#x].value_or(decltype(o.x){})

GlobalSettings ParseSettings(const toml::table& Table)
{
	GlobalSettings o;

	auto& general = *Table["General"].as_table();
	PARSE_TOML_MEMBER(general, EnableDebugMenu);
	PARSE_TOML_MEMBER(general, EnableCoreLogging);
	PARSE_TOML_MEMBER(general, EnableAssetLogging);
	PARSE_TOML_MEMBER(general, EnableAssetOverrides);
	PARSE_TOML_MEMBER(general, EnableDiscordRichPresence);
	PARSE_TOML_MEMBER(general, DebugMenuFontScale);

	if (o.DebugMenuFontScale <= 0)
		o.DebugMenuFontScale = 1.0f;

	auto& gameplay = *Table["Gameplay"].as_table();
	PARSE_TOML_MEMBER(gameplay, SkipIntroLogos);
	PARSE_TOML_MEMBER(gameplay, UnlockNGPExtras);
	PARSE_TOML_MEMBER(gameplay, UnlockEntitlementExtras);

	if (o.EnableAssetOverrides)
	{
		auto enabledOverrides = Table["AssetOverrides"]["Enabled"].as_array();
		auto disabledOverrides = Table["AssetOverrides"]["Disabled"].as_array();

		if (enabledOverrides)
		{
			for (auto& entry : *enabledOverrides)
				o.AssetOverrides.emplace_back(ParseOverride(*entry.as_table(), true));
		}

		if (disabledOverrides)
		{
			for (auto& entry : *disabledOverrides)
				o.AssetOverrides.emplace_back(ParseOverride(*entry.as_table(), false));
		}
	}

	auto cachedSpawnSetups = Table["CoreObjectCache"]["CachedSpawnSetups"].as_array();

	if (cachedSpawnSetups)
	{
		for (auto& entry : *cachedSpawnSetups)
		{
			auto& a = *entry.as_array();
			o.CachedSpawnSetups.emplace_back(a[0].value_or(""), a[1].value_or(""));
		}
	}

	auto cachedWeatherSetups = Table["CoreObjectCache"]["CachedWeatherSetups"].as_array();

	if (cachedWeatherSetups)
	{
		for (auto& entry : *cachedWeatherSetups)
		{
			auto& a = *entry.as_array();
			o.CachedWeatherSetups.emplace_back(a[0].value_or(""), a[1].value_or(""));
		}
	}

	return o;
}

AssetOverride ParseOverride(const toml::table& Table, bool Enable)
{
	AssetOverride o;

	o.Enabled = Enable;
	PARSE_TOML_MEMBER(Table, ObjectUUIDs);
	PARSE_TOML_MEMBER(Table, ObjectTypes);
	PARSE_TOML_MEMBER(Table, Path);
	PARSE_TOML_MEMBER(Table, Value);

	return o;
}

#undef PARSE_TOML_MEMBER

}
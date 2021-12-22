#pragma once

#include <unordered_set>
#include <string_view>
#include <format>
#include <stdio.h>

#include "../HRZ/Core/RTTI.h"

class RTTIYamlExporter
{
private:
	FILE *m_FileHandle = nullptr;
	std::vector<const HRZ::RTTI *> m_Types;
	const std::string m_GameTypePrefix;
	uint8_t m_IndentSpaces = 0;

public:
	RTTIYamlExporter() = delete;
	RTTIYamlExporter(const RTTIYamlExporter&) = delete;
	RTTIYamlExporter(const std::unordered_set<const HRZ::RTTI *>& Types, const std::string_view GameTypePrefix);
	RTTIYamlExporter& operator=(const RTTIYamlExporter&) = delete;

	void ExportRTTITypes(const std::string_view Directory);

private:
	void ExportGGRTTI();
	void IncreaseIndent();
	void DecreaseIndent();

	template<typename... TArgs>
	void Print(const std::string_view Format, TArgs&&... Args)
	{
		char buffer[2048];
		*std::format_to_n(buffer, std::size(buffer) - 1, Format, std::forward<TArgs>(Args)...).out = '\0';

		char indents[512]{};
		for (uint8_t i = 0; i < m_IndentSpaces; i++)
			indents[i] = ' ';

		fprintf(m_FileHandle, "%s%s\n", indents, buffer);
	}

	static std::string EscapeString(std::string Value);
};
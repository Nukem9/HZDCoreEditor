#pragma once

#include <string_view>
#include <format>
#include <unordered_set>
#include <stdio.h>

#include "../HRZ/Core/RTTI.h"

class RTTICSharpExporter
{
private:
	FILE *m_FileHandle = nullptr;
	std::vector<const HRZ::RTTI *> m_Types;
	const std::string m_GameTypePrefix;

public:
	RTTICSharpExporter() = delete;
	RTTICSharpExporter(const RTTICSharpExporter&) = delete;
	RTTICSharpExporter(const std::unordered_set<const HRZ::RTTI *>& Types, const std::string_view GameTypePrefix);
	RTTICSharpExporter& operator=(const RTTICSharpExporter&) = delete;

	void ExportAll(const std::string_view Directory);

private:
	void ExportFileHeader();
	void ExportFileFooter();
	void ExportRTTIEnum(const HRZ::RTTIEnum *Type);
	void ExportRTTIClass(const HRZ::RTTIClass *Type);

	template<typename... TArgs>
	void Print(const std::string_view Format, TArgs&&... Args)
	{
		char buffer[2048];
		*std::format_to_n(buffer, std::size(buffer) - 1, Format, std::forward<TArgs>(Args)...).out = '\0';

		fputs(buffer, m_FileHandle);
	}

	static bool IsBaseClassSuperfluous(const HRZ::RTTIClass *Type);
	static bool IsMemberNameDuplicated(const HRZ::RTTIClass *Type, const HRZ::RTTIClass::MemberEntry *MemberInfo);
	static std::string_view EnumTypeToString(const HRZ::RTTIEnum *Type);
	static void FilterMemberNameString(std::string& Name);
};
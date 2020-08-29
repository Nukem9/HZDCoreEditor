#pragma once

#include "common.h"
#include "RTTI.h"

namespace RTTICSharpExporter
{
	// This struct DOESN'T match the game, it just needs to be 72 bytes in size
	struct SorterEntry
	{
		const RTTIMemberTypeInfo *m_Type;
		const char *m_Category;
		uint32_t m_Offset;
		bool m_TopLevel;
		char _pad0[0x31];
	};
	static_assert(sizeof(SorterEntry) == 0x48);

	void ExportAll(const char *Directory);
	void ExportFileHeader(FILE *F);
	void ExportFileFooter(FILE *F);
	void ExportRTTIEnum(FILE *F, const RTTI *Type);
	void ExportRTTIClass(FILE *F, const RTTI *Type);

	const char *EnumTypeToString(const RTTI *Type);
	void FilterMemberNameString(std::string& Name);
	bool IsBaseClassSuperfluous(const RTTI *Type);
	bool IsPostLoadCallbackEnabled(const RTTI *Type);
	bool IsMemberNameDuplicated(const RTTI *Type, const RTTIMemberTypeInfo *MemberInfo);
	void BuildFullClassMemberLayout(const RTTI *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel);
	std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> GetSortedClassMembers(const RTTI *Type);
}
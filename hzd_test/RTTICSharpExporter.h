#pragma once

#include "common.h"
#include "HRZ/Core/RTTI.h"

namespace RTTICSharpExporter
{
	using namespace HRZ;

	void ExportAll(const char *Directory);
	void ExportFileHeader(FILE *F);
	void ExportFileFooter(FILE *F);
	void ExportRTTIEnum(FILE *F, const RTTIEnum *Type);
	void ExportRTTIClass(FILE *F, const RTTIClass *Type);

	bool IsBaseClassSuperfluous(const RTTIClass *Type);
	bool IsMemberNameDuplicated(const RTTIClass *Type, const RTTIClass::MemberEntry *MemberInfo);
	const char *EnumTypeToString(const RTTIEnum *Type);
	void FilterMemberNameString(std::string& Name);
}
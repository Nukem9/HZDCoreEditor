#pragma once

#include "common.h"
#include "HRZ/Core/GGRTTI.h"

namespace RTTICSharpExporter
{
	using namespace HRZ;

	void ExportAll(const char *Directory);
	void ExportFileHeader(FILE *F);
	void ExportFileFooter(FILE *F);
	void ExportRTTIEnum(FILE *F, const GGRTTIEnum *Type);
	void ExportRTTIClass(FILE *F, const GGRTTIClass *Type);

	bool IsBaseClassSuperfluous(const GGRTTIClass *Type);
	bool IsMemberNameDuplicated(const GGRTTIClass *Type, const GGRTTIClass::MemberEntry *MemberInfo);
	const char *EnumTypeToString(const GGRTTIEnum *Type);
	void FilterMemberNameString(std::string& Name);
}
#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class GGRTTI;

class ExportedSymbolMember
{
public:
	enum MemberType : uint32_t
	{
		MEMBER_TYPE_SIMPLE = 0,
		MEMBER_TYPE_ENUM = 1,
		MEMBER_TYPE_CLASS = 2,
		MEMBER_TYPE_STRUCT = 3,
		MEMBER_TYPE_TYPEDEF = 4,
		MEMBER_TYPE_FUNCTION = 5,
		MEMBER_TYPE_VARIABLE = 6,
		MEMBER_TYPE_CONTAINER = 7,
		MEMBER_TYPE_SOURCEFILE = 8,
	};

	class LanguageInfo
	{
	public:
		class SignaturePart
		{
		public:
			String m_BaseType;
			String m_Modifiers;
			void *m_Unknown;
		};
		assert_size(SignaturePart, 0x18);

		void *m_Address;
		const char *m_Name;
		const char *m_IncludeName;
		char _pad0[0x8];
		Array<SignaturePart> m_Signature;
		char _pad1[0x40];
	};
	assert_offset(LanguageInfo, m_Address, 0x0);
	assert_offset(LanguageInfo, m_Name, 0x8);
	assert_offset(LanguageInfo, m_IncludeName, 0x10);
	assert_offset(LanguageInfo, m_Signature, 0x20);
	assert_size(LanguageInfo, 0x70);

	MemberType m_Type;
	char _pad0[0x4];
	const GGRTTI *m_TypeInfo;
	const char *m_SymbolNamespace;
	const char *m_SymbolName;
	char _pad1[0x8];
	LanguageInfo m_Infos[3];
};
assert_offset(ExportedSymbolMember, m_Type, 0x0);
assert_offset(ExportedSymbolMember, m_TypeInfo, 0x8);
assert_offset(ExportedSymbolMember, m_SymbolNamespace, 0x10);
assert_offset(ExportedSymbolMember, m_SymbolName, 0x18);
assert_offset(ExportedSymbolMember, m_Infos, 0x28);
assert_size(ExportedSymbolMember, 0x178);

class ExportedSymbolGroup
{
public:
	bool m_AlwaysExport;
	const char *m_Namespace;
	Array<ExportedSymbolMember> m_Members;
	Array<const GGRTTI *> m_Dependents;

	virtual const GGRTTI *GetRTTI() const;
	virtual ~ExportedSymbolGroup();
	virtual void RegisterSymbols() = 0;
};
assert_offset(ExportedSymbolGroup, m_AlwaysExport, 0x8);
assert_offset(ExportedSymbolGroup, m_Namespace, 0x10);
assert_offset(ExportedSymbolGroup, m_Members, 0x18);
assert_offset(ExportedSymbolGroup, m_Dependents, 0x28);
assert_size(ExportedSymbolGroup, 0x38);

}
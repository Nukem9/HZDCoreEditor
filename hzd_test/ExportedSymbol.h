#pragma once

#include "common.h"
#include "PCore.h"
#include "RTTI.h"

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

		void *m_Address;
		const char *m_Name;
		char _pad0[0x10];
		Array<SignaturePart> m_Signatures;
		char _pad1[0x40];
	};

	MemberType m_Type;
	char _pad0[0x4];
	RTTI *m_TypeInfo;
	const char *m_SymbolNamespace;
	const char *m_SymbolName;
	char _pad1[0x8];
	LanguageInfo m_Infos[3];
};
static_assert(sizeof(ExportedSymbolMember::LanguageInfo::SignaturePart) == 0x18);

static_assert_offset(ExportedSymbolMember::LanguageInfo, m_Address, 0x0);
static_assert_offset(ExportedSymbolMember::LanguageInfo, m_Name, 0x8);
static_assert_offset(ExportedSymbolMember::LanguageInfo, m_Signatures, 0x20);
static_assert(sizeof(ExportedSymbolMember::LanguageInfo) == 0x70);

static_assert_offset(ExportedSymbolMember, m_Type, 0x0);
static_assert_offset(ExportedSymbolMember, m_TypeInfo, 0x8);
static_assert_offset(ExportedSymbolMember, m_SymbolNamespace, 0x10);
static_assert_offset(ExportedSymbolMember, m_SymbolName, 0x18);
static_assert_offset(ExportedSymbolMember, m_Infos, 0x28);
static_assert(sizeof(ExportedSymbolMember) == 0x178);

class ExportedSymbolGroup
{
public:
	virtual RTTI *GetRTTI();
	virtual ~ExportedSymbolGroup();
	virtual void RegisterSymbols() = 0;

	bool m_AlwaysExport;
	const char *m_Namespace;
	Array<ExportedSymbolMember> m_Members;
	Array<RTTI *> m_Dependents;
};
static_assert_offset(ExportedSymbolGroup, m_AlwaysExport, 0x8);
static_assert_offset(ExportedSymbolGroup, m_Namespace, 0x10);
static_assert_offset(ExportedSymbolGroup, m_Members, 0x18);
static_assert_offset(ExportedSymbolGroup, m_Dependents, 0x28);
static_assert(sizeof(ExportedSymbolGroup) == 0x38);
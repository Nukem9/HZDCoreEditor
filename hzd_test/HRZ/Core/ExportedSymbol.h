#pragma once

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(ExportedSymbolGroup);

class ExportedSymbolMember
{
public:
	enum class MemberType : uint32_t
	{
		Simple = 0,
		Enum = 1,
		Class = 2,
		Struct = 3,
		Typedef = 4,
		Function = 5,
		Variable = 6,
		Container = 7,
		SourceFile = 8,
	};

	class LanguageInfo
	{
	public:
		class SignaturePart
		{
		public:
			String m_BaseType;
			String m_Modifiers;
#if 1 // if (Horizon Zero Dawn)
			char _pad10[0x8];
#else // if (Death Stranding)
			char _pad10[0x18];
#endif
		};
#if 1 // if (Horizon Zero Dawn)
		assert_size(SignaturePart, 0x18);
#else // if (Death Stranding)
		assert_size(SignaturePart, 0x28);
#endif

		void *m_Address;
		const char *m_Name;
		const char *m_IncludeName;
		char _pad0[0x8];
		Array<SignaturePart> m_Signature;
#if 1 // if (Horizon Zero Dawn)
		char _pad30[0x40];
#else // if (Death Stranding)
		char _pad30[0x10];
#endif
	};
	assert_offset(LanguageInfo, m_Address, 0x0);
	assert_offset(LanguageInfo, m_Name, 0x8);
	assert_offset(LanguageInfo, m_IncludeName, 0x10);
	assert_offset(LanguageInfo, m_Signature, 0x20);
#if 1 // if (Horizon Zero Dawn)
	assert_size(LanguageInfo, 0x70);
#else // if (Death Stranding)
	assert_size(LanguageInfo, 0x40);
#endif

	MemberType m_Type;
	const RTTI *m_TypeInfo;
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
#if 1 // if (Horizon Zero Dawn)
assert_size(ExportedSymbolMember, 0x178);
#else // if (Death Stranding)
assert_size(ExportedSymbolMember, 0xE8);
#endif

class ExportedSymbolGroup
{
public:
	TYPE_RTTI(ExportedSymbolGroup);

	bool m_AlwaysExport;
	const char *m_Namespace;
	Array<ExportedSymbolMember> m_Members;
	Array<const RTTI *> m_Dependents;

	virtual const RTTI *GetRTTI() const;
	virtual ~ExportedSymbolGroup();
	virtual void RegisterSymbols() = 0;
};
assert_offset(ExportedSymbolGroup, m_AlwaysExport, 0x8);
assert_offset(ExportedSymbolGroup, m_Namespace, 0x10);
assert_offset(ExportedSymbolGroup, m_Members, 0x18);
assert_offset(ExportedSymbolGroup, m_Dependents, 0x28);
assert_size(ExportedSymbolGroup, 0x38);

}
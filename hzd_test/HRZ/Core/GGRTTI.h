#pragma once

#include <span>
#include <string>
#include <vector>

#include "../PCore/Common.h"

namespace HRZ
{

class GGRTTIPrimitive;
class GGRTTIContainer;
class GGRTTIEnum;
class GGRTTIClass;
class GGRTTIPOD;

class GGRTTI
{
public:
	enum TypeId : uint16_t
	{
		InvalidTypeId = 0xFFFF,
	};

	enum InfoType : uint8_t
	{
		INFO_TYPE_PRIMITIVE = 0,
		INFO_TYPE_REFERENCE = 1,
		INFO_TYPE_CONTAINER = 2,
		INFO_TYPE_ENUM = 3,
		INFO_TYPE_CLASS = 4,
		INFO_TYPE_ENUM_2 = 5,
		INFO_TYPE_POD = 6,
	};

	TypeId m_RuntimeTypeId1;
	TypeId m_RuntimeTypeId2;
	InfoType m_InfoType;

	union
	{
		uint8_t m_EnumUnderlyingTypeSize;	// +0x5 If enum: determines the underlying type size
		uint8_t m_ClassInheritanceCount;	// +0x5 If class: determines number of entries for m_InheritanceData
	};

	union
	{
		uint16_t m_EnumMemberCount;			// +0x6 If enum: determines number of entries for m_Values

		struct
		{
			uint8_t m_ClassMemberCount;		// +0x6 If class: determines number of entries for m_MemberData
			uint8_t m_LuaFunctionCount;		// +0x7 If class: determines the number of entries for lua functions
		};
	};

	bool IsExactKindOf(const GGRTTI *Other) const;
	bool IsKindOf(const GGRTTI *Other) const;

	const GGRTTIContainer *AsContainer() const;
	const GGRTTIEnum *AsEnum() const;
	const GGRTTIClass *AsClass() const;

	const GGRTTI *GetContainedType() const;
	std::string_view GetRTTITypeName() const;
	std::string GetSymbolName() const;
	uint64_t GetCoreBinaryTypeId() const;
};
assert_offset(GGRTTI, m_InfoType, 0x4);
assert_offset(GGRTTI, m_EnumUnderlyingTypeSize, 0x5);
assert_offset(GGRTTI, m_ClassInheritanceCount, 0x5);
assert_offset(GGRTTI, m_EnumMemberCount, 0x6);
assert_offset(GGRTTI, m_ClassMemberCount, 0x6);
assert_offset(GGRTTI, m_LuaFunctionCount, 0x7);

class GGRTTIPrimitive : public GGRTTI
{
public:
	// Type 0 = Primitive (float, int, bool, String)
	char _pad0[0x8];			// +0x8
	const char *m_Name;			// +0x10
	GGRTTI *m_ParentType;		// +0x18
	void *m_DeserializeString;	// +0x20
	void *m_SerializeString;	// +0x28
	void *m_SwapValues;			// +0x30
	void *m_TestEqualityValues;	// +0x38
	void *m_Constructor;		// +0x40
	void *m_UnknownFunction1;	// +0x48
	void *m_SwapEndianness;		// +0x50
	void *m_AssignValues;		// +0x58
	void *m_GetSize;			// +0x60
	void *m_CompareByStrings;	// +0x68
	void *m_UnknownFunction2;	// +0x70
};
assert_offset(GGRTTIPrimitive, m_Name, 0x10);
assert_offset(GGRTTIPrimitive, m_ParentType, 0x18);
assert_offset(GGRTTIPrimitive, m_DeserializeString, 0x20);
assert_offset(GGRTTIPrimitive, m_UnknownFunction1, 0x48);
assert_offset(GGRTTIPrimitive, m_CompareByStrings, 0x68);
assert_offset(GGRTTIPrimitive, m_UnknownFunction2, 0x70);
assert_size(GGRTTIPrimitive, 0x78);

class GGRTTIContainer : public GGRTTI
{
public:
	class ContainerData
	{
	public:
		const char *m_Name;
	};

	// Type 1 = Reference/Pointer (UUIDRef<>, StreamingRef<>, WeakPtr<>)
	// Type 2 = Container (Array<float>)
	GGRTTI *m_Type;			// +0x8
	ContainerData *m_Data;	// +0x10
};
assert_offset(GGRTTIContainer, m_Type, 0x8);
assert_offset(GGRTTIContainer, m_Data, 0x10);
assert_size(GGRTTIContainer, 0x18);

class GGRTTIEnum : public GGRTTI
{
public:
	class EnumEntry
	{
	public:
		uint32_t m_Value;
		const char *m_Name;
		char _pad0[0x18];
	};
	assert_offset(EnumEntry, m_Value, 0x0);
	assert_offset(EnumEntry, m_Name, 0x8);
	assert_size(EnumEntry, 0x28);

	// Type 3 = Enum
	// Type 5 = Enum
	char _pad0[0x8];		// +0x8
	const char *m_Name;		// +0x10
	EnumEntry *m_Values;	// +0x18
	char _pad1[0x8];		// +0x20

	auto EnumMembers() const
	{
		return std::span{ m_Values, m_EnumMemberCount };
	}
};
assert_offset(GGRTTIEnum, m_Name, 0x10);
assert_offset(GGRTTIEnum, m_Values, 0x18);
assert_size(GGRTTIEnum, 0x28);

class GGRTTIClass : public GGRTTI
{
public:
	using ConstructFunctionPfn = void *(*)(void *, void *);
	using DestructFunctionPfn = void(*)(void *, void *);
	using DeserializeFromStringPfn = void(*)();
	using SerializeToStringPfn = void(*)();
	using ExportedSymbolsGetterPfn = const GGRTTI *(*)();

	class InheritanceEntry
	{
	public:
		GGRTTI *m_Type;
		uint32_t m_Offset;
	};
	assert_size(InheritanceEntry, 0x10);

	class MemberEntry
	{
	public:
		using PropertyValuePfn = void(*)(void *, void *);

		enum Flags : uint8_t
		{
			SAVE_STATE_ONLY = 2,
		};

		GGRTTI *m_Type;
		uint16_t m_Offset;
		Flags m_Flags;
		const char *m_Name;
		PropertyValuePfn m_PropertyGetter;
		PropertyValuePfn m_PropertySetter;
		char _pad0[0x10];

		bool IsGroupMarker() const;
		bool IsSaveStateOnly() const;
	};
	assert_offset(MemberEntry, m_Type, 0x0);
	assert_offset(MemberEntry, m_Offset, 0x8);
	assert_offset(MemberEntry, m_Flags, 0xA);
	assert_offset(MemberEntry, m_Name, 0x10);
	assert_offset(MemberEntry, m_PropertyGetter, 0x18);
	assert_offset(MemberEntry, m_PropertySetter, 0x20);
	assert_size(MemberEntry, 0x38);

	class LuaFunctionEntry
	{
	public:
		char m_ReturnValueType;
		const char *m_Name;
		const char *m_ArgumentString;
		void *m_Function;
	};
	static_assert(sizeof(LuaFunctionEntry) == 0x20);

	class MessageHandlerEntry
	{
	public:
		GGRTTI *m_Type;		// MsgReadBinary/MsgInit/MsgXXX
		void *m_Callback;	// Handler
	};
	assert_size(MessageHandlerEntry, 0x10);

	class InheritedMessageEntry
	{
	public:
		bool m_Unknown;
		GGRTTI *m_Type;
		GGRTTI *m_ClassType;
	};
	assert_size(InheritedMessageEntry, 0x18);

	// Type 4 = Class declaration
	uint8_t m_MessageHandlerCount;						// +0x8 Determines number of entries for m_MessageHandlerTable
	uint8_t m_InheritedMessageCount;					// +0x9 Determines number of entries for m_InheritedMessageHandlerTable
	char _pad0[0x2];									// +0xA
	uint16_t m_UnknownC;								// +0xC
	char _pad1[0x2];									// +0xE
	uint32_t m_Size;									// +0x10
	uint16_t m_Alignment;								// +0x14
	uint16_t m_Flags;									// +0x16
	ConstructFunctionPfn m_Constructor;					// +0x18
	DestructFunctionPfn m_Destructor;					// +0x20
	DeserializeFromStringPfn m_DeserializeString;		// +0x28
	SerializeToStringPfn m_SerializeString;				// +0x30
	const char *m_Name;									// +0x38
	char _pad2[0x18];									// +0x40
	InheritanceEntry *m_InheritanceTable;				// +0x58
	MemberEntry *m_MemberTable;							// +0x60
	LuaFunctionEntry *m_LuaFunctionTable;				// +0x68
	MessageHandlerEntry *m_MessageHandlerTable;			// +0x70
	InheritedMessageEntry *m_InheritedMessageTable;		// +0x78
	ExportedSymbolsGetterPfn m_GetExportedSymbols;		// +0x80

	auto ClassInheritance() const
	{
		return std::span{ m_InheritanceTable, m_ClassInheritanceCount };
	}

	auto ClassMembers() const
	{
		return std::span{ m_MemberTable, m_ClassMemberCount };
	}

	auto ClassLuaFunctions() const
	{
		return std::span{ m_LuaFunctionTable, m_LuaFunctionCount };
	}

	auto ClassMessageHandlers() const
	{
		return std::span{ m_MessageHandlerTable, m_MessageHandlerCount };
	}

	auto ClassInheritedMessages() const
	{
		return std::span{ m_InheritedMessageTable, m_InheritedMessageCount };
	}

	bool IsPostLoadCallbackEnabled() const;
	std::vector<std::tuple<const MemberEntry *, const char *, size_t>> GetCategorizedClassMembers() const;

private:
	struct SorterEntry
	{
		size_t m_DeclOrder;
		const MemberEntry *m_Type;
		const char *m_Category;
		uint32_t m_Offset;
		bool m_TopLevel;
	};

	static void BuildFullClassMemberLayout(const GGRTTIClass *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel);
};
assert_offset(GGRTTIClass, m_MessageHandlerCount, 0x8);
assert_offset(GGRTTIClass, m_Size, 0x10);
assert_offset(GGRTTIClass, m_Constructor, 0x18);
assert_offset(GGRTTIClass, m_Name, 0x38);
assert_offset(GGRTTIClass, m_InheritanceTable, 0x58);
assert_offset(GGRTTIClass, m_MemberTable, 0x60);
assert_offset(GGRTTIClass, m_GetExportedSymbols, 0x80);

class GGRTTIPOD : public GGRTTI
{
public:
	// Type 6 = Plain old data. Doesn't exist in static RTTI. They're generated at runtime by combining fields for optimization purposes.
	uint32_t m_Size; // +0x8
};
assert_offset(GGRTTIPOD, m_Size, 0x8);

}
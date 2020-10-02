#pragma once

#include <span>
#include "common.h"

class GGRTTIPrimitive;
class GGRTTIContainer;
class GGRTTIEnum;
class GGRTTIClass;
class GGRTTIPOD;

class GGRTTI
{
public:
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

	uint16_t m_RuntimeTypeId1;
	uint16_t m_RuntimeTypeId2;
	InfoType m_InfoType;

	union
	{
		uint8_t m_EnumUnderlyingTypeSize;	// +0x5 If enum: determines the underlying type size
		uint8_t m_ClassInheritanceCount;	// +0x5 If class: determines number of entries for m_InheritanceData
	};

	union
	{
		uint16_t m_EnumMemberCount;			// +0x6 If enum: determines number of entries for m_Values
		uint8_t m_ClassMemberCount;			// +0x6 If class: determines number of entries for m_MemberData
	};

	const GGRTTIContainer *AsContainer() const;
	const GGRTTIEnum *AsEnum() const;
	const GGRTTIClass *AsClass() const;

	const GGRTTI *GetContainedType() const;
	std::string GetSymbolName() const;
	uint64_t GetCoreBinaryTypeId() const;
};
static_assert_offset(GGRTTI, m_InfoType, 0x4);
static_assert_offset(GGRTTI, m_EnumUnderlyingTypeSize, 0x5);
static_assert_offset(GGRTTI, m_ClassInheritanceCount, 0x5);
static_assert_offset(GGRTTI, m_EnumMemberCount, 0x6);
static_assert_offset(GGRTTI, m_ClassMemberCount, 0x6);

class GGRTTIPrimitive : public GGRTTI
{
public:
	// Type 0 = Primitive (float, int, bool, String)
	char _pad0[0x8];	// +0x8
	const char *m_Name;	// +0x10
};
static_assert_offset(GGRTTIPrimitive, m_Name, 0x10);

class GGRTTIContainer : public GGRTTI
{
public:
	// Type 1 = Reference/Pointer (UUIDRef<>, StreamingRef<>, WeakPtr<>)
	// Type 2 = Container (Array<float>)
	GGRTTI *m_Type;					// +0x8
	const char **m_ContainerName;	// +0x10 This is actually a pointer to another struct
};
static_assert_offset(GGRTTIContainer, m_Type, 0x8);
static_assert_offset(GGRTTIContainer, m_ContainerName, 0x10);

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
	static_assert(sizeof(EnumEntry) == 0x28);

	// Type 3 = Enum
	// Type 5 = Enum
	char _pad0[0x8];		// +0x8
	const char *m_Name;		// +0x10
	EnumEntry *m_Values;	// +0x18

	auto EnumMembers() const
	{
		return std::span{ m_Values, m_EnumMemberCount };
	}
};
static_assert_offset(GGRTTIEnum::EnumEntry, m_Value, 0x0);
static_assert_offset(GGRTTIEnum::EnumEntry, m_Name, 0x8);

static_assert_offset(GGRTTIEnum, m_Name, 0x10);
static_assert_offset(GGRTTIEnum, m_Values, 0x18);

class GGRTTIClass : public GGRTTI
{
public:
	using ConstructFunctionPfn = void *(*)(void *, void *);
	using DestructFunctionPfn = void(*)(void *, void *);
	using DeserializeFromStringPfn = void(*)();
	using SerializeToStringPfn = void(*)();

	class InheritanceEntry
	{
	public:
		GGRTTI *m_Type;
		uint32_t m_Offset;
	};
	static_assert(sizeof(InheritanceEntry) == 0x10);

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
	static_assert(sizeof(MemberEntry) == 0x38);

	class MessageHandlerEntry
	{
	public:
		GGRTTI *m_Type;		// MsgReadBinary/MsgInit/MsgXXX
		void *m_Callback;	// Handler
	};
	static_assert(sizeof(MessageHandlerEntry) == 0x10);

	// Type 4 = Class declaration
	uint8_t m_EventSubscriptionCount;			// +0x8 Determines number of entries for m_EventSubscriptions
	char _pad0[0x7];							// +0x9
	uint32_t m_Size;							// +0x10
	uint32_t m_Alignment;						// +0x14
	ConstructFunctionPfn m_Constructor;			// +0x18
	DestructFunctionPfn m_Destructor;			// +0x20
	DeserializeFromStringPfn m_Deserialize;		// +0x28
	SerializeToStringPfn m_Serialize;			// +0x30
	const char *m_Name;							// +0x38
	char _pad1[0x18];
	InheritanceEntry *m_InheritanceData;		// +0x58
	MemberEntry *m_MemberData;					// +0x60
	char _pad2[0x8];
	MessageHandlerEntry *m_MessageHandlerData;	// +0x70

	auto ClassInheritance() const
	{
		return std::span{ m_InheritanceData, m_ClassInheritanceCount };
	}

	auto ClassMembers() const
	{
		return std::span{ m_MemberData, m_ClassMemberCount };
	}

	auto ClassEventSubscriptions() const
	{
		return std::span{ m_MessageHandlerData, m_EventSubscriptionCount };
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
static_assert_offset(GGRTTIClass::InheritanceEntry, m_Type, 0x0);
static_assert_offset(GGRTTIClass::InheritanceEntry, m_Offset, 0x8);

static_assert_offset(GGRTTIClass::MemberEntry, m_Type, 0x0);
static_assert_offset(GGRTTIClass::MemberEntry, m_Offset, 0x8);
static_assert_offset(GGRTTIClass::MemberEntry, m_Flags, 0xA);
static_assert_offset(GGRTTIClass::MemberEntry, m_Name, 0x10);
static_assert_offset(GGRTTIClass::MemberEntry, m_PropertyGetter, 0x18);
static_assert_offset(GGRTTIClass::MemberEntry, m_PropertySetter, 0x20);

static_assert_offset(GGRTTIClass::MessageHandlerEntry, m_Type, 0x0);
static_assert_offset(GGRTTIClass::MessageHandlerEntry, m_Callback, 0x8);

static_assert_offset(GGRTTIClass, m_EventSubscriptionCount, 0x8);
static_assert_offset(GGRTTIClass, m_InheritanceData, 0x58);
static_assert_offset(GGRTTIClass, m_MemberData, 0x60);

class GGRTTIPOD : public GGRTTI
{
public:
	// Type 6 = Plain old data. Doesn't exist in static RTTI. They're generated at runtime by combining fields for optimization purposes.
	uint32_t m_Size; // +0x8
};
static_assert_offset(GGRTTIPOD, m_Size, 0x8);
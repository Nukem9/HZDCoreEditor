#pragma once

#include <span>
#include "common.h"

class RTTIEnumTypeInfo;
class RTTIInheritanceTypeInfo;
class RTTIMemberTypeInfo;
class RTTIClassEventInfo;
class RTTI;

class RTTIEnumTypeInfo
{
public:
	uint32_t m_Value;
	const char *m_Name;
	char _pad0[0x18];
};
static_assert(sizeof(RTTIEnumTypeInfo) == 0x28);
static_assert_offset(RTTIEnumTypeInfo, m_Value, 0x0);
static_assert_offset(RTTIEnumTypeInfo, m_Name, 0x8);

class RTTIInheritanceTypeInfo
{
public:
	RTTI *m_Type;
	uint32_t m_Offset;
};
static_assert(sizeof(RTTIInheritanceTypeInfo) == 0x10);
static_assert_offset(RTTIInheritanceTypeInfo, m_Type, 0x0);
static_assert_offset(RTTIInheritanceTypeInfo, m_Offset, 0x8);

class RTTIMemberTypeInfo
{
public:
	using PropertyValuePfn = void(*)(void *, void *);

	enum Flags : uint8_t
	{
		SAVE_STATE_ONLY = 2,
	};

	RTTI *m_Type;
	uint16_t m_Offset;
	Flags m_Flags;
	const char *m_Name;
	PropertyValuePfn m_PropertyGetter;
	PropertyValuePfn m_PropertySetter;
	char _pad0[0x10];

	bool IsGroupMarker() const
	{
		return m_Type == nullptr;
	}

	bool IsSaveStateOnly() const
	{
		return IsGroupMarker() || (m_Flags & SAVE_STATE_ONLY) != 0;
	}
};
static_assert(sizeof(RTTIMemberTypeInfo) == 0x38);
static_assert_offset(RTTIMemberTypeInfo, m_Type, 0x0);
static_assert_offset(RTTIMemberTypeInfo, m_Offset, 0x8);
static_assert_offset(RTTIMemberTypeInfo, m_Flags, 0xA);
static_assert_offset(RTTIMemberTypeInfo, m_Name, 0x10);
static_assert_offset(RTTIMemberTypeInfo, m_PropertyGetter, 0x18);
static_assert_offset(RTTIMemberTypeInfo, m_PropertySetter, 0x20);

class RTTIClassEventInfo
{
public:
	RTTI *m_Type;		// MsgReadBinary/MsgInit/MsgXXX
	void *m_Callback;	// Handler
};
static_assert(sizeof(RTTIClassEventInfo) == 0x10);
static_assert_offset(RTTIClassEventInfo, m_Type, 0x0);
static_assert_offset(RTTIClassEventInfo, m_Callback, 0x8);

class RTTI
{
public:
	using ConstructFunctionPfn = void *(*)(void *, void *);
	using DestructFunctionPfn = void(*)(void *, void *);
	using DeserializeFromStringPfn = void(*)();
	using SerializeToStringPfn = void(*)();

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

	uint16_t m_UnknownRuntimeId1;
	uint16_t m_UnknownRuntimeId2;
	InfoType m_InfoType;

	union
	{
		uint8_t m_EnumUnderlyingTypeSize;	// If enum: determines the underlying type size
		uint8_t m_ClassInheritanceCount;	// If class: determines number of entries for m_InheritanceData
	};

	union
	{
		uint16_t m_EnumMemberCount;			// If enum: determines number of entries for m_Values
		uint8_t m_ClassMemberCount;			// If class: determines number of entries for m_MemberData
	};

	union
	{
		// Type 0 = Primitive (float, int, bool, String)
		struct
		{
			char _pad0[0x8];							// +0x8
			const char *m_Name;							// +0x10
		} Primitive;

		// Type 1 = Reference/Pointer (UUIDRef<>, StreamingRef<>, WeakPtr<>)
		// Type 2 = Container (Array<float>)
		struct
		{
			RTTI *m_Type;								// +0x8
			const char **m_ContainerName;				// +0x10 This is actually a pointer to another struct
		} ContainerRef;

		// Type 3 = Enum
		// Type 5 = Enum
		struct
		{
			char _pad0[0x8];							// +0x8
			const char *m_Name;							// +0x10
			RTTIEnumTypeInfo *m_Values;					// +0x18
		} Enum;

		// Type 4 = Class declaration
		struct
		{
			uint8_t m_EventSubscriptionCount;			// +0x8
			char _pad0[0x7];							// +0x9
			uint32_t m_Size;							// +0x10
			uint32_t m_Alignment;						// +0x14
			ConstructFunctionPfn m_Constructor;			// +0x18
			DestructFunctionPfn m_Destructor;			// +0x20
			DeserializeFromStringPfn m_Deserialize;		// +0x28
			SerializeToStringPfn m_Serialize;			// +0x30
			const char *m_Name;							// +0x38
			char _pad1[0x18];
			RTTIInheritanceTypeInfo *m_InheritanceData;	// +0x58
			RTTIMemberTypeInfo *m_MemberData;			// +0x60
			char _pad2[0x8];
			RTTIClassEventInfo *m_EventSubscriptions;	// +0x70
		} Class;

		// Type 6 = Plain old data. Doesn't exist in static RTTI. They're generated at runtime by combining fields for optimization purposes.
		struct
		{
			uint32_t m_Size;							// +0x8
		} POD;
	};

	auto EnumMembers() const
	{
		return std::span{ Enum.m_Values, m_EnumMemberCount };
	}

	auto ClassInheritance() const
	{
		return std::span{ Class.m_InheritanceData, m_ClassInheritanceCount };
	}

	auto ClassMembers() const
	{
		return std::span{ Class.m_MemberData, m_ClassMemberCount };
	}

	auto ClassEventSubscriptions() const
	{
		return std::span{ Class.m_EventSubscriptions, Class.m_EventSubscriptionCount };
	}

	const RTTI *GetContainedType() const
	{
		switch (m_InfoType)
		{
		case INFO_TYPE_REFERENCE:
		case INFO_TYPE_CONTAINER:
			return ContainerRef.m_Type;
		}

		return this;
	}

	std::string GetSymbolName() const
	{
		switch (m_InfoType)
		{
		case INFO_TYPE_PRIMITIVE:
			return Primitive.m_Name;

		case INFO_TYPE_REFERENCE:
		case INFO_TYPE_CONTAINER:
			char refType[1024];

			if (!strcmp(*ContainerRef.m_ContainerName, "cptr"))
				sprintf_s(refType, "CPtr<%s>", ContainerRef.m_Type->GetSymbolName().c_str());
			else
				sprintf_s(refType, "%s<%s>", *ContainerRef.m_ContainerName, ContainerRef.m_Type->GetSymbolName().c_str());

			return refType;

		case INFO_TYPE_ENUM:
		case INFO_TYPE_ENUM_2:
			return Enum.m_Name;

		case INFO_TYPE_CLASS:
			return Class.m_Name;

		case INFO_TYPE_POD:
			char podType[16];
			sprintf_s(podType, "POD%d", POD.m_Size);

			return podType;
		}

		return "";
	}

	uint64_t GetCoreBinaryTypeId_UNSAFE() const
	{
		uint64_t hashedData[2] = {};

#if HORIZON_ZERO_DAWN
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
#elif DEATH_STRANDING
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");
#endif

		((void(__fastcall *)(uint64_t *, const RTTI *, __int64))(addr))(hashedData, this, 2);

		return hashedData[0];
	}

	bool IsPostLoadCallbackEnabled() const
	{
		if (m_InfoType != RTTI::INFO_TYPE_CLASS)
			__debugbreak();

		for (auto& event : ClassEventSubscriptions())
		{
			if (event.m_Type->GetSymbolName() == "MsgReadBinary")
				return true;
		}

		return false;
	}

private:
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

	static void BuildFullClassMemberLayout(const RTTI *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel)
	{
		const char *activeCategory = "";

		for (auto& base : Type->ClassInheritance())
			BuildFullClassMemberLayout(base.m_Type, Members, Offset + base.m_Offset, false);

		for (auto& member : Type->ClassMembers())
		{
			if (!member.m_Type)
				activeCategory = member.m_Name;

			SorterEntry entry
			{
				.m_Type = &member,
				.m_Category = activeCategory,
				.m_Offset = member.m_Offset + Offset,
				.m_TopLevel = TopLevel,
			};

			Members.emplace_back(entry);
		}
	}

public:
	std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> GetSortedClassMembers() const
	{
		// Nasty hack: I don't know how sorting order works with multiple properties at offset 0. Let the game determine it.
		std::vector<SorterEntry> sortedEntries;
		BuildFullClassMemberLayout(this, sortedEntries, 0, true);

		auto sortCompare = [](const SorterEntry *A, const SorterEntry *B)
		{
			return A->m_Offset < B->m_Offset;
		};

		if (sortedEntries.size() > 1)
		{
			auto start = &sortedEntries.data()[0];
			auto end = &sortedEntries.data()[sortedEntries.size() - 1];
			uint32_t temp = 0;

			// Signature is valid across both games. I'm amazed. 9/19/2020.
			const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 6C 24 20 56 41 56 41 57 48 83 EC 20 48 8B 02 4D 8B F9 49 8B E8 48 8B F2 4C 8B F1 48 39 01 0F 83 56 01 00 00 45 69 11 0D 66 19 00 48 B8 39 8E E3 38 8E E3 38 0E");
			((void(__fastcall *)(SorterEntry **, SorterEntry **, bool(__fastcall *)(const SorterEntry *, const SorterEntry *), uint32_t *))(addr))(&start, &end, sortCompare, &temp);
		}

		// We only care about the top-level fields here
		std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> out;

		for (auto& entry : sortedEntries)
		{
			if (entry.m_TopLevel)
				out.emplace_back(entry.m_Type, entry.m_Category);
		}

		return out;
	}
};
static_assert_offset(RTTI, m_InfoType, 0x4);
static_assert_offset(RTTI, m_EnumUnderlyingTypeSize, 0x5);
static_assert_offset(RTTI, m_ClassInheritanceCount, 0x5);
static_assert_offset(RTTI, m_EnumMemberCount, 0x6);
static_assert_offset(RTTI, m_ClassMemberCount, 0x6);
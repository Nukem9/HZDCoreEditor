#pragma once

#include <span>
#include <string>
#include <vector>
#include <optional>
#include <stdexcept>

#include "../PCore/Common.h"

#include "RTTILazyResolver.h"

namespace HRZ
{

#define DECL_RTTI(Type) static const RTTILazyResolver RTTI_##Type = RTTILazyResolver(#Type)
#define TYPE_RTTI(Type) static inline auto& TypeInfo = RTTI_##Type

class RTTI;
class RTTIObject;
class RTTIPrimitive;
class RTTIContainer;
class RTTIEnum;
class RTTIClass;
class RTTIPOD;

class RTTI
{
public:
	enum TypeId : uint16_t
	{
		InvalidTypeId = 0xFFFF,
	};

	enum class InfoType : uint8_t
	{
		Primitive = 0,
		Reference = 1,
		Container = 2,
		Enum = 3,
		Class = 4,
		EnumFlags = 5,
		POD = 6,
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

	bool IsExactKindOf(const RTTI *Other) const;
	bool IsKindOf(const RTTI *Other) const;

	const RTTIPrimitive *AsPrimitive() const;
	const RTTIContainer *AsContainer() const;
	const RTTIEnum *AsEnum() const;
	const RTTIClass *AsClass() const;

	const RTTI *GetContainedType() const;
	std::string_view GetRTTITypeName() const;
	std::string GetSymbolName() const;
	uint64_t GetCoreBinaryTypeId() const;

	std::optional<std::string> SerializeObject(const void *Object) const;
	bool DeserializeObject(void *Object, const String& InText) const;

	template<typename T, typename U>
	static T *Cast(U *Other)
	{
		if (Other->GetRTTI()->IsKindOf(T::TypeInfo))
			return static_cast<T *>(Other);

		return nullptr;
	}

	template<typename T, typename U>
	static T *DynamicCast(U *Other)
	{
		__debugbreak();
		return nullptr;
	}
};
assert_offset(RTTI, m_InfoType, 0x4);
assert_offset(RTTI, m_EnumUnderlyingTypeSize, 0x5);
assert_offset(RTTI, m_ClassInheritanceCount, 0x5);
assert_offset(RTTI, m_EnumMemberCount, 0x6);
assert_offset(RTTI, m_ClassMemberCount, 0x6);
assert_offset(RTTI, m_LuaFunctionCount, 0x7);

class RTTIPrimitive : public RTTI
{
public:
	// Type 0 = Primitive (float, int, bool, String)
	char _pad0[0x8];																// +0x8
	const char *m_Name;																// +0x10
	RTTI *m_ParentType;																// +0x18
	bool (* m_DeserializeString)(const String& InText, void *Object);				// +0x20
	bool (* m_SerializeString)(const void *Object, String& OutText);				// +0x28
	void (* m_AssignValue)(void *Lhs, const void *Rhs);								// +0x30
	bool (* m_TestEquality)(const void *Lhs, const void *Rhs);						// +0x38
	void (* m_Constructor)(void *Unused, void *Object);								// +0x40
	void (* m_Destructor)(void *Unused, void *Object);								// +0x48
	bool (* m_SwapEndianness)(const void *Source, const void *Dest, uint8_t Type);	// +0x50
	bool (* m_TryAssignValue)(void *Lhs, const void *Rhs);							// +0x58 No clue what this is doing for String
	size_t (* m_GetSizeInMemory)(const void *Object);								// +0x60
	bool (* m_CompareByStrings)(const void *Object, const char *, const char *);	// +0x68 Purpose unknown
	void (* m_UnknownFunction)();													// +0x70 Unused?

	std::optional<std::string> SerializeObject(const void *Object) const;
	bool DeserializeObject(void *Object, const String& InText) const;
};
assert_offset(RTTIPrimitive, m_Name, 0x10);
assert_offset(RTTIPrimitive, m_ParentType, 0x18);
assert_offset(RTTIPrimitive, m_DeserializeString, 0x20);
assert_offset(RTTIPrimitive, m_Destructor, 0x48);
assert_offset(RTTIPrimitive, m_CompareByStrings, 0x68);
assert_offset(RTTIPrimitive, m_UnknownFunction, 0x70);
assert_size(RTTIPrimitive, 0x78);

class RTTIContainer : public RTTI
{
public:
	struct Data
	{
		const char *m_Name;
	};

	struct RefData : Data
	{
		char _pad8[0x8];
		void (* m_Constructor)(const RTTIContainer *Type, void *Object);	// 0x10
		void (* m_Destructor)(const RTTIContainer *Type, void *Object);		// 0x18
	};
	assert_offset(RefData, m_Constructor, 0x10);

	struct ContainerData : Data
	{
		char _pad8[0x8];
		void (* m_Constructor)(const RTTIContainer *Type, void *Object);								// 0x10
		void (* m_Destructor)(const RTTIContainer *Type, void *Object);									// 0x18
		char _pad20[0x70];
		bool (* m_SerializeString)(const void *Object, const RTTIContainer *Type, String& OutText);		// +0x90
		bool (* m_DeserializeString)(const String& InText, const RTTIContainer *Type, void *Object);	// +0x98
	};
	assert_offset(ContainerData, m_Constructor, 0x10);
	assert_offset(ContainerData, m_SerializeString, 0x90);

	// Type 1 = Reference/Pointer (Ref<>, UUIDRef<>, StreamingRef<>, WeakPtr<>)
	// Type 2 = Container (Array<float>, HashMap<>, HashSet<>)
	RTTI *m_Type;	// +0x8
	Data *m_Data;	// +0x10

	std::optional<std::string> SerializeObject(const void *Object) const;
	bool DeserializeObject(void *Object, const String& InText) const;
};
assert_offset(RTTIContainer, m_Type, 0x8);
assert_offset(RTTIContainer, m_Data, 0x10);
assert_size(RTTIContainer, 0x18);

class RTTIEnum : public RTTI
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

	std::optional<std::string> SerializeObject(const void *Object) const;
	bool DeserializeObject(void *Object, const String& InText) const;
};
assert_offset(RTTIEnum, m_Name, 0x10);
assert_offset(RTTIEnum, m_Values, 0x18);
assert_size(RTTIEnum, 0x28);

class RTTIClass : public RTTI
{
public:
	using ConstructFunctionPfn = void *(*)(void *, void *);
	using DestructFunctionPfn = void(*)(void *, void *);
	using DeserializeFromStringPfn = bool(*)(void *Object, const String& InText);
	using SerializeToStringPfn = bool(*)(const void *Object, String& OutText);
	using ExportedSymbolsGetterPfn = const RTTI *(*)();

	class InheritanceEntry
	{
	public:
		RTTIClass *m_Type;
		uint32_t m_Offset;
	};
	assert_size(InheritanceEntry, 0x10);

	class MemberEntry
	{
	public:
		enum Flags : uint16_t
		{
			SAVE_STATE_ONLY = 2,
		};

		RTTI *m_Type;
		uint16_t m_Offset;
		Flags m_Flags;
		const char *m_Name;
		void (* m_PropertyGetter)(const void *ClassObject, void *Value);
		void (* m_PropertySetter)(void *ClassObject, const void *Value);
		char _pad0[0x10];

		bool IsGroupMarker() const;
		bool IsSaveStateOnly() const;
		bool IsProperty() const;
	};
	assert_offset(MemberEntry, m_Type, 0x0);
	assert_offset(MemberEntry, m_Offset, 0x8);
	assert_offset(MemberEntry, m_Flags, 0xA);
	assert_offset(MemberEntry, m_Name, 0x10);
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
		RTTI *m_Type;		// MsgReadBinary/MsgInit/MsgXXX
		void *m_Callback;	// Handler
	};
	assert_size(MessageHandlerEntry, 0x10);

	class InheritedMessageEntry
	{
	public:
		bool m_Unknown;
		RTTI *m_Type;
		RTTI *m_ClassType;
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

	bool HasPostLoadCallback() const;
	std::vector<std::tuple<const MemberEntry *, const char *, size_t>> GetCategorizedClassMembers() const;
	std::optional<std::string> SerializeObject(const void *Object) const;
	bool DeserializeObject(void *Object, const String& InText) const;

	template<typename T>
	bool SetMemberValue(void *Object, const char *Name, const T& Value) const
	{
		return VisitClassMembersByInheritance(Object, [&](const RTTIClass::MemberEntry& Member, void *MemberObject)
		{
			if (strcmp(Name, Member.m_Name) != 0)
				return false;

			if (Member.IsProperty())
				Member.m_PropertySetter(Object, &Value);
			else
				*reinterpret_cast<T *>(MemberObject) = Value;

			return true;
		});
	}

	bool SetMemberValueByString(void *Object, const char *Name, const String& Value) const
	{
		bool succeeded = false;

		VisitClassMembersByInheritance(Object, [&](const RTTIClass::MemberEntry& Member, void *MemberObject)
		{
			if (strcmp(Name, Member.m_Name) != 0)
				return false;

			if (Member.IsProperty())
				throw std::runtime_error("Cannot deserialize a property string. Functionality not implemented.");

			succeeded = Member.m_Type->DeserializeObject(MemberObject, Value);
			return true;
		});

		return succeeded;
	}

	template<typename T>
	bool GetMemberValue(const void *Object, const char *Name, T *OutValue) const
	{
		return VisitClassMembersByInheritance(const_cast<void *>(Object), [&](const RTTIClass::MemberEntry& Member, void *MemberObject)
		{
			if (strcmp(Name, Member.m_Name) != 0)
				return false;

			if (OutValue)
			{
				if (Member.IsProperty())
					Member.m_PropertyGetter(Object, OutValue);
				else
					*OutValue = *reinterpret_cast<std::add_const_t<T> *>(MemberObject);
			}

			return true;
		});
	}

	template<typename T>
	T& GetMemberRefUnsafe(void *Object, const char *Name) const
	{
		void *memberObjectPointer = nullptr;

		VisitClassMembersByInheritance(Object, [&](const RTTIClass::MemberEntry& Member, void *MemberObject)
		{
			if (strcmp(Name, Member.m_Name) != 0)
				return false;

			if (Member.IsProperty())
				throw std::runtime_error("Cannot obtain a reference to a property function");

			memberObjectPointer = MemberObject;
			return true;
		});

		if (!memberObjectPointer)
			throw std::runtime_error("Couldn't resolve member name");

		return *reinterpret_cast<T *>(memberObjectPointer);
	}

	template<typename Func>
	requires (std::is_invocable_r_v<bool, Func, const RTTIClass::MemberEntry& /*Member*/, void */*MemberObject*/>)
	bool VisitClassMembersByInheritance(void *Object, const Func& Callback) const
	{
		return EnumerateOrderedRTTIClassMembers([&](const RTTIClass::MemberEntry& Member, const char *, uint32_t BaseOffset, bool)
		{
			if (Member.IsGroupMarker())
				return false;

			auto rawObject = reinterpret_cast<void *>(reinterpret_cast<uintptr_t>(Object) + BaseOffset + Member.m_Offset);
			return Callback(Member, rawObject);
		});
	}

	template<typename Func>
	requires (std::is_invocable_r_v<bool, Func, const RTTIClass::MemberEntry& /*Member*/, const char */*Category*/, uint32_t /*BaseOffset*/, bool /*TopLevel*/>)
	bool EnumerateOrderedRTTIClassMembers(const Func& Callback, uint32_t BaseOffset = 0, bool TopLevel = true) const
	{
		const char *activeCategory = "";

		for (auto& base : ClassInheritance())
		{
			if (base.m_Type->EnumerateOrderedRTTIClassMembers(Callback, BaseOffset + base.m_Offset, false))
				return true;
		}

		for (auto& member : ClassMembers())
		{
			if (member.IsGroupMarker())
				activeCategory = member.m_Name;

			if (Callback(member, activeCategory, BaseOffset, TopLevel))
				return true;
		}

		return false;
	}
};
assert_offset(RTTIClass, m_MessageHandlerCount, 0x8);
assert_offset(RTTIClass, m_Size, 0x10);
assert_offset(RTTIClass, m_Constructor, 0x18);
assert_offset(RTTIClass, m_Name, 0x38);
assert_offset(RTTIClass, m_InheritanceTable, 0x58);
assert_offset(RTTIClass, m_MemberTable, 0x60);
assert_offset(RTTIClass, m_GetExportedSymbols, 0x80);

class RTTIPOD : public RTTI
{
public:
	// Type 6 = Plain old data. Doesn't exist in static RTTI. They're generated at runtime by combining fields for optimization purposes.
	uint32_t m_Size; // +0x8
};
assert_offset(RTTIPOD, m_Size, 0x8);

}
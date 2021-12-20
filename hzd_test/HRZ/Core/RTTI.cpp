#include <format>

#include "RTTI.h"

namespace HRZ
{

bool RTTI::IsExactKindOf(const RTTI *Other) const
{
	return this == Other;
}

bool RTTI::IsKindOf(const RTTI *Other) const
{
	return static_cast<TypeId>(m_RuntimeTypeId1 - Other->m_RuntimeTypeId1) <= Other->m_RuntimeTypeId2;
}

const RTTIPrimitive *RTTI::AsPrimitive() const
{
	if (m_InfoType != InfoType::Primitive)
		return nullptr;

	return static_cast<const RTTIPrimitive *>(this);
}

const RTTIContainer *RTTI::AsContainer() const
{
	if (m_InfoType != InfoType::Reference && m_InfoType != InfoType::Container)
		return nullptr;

	return static_cast<const RTTIContainer *>(this);
}

const RTTIEnum *RTTI::AsEnum() const
{
	if (m_InfoType != InfoType::Enum && m_InfoType != InfoType::EnumFlags)
		return nullptr;

	return static_cast<const RTTIEnum *>(this);
}

const RTTIClass *RTTI::AsClass() const
{
	if (m_InfoType != InfoType::Class)
		return nullptr;

	return static_cast<const RTTIClass *>(this);
}

const RTTI *RTTI::GetContainedType() const
{
	switch (m_InfoType)
	{
	case InfoType::Reference:
	case InfoType::Container:
		return static_cast<const RTTIContainer *>(this)->m_Type;
	}

	return this;
}

std::string_view RTTI::GetRTTITypeName() const
{
	switch (m_InfoType)
	{
	case InfoType::Primitive: return "RTTIPrimitive";
	case InfoType::Reference: return "RTTIContainer";
	case InfoType::Container: return "RTTIContainer";
	case InfoType::Enum: return "RTTIEnum";
	case InfoType::Class: return "RTTIClass";
	case InfoType::EnumFlags: return "RTTIEnum";
	case InfoType::POD: return "RTTIPOD";
	}

	return "";
}

std::string RTTI::GetSymbolName() const
{
	switch (m_InfoType)
	{
	case InfoType::Primitive:
		return static_cast<const RTTIPrimitive *>(this)->m_Name;

	case InfoType::Reference:
	case InfoType::Container:
	{
		auto container = static_cast<const RTTIContainer *>(this);

		if (!strcmp(container->m_Data->m_Name, "cptr"))
			return std::format("CPtr<{0:}>", container->m_Type->GetSymbolName());

		return std::format("{0:}{1:}", container->m_Data->m_Name, container->m_Type->GetSymbolName());
	}

	case InfoType::Enum:
	case InfoType::EnumFlags:
		return static_cast<const RTTIEnum *>(this)->m_Name;

	case InfoType::Class:
		return static_cast<const RTTIClass *>(this)->m_Name;

	case InfoType::POD:
		return std::format("POD{0:}", static_cast<const RTTIPOD *>(this)->m_Size);
	}

	return "";
}

uint64_t RTTI::GetCoreBinaryTypeId() const
{
	uint64_t hashedData[2] = {};
	Offsets::CallID<"RTTI::GetCoreBinaryTypeId", void(*)(uint64_t *, const RTTI *, __int64)>(hashedData, this, 2);

	return hashedData[0];
}

std::optional<std::string> RTTI::SerializeObject(const void *Object) const
{
	switch (m_InfoType)
	{
	case InfoType::Primitive:
		return static_cast<const RTTIPrimitive *>(this)->SerializeObject(Object);

	case InfoType::Reference:
	case InfoType::Container:
		return static_cast<const RTTIContainer *>(this)->SerializeObject(Object);

	case InfoType::Enum:
	case InfoType::EnumFlags:
		return static_cast<const RTTIEnum *>(this)->SerializeObject(Object);

	case InfoType::Class:
		return static_cast<const RTTIClass *>(this)->SerializeObject(Object);

	case InfoType::POD:
		return std::nullopt;
	}

	return std::nullopt;
}

bool RTTI::DeserializeObject(void *Object, const std::string_view InText) const
{
	switch (m_InfoType)
	{
	case InfoType::Primitive:
		return static_cast<const RTTIPrimitive *>(this)->DeserializeObject(Object, InText);

	case InfoType::Reference:
	case InfoType::Container:
		return static_cast<const RTTIContainer *>(this)->DeserializeObject(Object, InText);

	case InfoType::Enum:
	case InfoType::EnumFlags:
		return static_cast<const RTTIEnum *>(this)->DeserializeObject(Object, InText);

	case InfoType::Class:
		return static_cast<const RTTIClass *>(this)->DeserializeObject(Object, InText);

	case InfoType::POD:
		return false;
	}

	return false;
}

std::optional<std::string> RTTIPrimitive::SerializeObject(const void *Object) const
{
	if (String str; m_SerializeString && m_SerializeString(Object, str))
		return str.c_str();

	return std::nullopt;
}

bool RTTIPrimitive::DeserializeObject(void *Object, const std::string_view InText) const
{
	return (m_DeserializeString && m_DeserializeString(InText, Object));
}

std::optional<std::string> RTTIContainer::SerializeObject(const void *Object) const
{
	if (m_InfoType == InfoType::Reference)
		return std::nullopt;

	if (m_Data)
	{
		auto containerData = static_cast<const ContainerData *>(m_Data);

		if (String str; containerData->m_SerializeString && containerData->m_SerializeString(Object, this, str))
			return str.c_str();
	}

	return std::nullopt;
}

bool RTTIContainer::DeserializeObject(void *Object, const std::string_view InText) const
{
	if (m_InfoType == InfoType::Reference)
		return false;

	if (m_Data)
	{
		auto containerData = static_cast<const ContainerData *>(m_Data);

		return (containerData->m_DeserializeString && containerData->m_DeserializeString(InText, this, Object));
	}

	return false;
}

std::optional<std::string> RTTIEnum::SerializeObject(const void *Object) const
{
	if (m_InfoType == InfoType::EnumFlags)
		__debugbreak();

	for (auto& member : EnumMembers())
	{
		if (memcmp(&member.m_Value, Object, m_EnumUnderlyingTypeSize) == 0)
			return member.m_Name;
	}

	return std::nullopt;
}

bool RTTIEnum::DeserializeObject(void *Object, const std::string_view InText) const
{
	if (m_InfoType == InfoType::EnumFlags)
		__debugbreak();

	for (auto& member : EnumMembers())
	{
		if (member.m_Name == InText)
		{
			memcpy(Object, &member.m_Value, m_EnumUnderlyingTypeSize);
			return true;
		}
	}

	return false;
}

bool RTTIClass::MemberEntry::IsGroupMarker() const
{
	return m_Type == nullptr;
}

bool RTTIClass::MemberEntry::IsSaveStateOnly() const
{
	return (m_Flags & SAVE_STATE_ONLY) == SAVE_STATE_ONLY;
}

bool RTTIClass::MemberEntry::IsProperty() const
{
	return m_PropertyGetter || m_PropertySetter;
}

bool RTTIClass::HasPostLoadCallback() const
{
	for (auto& event : ClassMessageHandlers())
	{
		if (event.m_Type->GetSymbolName() == "MsgReadBinary")
			return true;
	}

	return false;
}

std::vector<std::tuple<const RTTIClass::MemberEntry *, const char *, size_t>> RTTIClass::GetCategorizedClassMembers() const
{
	// Build a list of all fields from this class and its parent classes
	struct SorterEntry
	{
		size_t m_DeclOrder;
		const MemberEntry *m_Type;
		const char *m_Category;
		uint32_t m_Offset;
		bool m_TopLevel;
	};

	std::vector<SorterEntry> sortedEntries;

	this->EnumerateClassMembersByInheritance([&](const RTTIClass::MemberEntry& Member, const char *Category, uint32_t BaseOffset, bool TopLevel)
	{
		SorterEntry entry
		{
			.m_DeclOrder = sortedEntries.size(),
			.m_Type = &Member,
			.m_Category = Category,
			.m_Offset = BaseOffset + Member.m_Offset,
			.m_TopLevel = TopLevel,
		};

		sortedEntries.emplace_back(entry);
		return false;
	});

	// Decima's specific quicksort algorithm is mandatory
	PCore_Quicksort<SorterEntry>(sortedEntries, [](const SorterEntry *A, const SorterEntry *B)
	{
		return A->m_Offset < B->m_Offset;
	});

	// Declaration order must be preserved - return the member index as defined in the static RTTI data
	std::vector<std::tuple<const MemberEntry *, const char *, size_t>> out;

	for (auto& entry : sortedEntries)
	{
		// We only care about the top-level fields
		if (!entry.m_TopLevel || entry.m_Type->IsGroupMarker())
			continue;

		out.emplace_back(entry.m_Type, entry.m_Category, entry.m_DeclOrder);
	}

	return out;
}

std::optional<std::string> RTTIClass::SerializeObject(const void *Object) const
{
	// Check if a dedicated decoding function was implemented
	if (m_SerializeString)
	{
		if (String str; m_SerializeString(Object, str))
			return str.c_str();

		return std::nullopt;
	}

	// Split each member by a newline
	std::string finalDecl;

	EnumerateClassMembersByInheritance([&](const RTTIClass::MemberEntry& Member, const char *, uint32_t BaseOffset, bool)
	{
		// TODO: Properties need to be handled
		if (Member.IsGroupMarker() || Member.IsProperty())
			return false;

		auto memberObject = reinterpret_cast<const void *>(reinterpret_cast<uintptr_t>(Object) + BaseOffset + Member.m_Offset);
		auto decl = Member.m_Type->SerializeObject(memberObject);

		if (decl)
			finalDecl += decl.value().append("\n");

		return false;
	});

	return finalDecl;
}

bool RTTIClass::DeserializeObject(void *Object, const std::string_view InText) const
{
	if (m_DeserializeString)
		return m_DeserializeString(Object, InText);

	__debugbreak();
	return false;
}

}
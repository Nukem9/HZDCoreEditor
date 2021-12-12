#include "RTTI.h"

namespace HRZ
{

#include "RTTI.inl"

bool RTTI::IsExactKindOf(const RTTI *Other) const
{
	return this == Other;
}

bool RTTI::IsKindOf(const RTTI *Other) const
{
	return static_cast<TypeId>(m_RuntimeTypeId1 - Other->m_RuntimeTypeId1) <= Other->m_RuntimeTypeId2;
}

const RTTIContainer *RTTI::AsContainer() const
{
	if (m_InfoType != INFO_TYPE_REFERENCE && m_InfoType != INFO_TYPE_CONTAINER)
		return nullptr;

	return static_cast<const RTTIContainer *>(this);
}

const RTTIEnum *RTTI::AsEnum() const
{
	if (m_InfoType != INFO_TYPE_ENUM && m_InfoType != INFO_TYPE_ENUM_2)
		return nullptr;

	return static_cast<const RTTIEnum *>(this);
}

const RTTIClass *RTTI::AsClass() const
{
	if (m_InfoType != INFO_TYPE_CLASS)
		return nullptr;

	return static_cast<const RTTIClass *>(this);
}

const RTTI *RTTI::GetContainedType() const
{
	switch (m_InfoType)
	{
	case INFO_TYPE_REFERENCE:
	case INFO_TYPE_CONTAINER:
		return static_cast<const RTTIContainer *>(this)->m_Type;
	}

	return this;
}

std::string_view RTTI::GetRTTITypeName() const
{
	switch (m_InfoType)
	{
	case INFO_TYPE_PRIMITIVE: return "RTTIPrimitive";
	case INFO_TYPE_REFERENCE: return "RTTIContainer";
	case INFO_TYPE_CONTAINER: return "RTTIContainer";
	case INFO_TYPE_ENUM: return "RTTIEnum";
	case INFO_TYPE_CLASS: return "RTTIClass";
	case INFO_TYPE_ENUM_2: return "RTTIEnum";
	case INFO_TYPE_POD: return "RTTIPOD";
	}

	return "";
}

std::string RTTI::GetSymbolName() const
{
	switch (m_InfoType)
	{
	case INFO_TYPE_PRIMITIVE:
		return static_cast<const RTTIPrimitive *>(this)->m_Name;

	case INFO_TYPE_REFERENCE:
	case INFO_TYPE_CONTAINER:
	{
		char refType[1024];
		auto container = static_cast<const RTTIContainer *>(this);

		if (!strcmp(container->m_Data->m_Name, "cptr"))
			sprintf_s(refType, "CPtr<%s>", container->m_Type->GetSymbolName().c_str());
		else
			sprintf_s(refType, "%s<%s>", container->m_Data->m_Name, container->m_Type->GetSymbolName().c_str());

		return refType;
	}

	case INFO_TYPE_ENUM:
	case INFO_TYPE_ENUM_2:
		return static_cast<const RTTIEnum *>(this)->m_Name;

	case INFO_TYPE_CLASS:
		return static_cast<const RTTIClass *>(this)->m_Name;

	case INFO_TYPE_POD:
		char podType[16];
		sprintf_s(podType, "POD%d", static_cast<const RTTIPOD *>(this)->m_Size);

		return podType;
	}

	return "";
}

uint64_t RTTI::GetCoreBinaryTypeId() const
{
	uint64_t hashedData[2] = {};
	Offsets::CallID<"RTTI::GetCoreBinaryTypeId", void(*)(uint64_t *, const RTTI *, __int64)>(hashedData, this, 2);

	return hashedData[0];
}

bool RTTIClass::MemberEntry::IsGroupMarker() const
{
	return m_Type == nullptr;
}

bool RTTIClass::MemberEntry::IsSaveStateOnly() const
{
	return (m_Flags & SAVE_STATE_ONLY) == SAVE_STATE_ONLY;
}

bool RTTIClass::IsPostLoadCallbackEnabled() const
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
	std::vector<SorterEntry> sortedEntries;
	BuildFullClassMemberLayout(this, sortedEntries, 0, true);

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

void RTTIClass::BuildFullClassMemberLayout(const RTTIClass *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel)
{
	const char *activeCategory = "";

	for (auto& base : Type->ClassInheritance())
		BuildFullClassMemberLayout(base.m_Type->AsClass(), Members, Offset + base.m_Offset, false);

	for (auto& member : Type->ClassMembers())
	{
		if (!member.m_Type)
			activeCategory = member.m_Name;

		SorterEntry entry
		{
			.m_DeclOrder = Members.size(),
			.m_Type = &member,
			.m_Category = activeCategory,
			.m_Offset = member.m_Offset + Offset,
			.m_TopLevel = TopLevel,
		};

		Members.emplace_back(entry);
	}
}

}
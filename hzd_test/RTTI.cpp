#include "RTTI.h"
#include "PCore.h"

const GGRTTIContainer *GGRTTI::AsContainer() const
{
	if (m_InfoType != INFO_TYPE_REFERENCE && m_InfoType != INFO_TYPE_CONTAINER)
		return nullptr;

	return static_cast<const GGRTTIContainer *>(this);
}

const GGRTTIEnum *GGRTTI::AsEnum() const
{
	if (m_InfoType != INFO_TYPE_ENUM && m_InfoType != INFO_TYPE_ENUM_2)
		return nullptr;

	return static_cast<const GGRTTIEnum *>(this);
}

const GGRTTIClass *GGRTTI::AsClass() const
{
	if (m_InfoType != INFO_TYPE_CLASS)
		return nullptr;

	return static_cast<const GGRTTIClass *>(this);
}

const GGRTTI *GGRTTI::GetContainedType() const
{
	switch (m_InfoType)
	{
	case INFO_TYPE_REFERENCE:
	case INFO_TYPE_CONTAINER:
		return static_cast<const GGRTTIContainer *>(this)->m_Type;
	}

	return this;
}

std::string GGRTTI::GetSymbolName() const
{
	switch (m_InfoType)
	{
	case INFO_TYPE_PRIMITIVE:
		return static_cast<const GGRTTIPrimitive *>(this)->m_Name;

	case INFO_TYPE_REFERENCE:
	case INFO_TYPE_CONTAINER:
	{
		char refType[1024];
		auto container = static_cast<const GGRTTIContainer *>(this);

		if (!strcmp(*container->m_ContainerName, "cptr"))
			sprintf_s(refType, "CPtr<%s>", container->m_Type->GetSymbolName().c_str());
		else
			sprintf_s(refType, "%s<%s>", *container->m_ContainerName, container->m_Type->GetSymbolName().c_str());

		return refType;
	}

	case INFO_TYPE_ENUM:
	case INFO_TYPE_ENUM_2:
		return static_cast<const GGRTTIEnum *>(this)->m_Name;

	case INFO_TYPE_CLASS:
		return static_cast<const GGRTTIClass *>(this)->m_Name;

	case INFO_TYPE_POD:
		char podType[16];
		sprintf_s(podType, "POD%d", static_cast<const GGRTTIPOD *>(this)->m_Size);

		return podType;
	}

	return "";
}

uint64_t GGRTTI::GetCoreBinaryTypeId() const
{
	uint64_t hashedData[2] = {};

	const static auto addr = g_OffsetMap["GGRTTI::GetCoreBinaryTypeId"];
	((void(__fastcall *)(uint64_t *, const GGRTTI *, __int64))(addr))(hashedData, this, 2);

	return hashedData[0];
}

bool GGRTTIClass::MemberEntry::IsGroupMarker() const
{
	return m_Type == nullptr;
}

bool GGRTTIClass::MemberEntry::IsSaveStateOnly() const
{
	return (m_Flags & SAVE_STATE_ONLY) == SAVE_STATE_ONLY;
}

bool GGRTTIClass::IsPostLoadCallbackEnabled() const
{
	for (auto& event : ClassEventSubscriptions())
	{
		if (event.m_Type->GetSymbolName() == "MsgReadBinary")
			return true;
	}

	return false;
}

std::vector<std::pair<const GGRTTIClass::MemberEntry *, const char *>> GGRTTIClass::GetSortedClassMembers() const
{
	std::vector<SorterEntry> sortedEntries;
	BuildFullClassMemberLayout(this, sortedEntries, 0, true);

	// Decima's specific quicksort algorithm is mandatory
	PCore_Quicksort<SorterEntry>(sortedEntries, [](const SorterEntry *A, const SorterEntry *B)
	{
		return A->m_Offset < B->m_Offset;
	});

	// We only care about the top-level fields
	std::vector<std::pair<const MemberEntry *, const char *>> out;

	for (auto& entry : sortedEntries)
	{
		if (entry.m_TopLevel)
			out.emplace_back(entry.m_Type, entry.m_Category);
	}

	return out;
}

void GGRTTIClass::BuildFullClassMemberLayout(const GGRTTIClass *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel)
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
			.m_Type = &member,
			.m_Category = activeCategory,
			.m_Offset = member.m_Offset + Offset,
			.m_TopLevel = TopLevel,
		};

		Members.emplace_back(entry);
	}
}
#include "RTTI.h"

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

#if HORIZON_ZERO_DAWN
	const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
#elif DEATH_STRANDING
	const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");
#endif

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
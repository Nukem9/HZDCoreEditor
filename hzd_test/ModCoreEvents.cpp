#include <unordered_set>

#include "HRZ/PGraphics3D/HwTexture.h"
#include "HRZ/PGraphics3D/HwBuffer.h"
#include "HRZ/PGraphics3D/TextureDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "HRZ/Core/Texture.h"
#include "HRZ/Core/VertexArrayResource.h"
#include "RTTI/RTTIScanner.h"
#include "ModConfig.h"
#include "ModCoreEvents.h"

using namespace HRZ;

namespace HRZ
{
DECL_RTTI(WeatherSetup);
DECL_RTTI(SpawnSetupBase);
DECL_RTTI(AIFaction);
}

HRZ::SharedLock ResourceListLock;
std::unordered_set<RTTIRefObject *> AllResources;
std::unordered_set<RTTIRefObject *> CachedWeatherSetups;
std::unordered_set<RTTIRefObject *> CachedSpawnSetupBases;
std::unordered_set<RTTIRefObject *> CachedAIFactions;

ModCoreEvents::ValuePatchVisitor::ValuePatchVisitor(const RTTIValuePatch& Patch) : m_Patch(Patch)
{
}

void ModCoreEvents::ValuePatchVisitor::SetValue(void *Object, const RTTI *Type)
{
	bool result = Type->DeserializeObject(Object, m_Patch.m_Value);

	if (!result)
	{
		DebugUI::LogWindow::AddLog("[AssetOverride] Failed to set variable '%s' to '%s' for object %p\n", m_Patch.m_Path.c_str(), m_Patch.m_Value.c_str(), Object);
		__debugbreak();
	}
}

int ModCoreEvents::ValuePatchVisitor::GetFlags()
{
	return 1;
}

ModCoreEvents::ModCoreEvents()
{
	auto splitStringByDelimiter = []<typename Func>(const std::string_view & Text, char Delim, const Func& Callback)
	{
		if (Text.empty())
			return;

		size_t last = 0;
		size_t next = 0;
		
		while ((next = Text.find(Delim, last)) != std::string_view::npos)
		{
			Callback(Text.substr(last, next - last));
			last = next + 1;
		}
		
		Callback(Text.substr(last));
	};

	// Create all of the patch instances from mod configuration data
	for (auto& override : ModConfiguration.AssetOverrides)
	{
		if (!override.Enabled)
			continue;

		// Lookup is done by UUID
		splitStringByDelimiter(override.ObjectUUIDs, ',', [&](const std::string_view& UUID)
		{
			auto uuid = GGUUID::Parse(UUID);

			RTTIValuePatch patch
			{
				.m_MatchCriteria = uuid,
				.m_Path = override.Path.c_str(),
				.m_Value = override.Value.c_str(),
			};

			m_RTTIPatchesByUUID[uuid].emplace_back(patch);
		});

		// Lookup is done by type
		splitStringByDelimiter(override.ObjectTypes, ',', [&](const std::string_view& Type)
		{
			auto type = RTTIScanner::GetTypeByName(Type);

			if (!type)
				__debugbreak();

			RTTIValuePatch patch
			{
				.m_MatchCriteria = type,
				.m_Path = override.Path.c_str(),
				.m_Value = override.Value.c_str(),
			};

			m_RTTIPatchesByType[type].emplace_back(patch);
		});
	}
}

void ModCoreEvents::OnBeginRootCoreLoad(const String& CorePath)
{
	DebugUI::LogWindow::AddLog("OnBeginRootCoreLoad %s\n", CorePath.c_str());
}

void ModCoreEvents::OnEndRootCoreLoad(const String& CorePath)
{
	DebugUI::LogWindow::AddLog("OnEndRootCoreLoad %s\n", CorePath.c_str());
}

void ModCoreEvents::OnBeginCoreUnload(const String& CorePath)
{
	DebugUI::LogWindow::AddLog("OnBeginCoreUnload %s\n", CorePath.c_str());
}

void ModCoreEvents::OnEndCoreUnload()
{
	DebugUI::LogWindow::AddLog("OnEndCoreUnload\n");
}

void ModCoreEvents::OnCoreLoad(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	for (auto& refObject : Objects)
	{
		if (refObject->m_ObjectUUID == GGUUID::Parse("{44E1CF57-24A6-9A43-B087-172C87BB0DFE}"))
		{
			// Disable the intro menu
			*(HRZ::Ref<HRZ::RTTIRefObject> *)((uintptr_t)refObject.get() + 0x158) = *(HRZ::Ref<HRZ::RTTIRefObject> *)((uintptr_t)refObject.get() + 0xB0);
		}
		else if (refObject->m_ObjectUUID == GGUUID::Parse("{F5F801CF-5F14-F34F-8695-E701F7BF3728}"))
		{
			auto& array = refObject->GetMemberRefUnsafe<Array<void *>>("OutOfBoundsAreaTags");
			array.clear();
		}

		auto rtti = refObject->GetRTTI()->AsClass();
		auto object = refObject.get();

		auto applyPatches = [&](const std::vector<RTTIValuePatch>& Patches)
		{
			for (auto& patch : Patches)
			{
				ValuePatchVisitor v(patch);
				RTTIObjectTweaker::VisitObjectPath(object, rtti, patch.m_Path, &v);

				if (!v.m_LastError.empty())
					__debugbreak();
			}
		};

		if (auto itr = m_RTTIPatchesByUUID.find(object->m_ObjectUUID); itr != m_RTTIPatchesByUUID.end())
			applyPatches(itr->second);

		if (auto itr = m_RTTIPatchesByType.find(rtti); itr != m_RTTIPatchesByType.end())
			applyPatches(itr->second);
	}

	DebugUI::LogWindow::AddLog("OnCoreLoad (%lld) %s\n", Objects.size(), CorePath.c_str());
}

void ModCoreEvents::OnLoadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	ResourceListLock.lock();
	{
		for (auto& refObject : Objects)
		{
			AllResources.emplace(refObject.get());

			if (refObject->GetRTTI()->IsKindOf(HRZ::RTTI_WeatherSetup))
				CachedWeatherSetups.emplace(refObject.get());

			if (refObject->GetRTTI()->IsKindOf(HRZ::RTTI_SpawnSetupBase))
				CachedSpawnSetupBases.emplace(refObject.get());

			if (refObject->GetRTTI()->IsKindOf(HRZ::RTTI_AIFaction))
				CachedAIFactions.emplace(refObject.get());
		}
	}
	ResourceListLock.unlock();

	DebugUI::LogWindow::AddLog("OnLoadCoreObjects (%lld) %s\n", Objects.size(), CorePath.c_str());
}

void ModCoreEvents::OnUnloadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	ResourceListLock.lock();
	{
		for (auto& refObject : Objects)
		{
			AllResources.erase(refObject.get());
			CachedWeatherSetups.erase(refObject.get());
			CachedSpawnSetupBases.erase(refObject.get());
			CachedAIFactions.erase(refObject.get());
		}
	}
	ResourceListLock.unlock();

	DebugUI::LogWindow::AddLog("OnUnloadCoreObjects (%lld) %s\n", Objects.size(), CorePath.c_str());
}

ModCoreEvents& ModCoreEvents::Instance()
{
	static ModCoreEvents handler;
	return handler;
}
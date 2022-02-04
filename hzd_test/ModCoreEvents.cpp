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
std::unordered_set<RTTIRefObject *> CachedWeatherSetups;
std::unordered_set<RTTIRefObject *> CachedAIFactions;

ModCoreEvents::ValuePatchVisitor::ValuePatchVisitor(const RTTIValuePatch& Patch) : m_Patch(Patch)
{
}

void ModCoreEvents::ValuePatchVisitor::SetValue(void *Object, const RTTI *Type)
{
	bool result = Type->DeserializeObject(Object, m_Patch.m_Value);

	if (!result)
		m_LastError = String::Format("Failed to set variable '%s' to '%s'.", m_Patch.m_Path.c_str(), m_Patch.m_Value.c_str(), Object);
}

int ModCoreEvents::ValuePatchVisitor::GetFlags()
{
	return 1;
}

ModCoreEvents::ModCoreEvents()
{
	auto splitStringByDelimiter = []<typename Func>(const std::string_view Text, char Delim, const Func& Callback)
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

		if (override.ObjectTypes.empty() && override.ObjectUUIDs.empty())
			DebugUI::LogWindow::AddLog("[AssetOverride] ObjectTypes and ObjectUUIDs are both empty. Patches will have no effect.\n");

		// Lookup is done by type
		splitStringByDelimiter(override.ObjectTypes, ',', [&](const std::string_view Type)
		{
			auto type = RTTIScanner::GetTypeByName(Type);

			if (!type)
			{
				DebugUI::LogWindow::AddLog("[AssetOverride] Failed to resolve type name '%s'. Skipping.\n", Type.data());
				return;
			}

			RTTIValuePatch patch
			{
				.m_Path = override.Path.c_str(),
				.m_Value = override.Value.c_str(),
			};

			m_RTTIPatchesByType[type].emplace_back(patch);
		});

		// Lookup is done by UUID
		splitStringByDelimiter(override.ObjectUUIDs, ',', [&](const std::string_view UUID)
		{
			auto uuid = GGUUID::TryParse(UUID);

			if (!uuid)
			{
				DebugUI::LogWindow::AddLog("[AssetOverride] Failed to resolve UUID '%s'. Skipping.\n", UUID.data());
				return;
			}

			RTTIValuePatch patch
			{
				.m_Path = override.Path.c_str(),
				.m_Value = override.Value.c_str(),
			};

			m_RTTIPatchesByUUID[uuid.value()].emplace_back(patch);
		});
	}
}

void ModCoreEvents::OnBeginRootCoreLoad(const String& CorePath)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnBeginRootCoreLoad(%s)\n", CorePath.c_str());
}

void ModCoreEvents::OnEndRootCoreLoad(const String& CorePath)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnEndRootCoreLoad(%s)\n", CorePath.c_str());
}

void ModCoreEvents::OnBeginCoreUnload(const String& CorePath)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnBeginCoreUnload(%s)\n", CorePath.c_str());
}

void ModCoreEvents::OnEndCoreUnload()
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnEndCoreUnload()\n");
}

void ModCoreEvents::OnCoreLoad(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnCoreLoad(%s, %lld objects)\n", CorePath.c_str(), Objects.size());

	for (auto& refObject : Objects)
	{
		// Skip the main intro movie. InGameMenuResource needs to be manually patched as it will crash when unloading if
		// StartupIntro is a null pointer.
		if (ModConfiguration.SkipIntroLogos)
		{
			if (refObject->m_ObjectUUID == GGUUID::Parse("{44E1CF57-24A6-9A43-B087-172C87BB0DFE}"))
				refObject->GetMemberRefUnsafe<Ref<RTTIRefObject>>("StartupIntro") = refObject->GetMemberRefUnsafe<Ref<RTTIRefObject>>("StartPage");
		}

		// Apply all patches loaded from user config
		auto rtti = refObject->GetRTTI();
		auto applyPatches = [&](const std::vector<RTTIValuePatch>& Patches)
		{
			for (auto& patch : Patches)
			{
				ValuePatchVisitor v(patch);
				RTTIObjectTweaker::VisitObjectPath(refObject, rtti, patch.m_Path, &v);

				if (!v.m_LastError.empty())
					DebugUI::LogWindow::AddLog("[AssetOverride] Failed to patch object %p. %s\n", refObject, v.m_LastError.c_str());
			}
		};

		if (auto itr = m_RTTIPatchesByType.find(rtti); itr != m_RTTIPatchesByType.end())
			applyPatches(itr->second);

		if (auto itr = m_RTTIPatchesByUUID.find(refObject->m_ObjectUUID); itr != m_RTTIPatchesByUUID.end())
			applyPatches(itr->second);
	}
}

void ModCoreEvents::OnLoadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnLoadCoreObjects(%s, %lld objects)\n", CorePath.c_str(), Objects.size());

	ResourceListLock.lock();
	{
		for (auto& refObject : Objects)
		{
			if (refObject->GetRTTI()->IsKindOf(HRZ::RTTI_WeatherSetup))
				CachedWeatherSetups.emplace(refObject);

			if (refObject->GetRTTI()->IsKindOf(HRZ::RTTI_AIFaction))
				CachedAIFactions.emplace(refObject);
		}
	}
	ResourceListLock.unlock();
}

void ModCoreEvents::OnUnloadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects)
{
	if (ModConfiguration.EnableCoreLogging)
		DebugUI::LogWindow::AddLog("[Asset] OnUnloadCoreObjects(%s, %lld objects)\n", CorePath.c_str(), Objects.size());

	ResourceListLock.lock();
	{
		for (auto& refObject : Objects)
		{
			CachedWeatherSetups.erase(refObject);
			CachedAIFactions.erase(refObject);
		}
	}
	ResourceListLock.unlock();
}

ModCoreEvents& ModCoreEvents::Instance()
{
	// Yes, I'm intentionally leaking memory. There's no virtual destructor present and this
	// never gets unregistered properly.
	static auto handler = new ModCoreEvents();

	return *handler;
}
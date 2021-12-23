#include <unordered_set>

#include "HRZ/PGraphics3D/HwTexture.h"
#include "HRZ/PGraphics3D/HwBuffer.h"
#include "HRZ/PGraphics3D/TextureDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "HRZ/Core/Texture.h"
#include "HRZ/Core/VertexArrayResource.h"
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

HRZ::Ref<HwTexture> TestTexture;
HRZ::Ref<HwBuffer> TestBuffer;

ModCoreEvents& ModCoreEvents::Instance()
{
	static ModCoreEvents handler;
	return handler;
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

void ModCoreEvents::OnEndCoreUnload(/* 'const String& CorePath' argument is passed but the pointer is invalid */)
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

		if (refObject->m_ObjectUUID == GGUUID::Parse("{9DE4FAD6-597C-3533-BCE4-73A3335438C4}"))
		{
			auto texture = RTTI::Cast<Texture>(refObject.get());
			auto b = static_cast<TextureDX12 *>(texture->m_HwTexture.get());
			b->m_Copies[0].m_State.m_D3DResource->SetName(L"MY SPECIAL RESOURCE");

			TestTexture = texture->m_HwTexture;
		}

		if (refObject->m_ObjectUUID == GGUUID::Parse("{3F7C9F06-44F9-8A3F-85F4-BA9C2C8D3500}"))
		{
			auto b = RTTI::Cast<VertexArrayResource>(refObject.get());

			TestBuffer = b->m_VertexArray->m_VertexStreams[1].m_Resource->m_Buffer;
		}

		if (refObject->m_ObjectUUID == GGUUID::Parse("{28846CC1-96AF-AA4A-8AB9-C491D56BC17E}") ||
			refObject->m_ObjectUUID == GGUUID::Parse("{68FF19FB-A372-6947-8216-66FF83DB0A1F}"))
		{
			// Ref<Extern> {'sounds/effects/movements/gear/aloy_outfit_warbot/wav/shield_loop_b_1_m', {28846CC1-96AF-AA4A-8AB9-C491D56BC17E}}
			// Ref<Extern> {'sounds/effects/movements/gear/aloy_outfit_warbot/wav/shield_loop_a_1_m', {68FF19FB-A372-6947-8216-66FF83DB0A1F}}
			// SampleCount = 0
			*(int *)((uintptr_t)refObject.get() + 0x64) = 0;
		}

		if (refObject->m_ObjectUUID == GGUUID::Parse("{0867F5E6-0375-7434-9534-BB9B73FB1103}"))
		{
			// Ref<Extern> {'models/characters/humans/aloyadvancedwarbot/animation/parts/shield', {{0867F5E6-0375-7434-9534-BB9B73FB1103}}
			// EndIndex = 0
			*(int *)((uintptr_t)refObject.get() + 0x74) = 0;
		}

		if (refObject->m_ObjectUUID == GGUUID::Parse("{44E1CF57-24A6-9A43-B087-172C87BB0DFE}"))
		{
		}
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
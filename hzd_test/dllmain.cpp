#include <atomic>
#include <stdlib.h>
#include <detours/Detours.h>

#include "HRZ/PGraphics3D/SwapChainDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/MainMenuBar.h"

#include "HRZ/Core/Application.h"
#include "HRZ/Core/StreamingManager.h"
#include "HRZ/Core/CoreFileManager.h"
#include "HRZ/Core/WorldTransform.h"
#include "HRZ/Core/CameraEntity.h"
#include "HRZ/Core/GameModule.h"
#include "HRZ/Core/WorldState.h"
#include "HRZ/Core/BodyVariantRuntimeComponent.h"

#include "RTTI/MSRTTI.h"
#include "RTTI/RTTIScanner.h"

#include "LogHooks.h"
#include "ModConfig.h"
#include "ModCoreEvents.h"
#include "common.h"

using namespace HRZ;

bool hk_SwapChainDX12_Present(SwapChainDX12 *SwapChain)
{
	// Register debug UI
	static bool initUI = [&]()
	{
		DebugUI::Initialize(SwapChain);
		return true;
	}();

	// Register core load callbacks
	auto streamingManager = HRZ::StreamingManager::Instance();

	if (streamingManager)
	{
		static bool initCore = [&]()
		{
			streamingManager->m_CoreFileManager->RegisterEventListener(ModCoreEvents::Instance());
			return true;
		}();
	}

	Application::RunMainThreadCallbacks();

	DebugUI::RenderUI();
	DebugUI::RenderUID3D(SwapChain);
	return SwapChain->Present();
}

void hk_call_1413AB8FC(HRZ::CameraEntity *This, WorldTransform& Transform)
{
	if (DebugUI::MainMenuBar::m_FreeCamMode == DebugUI::MainMenuBar::FreeCamMode::Free)
		return;

	This->SetTransform(Transform);
}

void hk_call_1411E3210(HRZ::GameModule *This, float Timescale, float TransitionTime)
{
	if (DebugUI::MainMenuBar::m_TimescaleOverride && (!This->IsSuspended() || DebugUI::MainMenuBar::m_TimescaleOverrideInMenus))
	{
		This->m_TimescaleTransitionCurrent = DebugUI::MainMenuBar::m_Timescale;
		This->m_TimescaleTransitionTarget = DebugUI::MainMenuBar::m_Timescale;
		This->m_ActiveTimescale = DebugUI::MainMenuBar::m_Timescale;
		return;
	}

	This->SetTimescale(Timescale, TransitionTime);
}

struct EntrypointToAddress
{
	uint32_t m_CRC;
	void *m_Address;
};

struct EntrypointArray
{
	uint32_t m_Count;
	char _pad4[0xC];
	EntrypointToAddress m_Entries[10000];
};

void hk_call_1417BC7AA()
{
	RTTIScanner::ExportAll("C:\\ds_rtti_export", "DS");

	auto& array = *Offsets::Resolve<EntrypointArray *>(0x73E7120);
	FILE *temp = fopen("C:\\ds_rtti_export\\entrypoint_crc_listing.csv", "w");

	for (size_t i = 0; i < array.m_Count; i++)
	{
		auto& element = array.m_Entries[i];

		fprintf(temp, "0x%llX,0x%X\n", reinterpret_cast<uintptr_t>(element.m_Address) - Offsets::GetModule().first + 0x140000000, element.m_CRC);
	}

	fclose(temp);
	ExitProcess(0);
}

void hk_call_1417B52CF(uint64_t *Hash, const char *Data, size_t DataLength)
{
	static FILE *temp = fopen("C:\\ds_rtti_export\\hash_listing.txt", "w");

	Offsets::Call<0x16B9B60, void(*)(uint64_t *, const char *, size_t)>(Hash, Data, DataLength);

	if (temp)
	{
		fprintf(temp, "%llX%llX = %s\n", Hash[0], Hash[1], Data);
		fflush(temp);
	}
}

void (*original_ProcessAIJob1_140DBD530)(__int64);
void hk_ProcessAIJob1_140DBD530(__int64 a1)
{
	if (DebugUI::MainMenuBar::m_PauseAIProcessing)
		return;

	original_ProcessAIJob1_140DBD530(a1);
}

void (* original_ProcessAIJob2_140DBD670)(__int64);
void hk_ProcessAIJob2_140DBD670(__int64 a1)
{
	if (DebugUI::MainMenuBar::m_PauseAIProcessing)
		return;

	original_ProcessAIJob2_140DBD670(a1);
}

void hk_call_1414A4418(BodyVariantRuntimeComponent *Component, const GGUUID& UUID)
{
	Component->SetVariantByUUID(UUID);

#if 0
	if (!ModConfiguration.ForceBodyVariantUUID.empty())
	{
		// Find the core file that maps to this GUID
		auto itr = std::find_if(ModConfiguration.CachedBodyVariants.begin(), ModConfiguration.CachedBodyVariants.end(),
		[](const auto& Pair)
		{
			return Pair.second == ModConfiguration.ForceBodyVariantUUID;
		});

		if (itr == ModConfiguration.CachedBodyVariants.end())
		{
			// User made a typo somewhere
			// TODO: Log
		}
		else
		{
			// Stream in the HumanBodyVariant
			Component->ForceSetUnlistedVariantByPath(itr->first.c_str(), GGUUID::TryParse(itr->second).value());
		}
	}
#endif
}

void LoadSignatures(GameType Game)
{
	auto [moduleBase, moduleEnd] = Offsets::GetModule();

	auto offsetFromInstruction = [&](const char *Signature, uint32_t Add)
	{
		auto addr = XUtil::FindPattern(moduleBase, moduleEnd - moduleBase, Signature);

		if (!addr)
			return addr;

		auto relOffset = *reinterpret_cast<int32_t *>(addr + Add) + sizeof(int32_t);
		return addr + Add + relOffset - moduleBase;
	};

	if (Game == GameType::DeathStranding)
	{
		// Functions
		Offsets::MapSignature("String::CtorCString", "40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
		Offsets::MapSignature("String::Dtor", "40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");

		// Hooks

		// Globals
		Offsets::MapAddress("ExportedSymbolGroupArray", offsetFromInstruction("48 8B C2 4C 8D ? ? ? ? ? 48 8D ? ? ? ? ? 4C 8D ? ? ? ? ? 48 8D ? ? ? ? ? 48 FF E0", 6));
	}
	else if (Game == GameType::HorizonZeroDawn)
	{
		// Functions
		Offsets::MapSignature("String::CtorCString", "48 89 5C 24 10 48 89 6C 24 18 48 89 7C 24 20 41 56 48 83 EC 20 33 FF 48 8B EA 48 89 39 4C 8B F1 48 C7 C3 FF FF FF FF");
		Offsets::MapSignature("String::Dtor", "40 53 48 83 EC 20 48 8B 19 48 85 DB 74 37 48 83 C3 F0 48 8D");
		Offsets::MapSignature("String::AssignFromOther", "48 89 5C 24 10 48 89 74 24 18 57 48 83 EC 20 48 8B 39 48 8B F2 48 8B D9 48 3B 3A 74 54");
		Offsets::MapSignature("String::operatorEquality", "48 89 74 24 10 57 48 83 EC 20 4C 8B 09 48 8B FA 4C 8B 02 48 8B F1 4D 3B C8 75 0D B0 01 48 8B 74 24 38 48 83 C4 20 5F C3 41 8B 40 F8");

		Offsets::MapSignature("WString::Ctor", "B8 01 00 00 00 F0 0F C1 ? ? ? ? 02 48 8D 15 ? ? ? ? 48 8B C1 48 89 11 C3");
		Offsets::MapSignature("WString::CtorCString", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 33 ED 48 8B F2 8B DD 48 89 29");
		Offsets::MapSignature("WString::Dtor", "48 8B 09 B8 FF FF FF FF F0 0F C1 01 83 F8 01 0F 84 ? ? ? ? C3");
		Offsets::MapSignature("WString::AssignFromOther", "48 89 5C 24 08 57 48 83 EC 20 48 8B D9 48 8B FA 48 8B 09 48 3B 0A 74 22");
		Offsets::MapSignature("WString::EncodeUTF8", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B 01 48 8B F2 48 8B D9 48 8B CE 8B 78 08");

		Offsets::MapSignature("WeakPtr::Acquire", "40 57 48 83 EC 20 48 8B F9 48 8B 09 48 85 C9 74 58 33 C0 48 89 5C 24 30");
		Offsets::MapSignature("WeakPtr::Release", "40 57 48 83 EC 20 48 8B F9 48 8B 09 48 85 C9 0F 84 8D 00 00 00 33 C0 48 89 5C 24 30 F2 48 0F 38 F1 C1");

		Offsets::MapSignature("Entity::AddComponent", "48 89 5C 24 10 48 89 6C 24 18 48 89 74 24 20 57 41 56 41 57 48 83 EC 40 48 8B F9 48 8B F2 48 8B 4A 30");
		Offsets::MapSignature("Entity::RemoveComponent", "48 8B C4 56 57 48 83 EC 68 83 B9 30 02 00 00 00 48 8B F2 48 8B F9");
		Offsets::MapSignature("Entity::SetFaction", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 40 48 8B F9 48 8B F2 48 83 C1 50 FF 15 ? ? ? ? 85 C0 75 0A 48 8D 4F 50 FF 15 ? ? ? ? 48 8B 8F 28 02 00 00 48 3B CE");

		Offsets::MapSignature("HwBuffer::Map", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 48 89 7C 24 20 41 54 41 56 41 57 48 83 EC 20 48 8B 01 45 8B F1 8B 6C 24 60");
		Offsets::MapSignature("HwBuffer::Unmap", "40 53 48 83 EC 30 44 8B 42 08 48 8D 44 24 40 44 89 44 24 40 48 8B DA");

		Offsets::MapSignature("NodeGraph::ExportedReloadLastSaveGame", "40 53 48 83 EC 40 0F 29 74 24 30 B9 20 00 00 00 0F 28 F0 E8");
		Offsets::MapSignature("NodeGraph::ExportedCreateSaveGame", "48 83 EC 48 44 0F B6 D1 48 8B ? ? ? ? ? 48 85 C9 74 3B 32 C0 41 B9 01 00 00 00 84 D2");
		Offsets::MapSignature("NodeGraph::ExportedSpawnpointSpawn", "48 85 C9 74 25 80 B9 80 01 00 00 00 75 1C 80 B9 81 01 00 00 00 75 13");
		Offsets::MapSignature("NodeGraph::ExportedIntersectLine", "48 8B C4 48 89 58 08 48 89 70 10 48 89 78 18 55 41 54 41 55 41 56 41 57 48 8D 68 D1 48 81 EC E0 00 00 00 0F 10 01");

		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		Offsets::MapSignature("RTTI::CreateObject", "48 89 5C 24 20 57 48 83 EC 20 44 0F B6 49 04 4C 8D ? ? ? ? ? 45 33 C0 48 8B D9 41 83 F9 06");

		Offsets::MapSignature("StreamingRefHandle::Dtor", "40 53 48 83 EC 20 48 8B CA 48 8B DA E8 ? ? ? ? 48 8B 1B");
		Offsets::MapSignature("StreamingRefHandle::AssignFromOther", "48 89 5C 24 08 57 48 83 EC 20 48 8B 02 48 8B FA 48 8B D9 48 39 01 0F 84");

		Offsets::MapSignature("Player::GetLocalPlayer", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 30 48 63 E9 85 C9 0F 85 E2 00 00 00");
		Offsets::MapSignature("SwapChainDX12::Present", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 40 48 8B 05 ? ? ? ? 48 8B D9");
		Offsets::MapSignature("PackfileDevice::MountArchive", "44 89 44 24 18 48 89 54 24 10 48 89 4C 24 08 55 53 56 57 41 56");
		Offsets::MapSignature("LocalizedTextResource::GetTranslation", "48 89 5C 24 10 57 48 83 EC 20 48 8B F9 48 8B DA 48 8B 0D ? ? ? ? 48 8B 01");
		Offsets::MapSignature("RTTIRefObject::DecRef", "40 53 48 83 EC 20 48 8B D9 B8 FF FF FF FF F0 0F C1 41 08 25 FF FF FF 00 83 F8 01");
		Offsets::MapSignature("WeatherSystem::SetWeatherOverride", "48 8B C4 53 48 81 EC 90 00 00 00 48 89 70 10 48 8D 99 30 01 00 00 48 89 78 18 48 8B F9");
		Offsets::MapSignature("WorldState::SetTimeOfDay", "F3 0F 10 99 98 00 00 00 0F 57 C0 0F 2F D0 76 3B 0F 2F CB 72 06");
		Offsets::MapSignature("ToggleDamageLogging", "40 53 48 83 EC 20 84 D2 0F 84 B9 00 00 00 48 8B 05 ? ? ? ? 33 DB");
		Offsets::MapSignature("CoreFileManager::RegisterEventListener", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B F9 48 8B F2 48 83 C1 18 FF 15 ? ? ? ? 84 C0 75 0A 48 8D 4F 18 FF 15 ? ? ? ? 48 8D 4F 28");
		Offsets::MapSignature("RTTIObjectTweaker::VisitObjectPath", "4C 8B DC 55 53 56 41 55 41 56 49 8D 6B B1 48 81 EC 90 00 00 00");
		Offsets::MapSignature("HeapAllocator::Free", "48 83 EC 28 48 85 C9 0F 84 ? ? ? ? 8B ? ? ? ? ? 65 48 8B 04 25 58 00 00 00 48 89 5C 24 30 48 89 7C 24 20");
		Offsets::MapSignature("BodyVariantRuntimeComponent::SetVariantByUUID", "40 56 41 54 41 55 48 83 EC 70 4C 8D A1 80 00 00 00");

		// Hooks
		Offsets::MapSignature("SwapChainDX12PresentHookLoc", "E8 ? ? ? ? 49 8B 76 08 33 FF 41 39 3E 7E 17");
		Offsets::MapSignature("PostLoadObjectHookLoc", "48 8B 53 38 FF 50 10 E9 ? ? ? ? 48 8B CB");
		Offsets::MapSignature("PreLoadObjectHookLoc", "48 8B 53 38 FF 50 08 48 8B 54 24 20 48 8D");
		Offsets::MapSignature("PackfileDeviceMountArchiveHookLoc1", "E8 ? ? ? ? 48 8D 4D E7 E8 ? ? ? ? 48 83 C3 08 48 3B DF");
		Offsets::MapSignature("PackfileDeviceMountArchiveHookLoc2", "E8 ? ? ? ? 41 FF 46 40 48 8D 4D EF E8");
		Offsets::MapSignature("SetCameraTransformHookLoc", "E8 ? ? ? ? 48 8B 8F 38 02 00 00 48 8D ? ? ? ? ? E8");
		Offsets::MapSignature("SetGameModuleTimescaleHookLoc", "E8 ? ? ? ? 48 8B ? ? ? ? ? 4C 8D 44 24 20 48 8D 54 24 40 4C 89 7C 24 20");
		Offsets::MapSignature("EntitlementOverridePatchLoc", "40 53 41 56 41 57 48 83 EC 20 48 83 39 00 4C 8B FA 4C 8B F1 75 14 BB FF FF FF FF 3B DB 0F 95 C0");
		Offsets::MapSignature("FacepaintIsUnlockedPatchLoc", "4C 8B 01 85 D2 78 29 41 8B 40 28 FF C8 3B D0 7F 1F");
		Offsets::MapSignature("FocusModelIsUnlockedPatchLoc1", "4C 8B 01 85 D2 78 29 41 8B 40 40 FF C8 3B D0 7F 1F");
		Offsets::MapSignature("FocusModelIsUnlockedPatchLoc2", "7F 09 4A 8B 04 C8 C3 49 8B 40 48 48 8B 00 C3 33 C0 C3");
		Offsets::MapSignature("ProcessAIJobHookLoc1", "48 8B 01 FF 90 00 01 00 00 48 8B 03 48 8D", -0xF8);
		Offsets::MapSignature("ProcessAIJobHookLoc2", "48 8B 01 FF 90 00 01 00 00 48 8B 03 48 8B 5C 24 30", -0xE6);
		Offsets::MapSignature("SetBodyVariantHookLoc1", "E8 ? ? ? ? 48 8B 5C 24 58 48 83 C4 38 5F 5D C3");
		Offsets::MapSignature("SendCrashReportDialogPatchLoc", "74 14 FF 15 ? ? ? ? 48 8B C8 BA FF FF FF FF FF 15");
		Offsets::MapAddress("CutsceneAspectRatioPatchLoc", offsetFromInstruction("F3 0F 10 ? ? ? ? ? F3 0F 5C C7 F3 0F 11 44 24 58 8B 44 24 58 25 00 00 80 7F", 0x4));

		// Globals
		Offsets::MapAddress("ExportedSymbolGroupArray", offsetFromInstruction("48 8B C2 4C 8D ? ? ? ? ? 48 8D ? ? ? ? ? 48 8D ? ? ? ? ? 48 FF E0", 0x6));
		Offsets::MapAddress("Application::Instance", offsetFromInstruction("48 89 5C 24 08 57 48 83 EC 20 48 83 79 28 00 48 8B DA 48 8B F9 74 5A 33 D2 48 8D", 0x1C));
		Offsets::MapAddress("RenderingDeviceDX12::Instance", offsetFromInstruction("48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 83 B9 38 01 00 00 00 48 8D ? ? ? ? ? 48 89 01 48 8B F9 74 13 48 8D 91 30 01 00 00 48 8D", 0x30));
		Offsets::MapAddress("StreamingManager::Instance", offsetFromInstruction("48 8B ? ? ? ? ? 45 33 C0 48 8D 50 30 4C 8B 09 41 FF 51 30 48 8B CB", 0x3));

		// Structure offsets
		Offsets::MapSignature("RenderingConfigDescriptorHeapOffsetPtr", "4C 8D 45 98 45 0F AF CF 41 8B D5 49 8B CE 4C 03 08 4C 89 4E F4", -0x10);
		Offsets::MapSignature("RenderingConfigDescriptorHandleIncrementSizeOffsetPtr", "4C 8D 45 98 45 0F AF CF 41 8B D5 49 8B CE 4C 03 08 4C 89 4E F4", -0x4);
		Offsets::MapSignature("RenderingConfigDescriptorCommandQueueOffsetPtr", "48 89 7D 30 48 8B 01 FF 50 78 85 C0 79 14", -0x4);
	}
}

void ApplyHooks(GameType Game)
{
	auto [moduleBase, _] = Offsets::GetModule();

	if (Game == GameType::DeathStranding)
	{
		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		XUtil::DetourCall(moduleBase + 0x17BC7AA, &hk_call_1417BC7AA);
		XUtil::DetourCall(moduleBase + 0x17B52CF, &hk_call_1417B52CF);

		// Kill SteamAPI_RestartAppIfNecessary check
		XUtil::PatchMemoryNop(moduleBase + 0x16A5864, 9);
	}
	else if (Game == GameType::HorizonZeroDawn)
	{
		InternalModConfig::InitializeDefault();

		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		// Prevent crash reports from being submitted to Guerrilla if they're not caused by the vanilla game
		XUtil::PatchMemory(Offsets::ResolveID<"SendCrashReportDialogPatchLoc">(), { 0x90, 0x90 });

		// Redirect internal log prints
		Detours::IATHook(moduleBase, "api-ms-win-crt-stdio-l1-1-0.dll", "__acrt_iob_func", reinterpret_cast<uintptr_t>(&LogHooks::hk___acrt_iob_func));
		Detours::IATHook(moduleBase, "api-ms-win-crt-stdio-l1-1-0.dll", "fwrite", reinterpret_cast<uintptr_t>(&LogHooks::hk_fwrite));
		Detours::IATHook(moduleBase, "api-ms-win-crt-stdio-l1-1-0.dll", "__stdio_common_vfprintf", reinterpret_cast<uintptr_t>(&LogHooks::hk___stdio_common_vfprintf));

		XUtil::DetourCall(Offsets::ResolveID<"PackfileDeviceMountArchiveHookLoc1">(), &LogHooks::PackfileDevice_MountArchive);
		XUtil::DetourCall(Offsets::ResolveID<"PackfileDeviceMountArchiveHookLoc2">(), &LogHooks::PackfileDevice_MountArchive);

		XUtil::DetourCall(Offsets::ResolveID<"PreLoadObjectHookLoc">(), &LogHooks::PreLoadObjectHook);
		XUtil::DetourCall(Offsets::ResolveID<"PostLoadObjectHookLoc">(), &LogHooks::PostLoadObjectHook);

		//XUtil::DetourJump(moduleBase + 0x0605830, &LogHooks::NodeGraphAlert);
		//XUtil::DetourJump(moduleBase + 0x0605840, &LogHooks::NodeGraphAlertWithName);
		//XUtil::DetourJump(moduleBase + 0x0605860, &LogHooks::NodeGraphTrace);

		// Rendering
		XUtil::DetourCall(Offsets::ResolveID<"SwapChainDX12PresentHookLoc">(), &hk_SwapChainDX12_Present);

		// Function to set 3rd person camera position
		XUtil::DetourCall(Offsets::ResolveID<"SetCameraTransformHookLoc">(), &hk_call_1413AB8FC);

		// Override GameModule timescale function
		XUtil::DetourCall(Offsets::ResolveID<"SetGameModuleTimescaleHookLoc">(), &hk_call_1411E3210);

		// Override AI processing function
		original_ProcessAIJob1_140DBD530 = XUtil::DetourJump(Offsets::ResolveID<"ProcessAIJobHookLoc1">(), &hk_ProcessAIJob1_140DBD530);
		original_ProcessAIJob2_140DBD670 = XUtil::DetourJump(Offsets::ResolveID<"ProcessAIJobHookLoc2">(), &hk_ProcessAIJob2_140DBD670);

		// Override current player body variant
		XUtil::DetourCall(Offsets::ResolveID<"SetBodyVariantHookLoc1">(), &hk_call_1414A4418);

		// Override the WorldState SetTimeOfDay function
		//XUtil::DetourJump(Offsets::ResolveID<"WorldState::SetTimeOfDay">(), &WorldState::SetTimeOfDay);

		// Enable all entitlements
		if (ModConfiguration.UnlockEntitlementExtras)
			XUtil::PatchMemory(Offsets::ResolveID<"EntitlementOverridePatchLoc">(), { 0xB0, 0x01, 0xC3 });

		// Enable all focus models and facepaints
		if (ModConfiguration.UnlockNGPExtras)
		{
			XUtil::PatchMemory(Offsets::ResolveID<"FacepaintIsUnlockedPatchLoc">(), { 0xB0, 0x01, 0xC3 });
			XUtil::PatchMemory(Offsets::ResolveID<"FocusModelIsUnlockedPatchLoc1">(), { 0xB0, 0x01, 0xC3 });
			XUtil::PatchMemory(Offsets::ResolveID<"FocusModelIsUnlockedPatchLoc2">(), { 0x90, 0x90 });
		}

		// Force a set aspect ratio during realtime cutscenes
		if (ModConfiguration.CutsceneAspectRatio > 0)
			XUtil::PatchMemory(Offsets::ResolveID<"CutsceneAspectRatioPatchLoc">(), reinterpret_cast<uint8_t *>(&ModConfiguration.CutsceneAspectRatio), sizeof(float));
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		wchar_t modulePath[MAX_PATH] {};
		GetModuleFileNameW(GetModuleHandleW(nullptr), modulePath, static_cast<DWORD>(std::size(modulePath)));

		wchar_t executableName[MAX_PATH] {};
		_wsplitpath_s(modulePath, nullptr, 0, nullptr, 0, executableName, std::size(executableName), nullptr, 0);

		auto gameType = GameType::Invalid;

		if (!_wcsicmp(executableName, L"ds"))
			gameType = GameType::DeathStranding;
		else if (!_wcsicmp(executableName, L"HorizonZeroDawn"))
			gameType = GameType::HorizonZeroDawn;

		try
		{
			if (gameType == GameType::Invalid)
				throw std::runtime_error("Trying to use winhttp.dll with an unsupported game. 'ds.exe' or 'HorizonZeroDawn.exe' are expected");

			LoadSignatures(gameType);
			ApplyHooks(gameType);
		}
		catch (const std::exception& e)
		{
			wchar_t buffer[2048];

			swprintf_s(buffer,
				L"An exception has occurred on startup: %hs. Failed to initialize Horizon Zero Dawn mod. Note that the December 2021 patch"
				" (version 1.11) is the minimum requirement.\n\nExecutable path: %ws", e.what(), modulePath);

			MessageBoxW(nullptr, buffer, L"Error", MB_ICONERROR);
		}
	}

	return TRUE;
}
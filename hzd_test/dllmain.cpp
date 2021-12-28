#include <atomic>
#include <stdlib.h>

#include "HRZ/Core/RTTIBinaryReader.h"
#include "HRZ/Core/PrefetchList.h"
#include "HRZ/PGraphics3D/SwapChainDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "HRZ/DebugUI/MainMenuBar.h"

#include "HRZ/Core/StreamingManager.h"
#include "HRZ/Core/CoreFileManager.h"
#include "HRZ/Core/WorldTransform.h"
#include "HRZ/Core/CameraEntity.h"
#include "HRZ/Core/GameModule.h"

#include "RTTI/MSRTTI.h"
#include "RTTI/RTTIScanner.h"

#include "ModConfig.h"
#include "ModCoreEvents.h"
#include "common.h"

using namespace HRZ;

void PreLoadObjectHook(RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	Reader->OnObjectPreInit(object);

	if (false)
	{
		// Subtract 12 for the core entry header length (uint64, uint)
		uint64_t assetOffset = Reader->GetStreamPosition() - 12;

		DebugUI::LogWindow::AddLog("[Asset] Loading %s at [offset %lld, address 0x%p] (%s)\n", object->GetRTTI()->GetSymbolName().c_str(), assetOffset, object, Reader->m_FullFilePath.c_str());
	}
}

void PostLoadObjectHook(RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	auto rtti = object->GetRTTI();

	Reader->OnObjectPostInit(object);

	if (false)
	{
		DebugUI::LogWindow::AddLog("[Asset] Finished loading %s at [offset %lld, address 0x%p] (%s)", rtti->GetSymbolName().c_str(), Reader->GetStreamPosition(), object, Reader->m_FullFilePath.c_str());

		// Dump the name
		if (String assetName; object->GetMemberValue("Name", &assetName) && !assetName.empty())
			DebugUI::LogWindow::AddLog(" (%s)\n", assetName.c_str());
		else
			DebugUI::LogWindow::AddLog("\n");

		// Log prefetch values for debugging purposes
		if (auto prefetch = RTTI::Cast<PrefetchList>(object); prefetch)
		{
			DebugUI::LogWindow::AddLog("[Asset] Prefetch load done\n");
			DebugUI::LogWindow::AddLog("[Asset] Total files: %d\n", prefetch->m_Files.size());
			DebugUI::LogWindow::AddLog("[Asset] Total sizes: %d\n", prefetch->m_Sizes.size());
			DebugUI::LogWindow::AddLog("[Asset] Total links: %d\n", prefetch->m_Links.size());
		}
	}
}

void NodeGraphAlert(const char *Message, bool Unknown)
{
	DebugUI::LogWindow::AddLog("[Alert] %s\n", Message);
}

void NodeGraphAlertWithName(const char *Category, const char *Severity, const char *Message, bool Unknown)
{
	DebugUI::LogWindow::AddLog("[Alert] [%s] [%s] %s\n", Category, Severity, Message);
}

void NodeGraphTrace(const char *UUID, const char *Message)
{
	DebugUI::LogWindow::AddLog("[Trace] [%s] %s\n", UUID, Message);
}

void InternalEngineLog(const char *Format, ...)
{
	va_list va;
	char buffer[2048];

	va_start(va, Format);
	int len = _vsnprintf_s(buffer, _TRUNCATE, Format, va);
	va_end(va);

	if (len > 1 && buffer[len - 1] == '\n')
		DebugUI::LogWindow::AddLog("[Engine] %s", buffer);
	else
		DebugUI::LogWindow::AddLog("[Engine] %s\n", buffer);
}

void PackfileDevice_MountArchive(class PackFileDevice *Device, const String& BinPath, uint32_t Priority)
{
	Offsets::CallID<"PackfileDevice::MountArchive", void(*)(PackFileDevice *, const String&, uint32_t)>(Device, BinPath, Priority);

	DebugUI::LogWindow::AddLog("[PackfileDevice] Mounted archive %s with priority %u\n", BinPath.c_str(), Priority);
}

bool hk_SwapChainDX12_Present(SwapChainDX12 *SwapChain)
{
	static bool init = [&]()
	{
		DebugUI::Initialize(SwapChain);
		return true;
	}();

	static bool initCoreEvents = false;
	auto streamingManager = HRZ::StreamingManager::Instance();

	if (streamingManager && !initCoreEvents)
	{
		streamingManager->m_CoreFileManager->RegisterEventListener(ModCoreEvents::Instance());
		initCoreEvents = true;
	}

	/*
	uintptr_t playerProfile = *Offsets::Resolve<uintptr_t *>(0x7133A40);

	if (playerProfile)
	{
		// Set the highest NG+ difficulty that has been unlocked. Used for custom face paints and focus models.
		*(int *)(playerProfile + 0x25C) = 5;
	}*/

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

void hk_call_1417BC7AA()
{
	RTTIScanner::ExportAll("C:\\ds_rtti_export", "DS");
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

void LoadSignatures(GameType Game)
{
	auto [moduleBase, moduleEnd] = Offsets::GetModule();

	auto offsetFromInstruction = [&](const char *Signature, uint32_t Add)
	{
		auto addr = XUtil::FindPattern(moduleBase, moduleEnd - moduleBase, Signature);
		auto relOffset = *reinterpret_cast<int32_t *>(addr + Add) + sizeof(int32_t);

		return addr + Add + relOffset - moduleBase;;
	};

	if (Game == GameType::DeathStranding)
	{
		// Functions
		Offsets::MapSignature("String::CtorCString", "40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
		Offsets::MapSignature("String::Dtor", "40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");

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

		Offsets::MapSignature("Player::GetLocalPlayer", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 30 48 63 E9 85 C9 0F 85 E2 00 00 00");

		Offsets::MapSignature("Entity::AddComponent", "48 89 5C 24 10 48 89 6C 24 18 48 89 74 24 20 57 41 56 41 57 48 83 EC 40 48 8B F9 48 8B F2 48 8B 4A 30");
		Offsets::MapSignature("Entity::RemoveComponent", "48 8B C4 56 57 48 83 EC 68 83 B9 30 02 00 00 00 48 8B F2 48 8B F9");

		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		Offsets::MapSignature("SwapChainDX12::Present", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 40 48 8B 05 ? ? ? ? 48 8B D9");
		Offsets::MapSignature("PackfileDevice::MountArchive", "44 89 44 24 18 48 89 54 24 10 48 89 4C 24 08 55 53 56 57 41 56");
		Offsets::MapSignature("LocalizedTextResource::GetTranslation", "48 89 5C 24 10 57 48 83 EC 20 48 8B F9 48 8B DA 48 8B 0D ? ? ? ? 48 8B 01");
		Offsets::MapSignature("RTTIRefObject::DecRef", "40 53 48 83 EC 20 48 8B D9 B8 FF FF FF FF F0 0F C1 41 08 25 FF FF FF 00 83 F8 01");
		Offsets::MapSignature("WeatherSystem::SetWeatherOverride", "48 8B C4 53 48 81 EC 90 00 00 00 48 89 70 10 48 8D 99 30 01 00 00 48 89 78 18 48 8B F9");
		Offsets::MapSignature("WorldState::SetTimeOfDay", "F3 0F 10 99 98 00 00 00 0F 57 C0 0F 2F D0 76 3B 0F 2F CB 72 06");
		Offsets::MapSignature("ToggleDamageLogging", "40 53 48 83 EC 20 84 D2 0F 84 B9 00 00 00 48 8B 05 ? ? ? ? 33 DB");

		// Globals
		Offsets::MapAddress("ExportedSymbolGroupArray", offsetFromInstruction("48 8B C2 4C 8D ? ? ? ? ? 48 8D ? ? ? ? ? 48 8D ? ? ? ? ? 48 FF E0", 6));
		Offsets::MapAddress("Application::Instance", offsetFromInstruction("48 89 5C 24 08 57 48 83 EC 20 48 83 79 28 00 48 8B DA 48 8B F9 74 5A 33 D2 48 8D", 28));
		Offsets::MapAddress("RenderingDeviceDX12::Instance", offsetFromInstruction("48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 83 B9 38 01 00 00 00 48 8D ? ? ? ? ? 48 89 01 48 8B F9 74 13 48 8D 91 30 01 00 00 48 8D", 48));
	}
}

void ApplyHooks(GameType Game)
{
	auto [moduleBase, _] = Offsets::GetModule();

	if (Game == GameType::DeathStranding)
	{
		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		XUtil::DetourCall(moduleBase + 0x17BC7AA, hk_call_1417BC7AA);
		XUtil::DetourCall(moduleBase + 0x17B52CF, hk_call_1417B52CF);

		// Kill SteamAPI_RestartAppIfNecessary check
		XUtil::PatchMemoryNop(moduleBase + 0x16A5864, 9);
	}
	else if (Game == GameType::HorizonZeroDawn)
	{
		ModConfig::InitializeDefault();

		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		XUtil::DetourCall(moduleBase + 0x023CFD0, &hk_SwapChainDX12_Present);

		XUtil::PatchMemoryNop(moduleBase + 0x04A9762, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(moduleBase + 0x04A9762, &PostLoadObjectHook);

		XUtil::PatchMemoryNop(moduleBase + 0x04A9735, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(moduleBase + 0x04A9735, &PreLoadObjectHook);

		XUtil::DetourJump(moduleBase + 0x0605830, NodeGraphAlert);
		XUtil::DetourJump(moduleBase + 0x0605840, NodeGraphAlertWithName);
		XUtil::DetourJump(moduleBase + 0x0605860, NodeGraphTrace);
		XUtil::DetourJump(moduleBase + 0x031C4D0, InternalEngineLog);
		XUtil::DetourJump(moduleBase + 0x0372D60, InternalEngineLog);
		XUtil::DetourJump(moduleBase + 0x0372B50, InternalEngineLog);

		XUtil::DetourCall(moduleBase + 0x01409B2, PackfileDevice_MountArchive);
		XUtil::DetourCall(moduleBase + 0x0140A1E, PackfileDevice_MountArchive);

		// Function to set 3rd person camera position
		XUtil::DetourCall(moduleBase + 0x13AF84C, hk_call_1413AB8FC);

		// Override GameModule timescale function
		XUtil::DetourCall(moduleBase + 0x11E3210, hk_call_1411E3210);

		// Kill one of the out of bounds checks
		if (ModConfiguration.UnlockMapBorders)
		{
			XUtil::PatchMemoryNop(moduleBase + 0x0EF937D, 2);
		}

		// Enable all entitlements
		if (ModConfiguration.UnlockEntitlementExtras)
		{
			XUtil::PatchMemory(moduleBase + 0x0ED5D50, { 0xB0, 0x01, 0xC3 });
		}
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		wchar_t modulePath[MAX_PATH] {};
		GetModuleFileNameW(GetModuleHandleW(nullptr), modulePath, std::size(modulePath));

		wchar_t executableName[MAX_PATH] {};
		_wsplitpath_s(modulePath, nullptr, 0, nullptr, 0, executableName, std::size(executableName), nullptr, 0);

		auto gameType = GameType::Invalid;

		if (!_wcsicmp(executableName, L"ds"))
			gameType = GameType::DeathStranding;
		else if (!_wcsicmp(executableName, L"HorizonZeroDawn"))
			gameType = GameType::HorizonZeroDawn;

		LoadSignatures(gameType);
		ApplyHooks(gameType);
	}

	return TRUE;
}
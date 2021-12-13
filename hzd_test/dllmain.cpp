#include <atomic>
#include <stdlib.h>

#include "HRZ/Core/RTTIBinaryReader.h"
#include "HRZ/Core/PrefetchList.h"
#include "HRZ/PGraphics3D/SwapChainDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "HRZ/DebugUI/MainMenuBar.h"

#include "RTTI/MSRTTI.h"
#include "RTTI/RTTIIDAExporter.h"
#include "RTTI/RTTIScanner.h"

#include "common.h"

using namespace HRZ;

constexpr auto test = GGUUID::Parse("40e36691-5fd0-4a79-b3b3-87b2a3d13e9c");
constexpr auto test2 = GGUUID::Parse("{40e36691-5fd0-4a79-b3b3-87b2a3d13e9c}");
constexpr auto test3 = GGUUID::Parse("{40E36691-5FD0-4A79-B3B3-87B2A3D13E9C}");

void PreLoadObjectHook(RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	Reader->OnObjectPreInit(object);

	// Subtract 12 for the core entry header length (uint64, uint)
	uint64_t assetOffset = Reader->GetStreamPosition() - 12;

	DebugUI::LogWindow::AddLog("[Asset] Loading %s at [offset %lld, address 0x%p] (%s)\n", object->GetRTTI()->GetSymbolName().c_str(), assetOffset, object, Reader->m_FullFilePath.c_str());
}

void PostLoadObjectHook(RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	Reader->OnObjectPostInit(object);

	if (auto prefetch = RTTI::Cast<PrefetchList>(object); prefetch)
	{
		DebugUI::LogWindow::AddLog("[Asset] Finished prefetch load: %lld\n", Reader->GetStreamPosition());
		DebugUI::LogWindow::AddLog("[Asset] Total files: %d\n", prefetch->m_Files.size());
		DebugUI::LogWindow::AddLog("[Asset] Total sizes: %d\n", prefetch->m_Sizes.size());
		DebugUI::LogWindow::AddLog("[Asset] Total links: %d\n", prefetch->m_Links.size());
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

void PackFileDevice_MountArchive(class PackFileDevice *Device, const String& BinPath, uint32_t Priority)
{
	Offsets::CallID<"PackFileDevice::MountArchive", void(*)(PackFileDevice *, const String&, uint32_t)>(Device, BinPath, Priority);

	DebugUI::LogWindow::AddLog("[PackFileDevice] Mounted archive %s with priority %u\n", BinPath.c_str(), Priority);
}

bool hk_SwapChainDX12_Present(SwapChainDX12 *SwapChain)
{
	static bool init = [&]()
	{
		DebugUI::Initialize(SwapChain);
		return true;
	}();

	DebugUI::RenderUI();
	DebugUI::RenderUID3D(SwapChain);
	return SwapChain->Present();
}

void hk_call_1413AB8FC(class CameraEntity *Entity, WorldTransform& Transform)
{
	if (DebugUI::MainMenuBar::m_FreeCamMode == DebugUI::MainMenuBar::FreeCamMode::Free)
		return;

	Entity->SetTransform(Transform);
}

void LoadSignatures()
{
	wchar_t modulePath[MAX_PATH];
	GetModuleFileNameW(GetModuleHandleW(nullptr), modulePath, MAX_PATH);

	wchar_t executableName[MAX_PATH];
	_wcslwr_s(modulePath);
	_wsplitpath_s(modulePath, nullptr, 0, nullptr, 0, executableName, ARRAYSIZE(executableName), nullptr, 0);

	if (!wcscmp(executableName, L"ds"))
		g_GameType = GameType::DeathStranding;
	else if (!wcscmp(executableName, L"horizonzerodawn"))
		g_GameType = GameType::HorizonZeroDawn;
	else
		__debugbreak();

	auto [moduleBase, moduleEnd] = Offsets::GetModule();

	auto scan = [&](const char *Signature)
	{
		return XUtil::FindPattern(moduleBase, moduleEnd - moduleBase, Signature) - moduleBase;
	};

	if (g_GameType == GameType::DeathStranding)
	{
		// 1.04
		Offsets::MapSignature("String::String", "40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
		Offsets::MapSignature("String::~String", "40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");

		Offsets::MapAddress("ExportedSymbolGroupArray", 0x4870440);
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		// Functions
		Offsets::MapSignature("String::CtorCString", "48 89 5C 24 10 48 89 6C 24 18 48 89 7C 24 20 41 56 48 83 EC 20 33 FF 48 8B EA 48 89 39 4C 8B F1 48 C7 C3 FF FF FF FF");
		Offsets::MapSignature("String::Dtor", "40 53 48 83 EC 20 48 8B 19 48 85 DB 74 37 48 83 C3 F0 48 8D");
		Offsets::MapSignature("String::AssignFromOther", "48 89 5C 24 10 48 89 74 24 18 57 48 83 EC 20 48 8B 39 48 8B F2 48 8B D9 48 3B 3A 74 54");

		Offsets::MapSignature("WString::Ctor", "B8 01 00 00 00 F0 0F C1 ? ? ? ? 02 48 8D 15 ? ? ? ? 48 8B C1 48 89 11 C3");
		Offsets::MapSignature("WString::CtorCString", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 33 ED 48 8B F2 8B DD 48 89 29");
		Offsets::MapSignature("WString::Dtor", "48 8B 09 B8 FF FF FF FF F0 0F C1 01 83 F8 01 0F 84 ? ? ? ? C3");
		Offsets::MapSignature("WString::AssignFromOther", "48 89 5C 24 08 57 48 83 EC 20 48 8B D9 48 8B FA 48 8B 09 48 3B 0A 74 22");
		Offsets::MapSignature("WString::EncodeUTF8", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B 01 48 8B F2 48 8B D9 48 8B CE 8B 78 08");

		Offsets::MapSignature("Player::GetLocalPlayer", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 30 48 63 E9 85 C9 0F 85 E2 00 00 00");
		Offsets::MapAddress("Player::GetLastActivatedCamera", 0x0C29FC0);

		Offsets::MapSignature("Entity::AddComponent", "48 89 5C 24 10 48 89 6C 24 18 48 89 74 24 20 57 41 56 41 57 48 83 EC 40 48 8B F9 48 8B F2 48 8B 4A 30");
		Offsets::MapSignature("Entity::RemoveComponent", "48 8B C4 56 57 48 83 EC 68 83 B9 30 02 00 00 00 48 8B F2 48 8B F9");

		Offsets::MapSignature("SlowMotionManager::AddTimescaleModifier", "48 8B C4 53 56 48 83 EC 68 8B 31 48 8B D9 F3 0F 10");
		Offsets::MapSignature("SlowMotionManager::RemoveTimescaleModifier", "48 89 5C 24 10 56 48 83 EC 20 48 63 41 08 45 33 C0 48 8B F2 48 8B D9 85 C0");

		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		Offsets::MapSignature("SwapChainDX12::Present", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 40 48 8B 05 ? ? ? ? 48 8B D9");
		Offsets::MapSignature("PackFileDevice::MountArchive", "44 89 44 24 18 48 89 54 24 10 48 89 4C 24 08 55 53 56 57 41 56");
		Offsets::MapSignature("LocalizedTextResource::GetTranslation", "48 89 5C 24 10 57 48 83 EC 20 48 8B F9 48 8B DA 48 8B 0D ? ? ? ? 48 8B 01");
		Offsets::MapSignature("RTTIRefObject::DecRef", "40 53 48 83 EC 20 48 8B D9 B8 FF FF FF FF F0 0F C1 41 08 25 FF FF FF 00 83 F8 01");
		Offsets::MapSignature("WeatherSystem::SetWeatherOverride", "48 8B C4 53 48 81 EC 90 00 00 00 48 89 70 10 48 8D 99 30 01 00 00 48 89 78 18 48 8B F9");
		Offsets::MapSignature("WorldState::SetTimeOfDay", "F3 0F 10 99 98 00 00 00 0F 57 C0 0F 2F D0 76 3B 0F 2F CB 72 06");
		Offsets::MapSignature("ToggleDamageLogging", "40 53 48 83 EC 20 84 D2 0F 84 B9 00 00 00 48 8B 05 ? ? ? ? 33 DB");

		// Globals
		Offsets::MapAddress("ExportedSymbolGroupArray", 0x2A256F0);
		Offsets::MapAddress("Application::Instance", 0x7132AF0);
		Offsets::MapAddress("RenderingDeviceDX12::Instance", 0x2D39E80);
	}
}

void ApplyHooks()
{
	auto [moduleBase, _] = Offsets::GetModule();

	if (g_GameType == GameType::DeathStranding)
	{
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
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

		XUtil::DetourCall(moduleBase + 0x01409B2, PackFileDevice_MountArchive);
		XUtil::DetourCall(moduleBase + 0x0140A1E, PackFileDevice_MountArchive);

		// Function to set 3rd person camera position
		XUtil::DetourCall(moduleBase + 0x13AF47C, hk_call_1413AB8FC);

		// Kill one of the out of bounds checks
		XUtil::PatchMemoryNop(moduleBase + 0x0EF92FD, 2);
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		LoadSignatures();
		ApplyHooks();
	}

	return TRUE;
}
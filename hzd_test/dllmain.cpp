#include <atomic>
#include <stdlib.h>

#include "HRZ/Core/RTTIBinaryReader.h"
#include "HRZ/Core/PrefetchList.h"
#include "HRZ/PGraphics3D/SwapChainDX12.h"
#include "HRZ/DebugUI/DebugUI.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "HRZ/DebugUI/MainMenuBar.h"

#include "common.h"
#include "MSRTTI.h"
#include "RTTIIDAExporter.h"
#include "RTTIScanner.h"

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
	Offsets::Call<0x0147790, void(*)(PackFileDevice *, const String&, uint32_t)>(Device, BinPath, Priority);

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

	Offsets::Call<0x0BB41A0, void(*)(CameraEntity *, WorldTransform&)>(Entity, Transform);
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
		strcpy_s(g_GamePrefix, "DS");

		Offsets::MapSignature("String::String", "40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
		Offsets::MapSignature("String::~String", "40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");

		// 1.04
		Offsets::MapAddress("ExportedSymbolGroupArray", 0x4870440);
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		strcpy_s(g_GamePrefix, "HZD");

		Offsets::MapSignature("String::Assign", "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B 39 48");
		Offsets::MapSignature("String::Concat", "40 53 48 83 EC 20 80 3A");
		Offsets::MapSignature("String::CRC32", "48 8B 11 8B 42 F4");
		Offsets::MapSignature("RTTI::GetCoreBinaryTypeId", "48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		Offsets::MapSignature("ShaderCachePresent", "48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D");
		Offsets::MapAddress("Player::GetLocalPlayer", 0x0C2F710);
		Offsets::MapAddress("SwapChainDX12::Present", 0x02027A0);
		Offsets::MapAddress("Entity::AddComponent", 0x0B97960);
		Offsets::MapAddress("Entity::RemoveComponent", 0x0BAF7C0);
		Offsets::MapAddress("SlowMotionManager::AddTimescaleModifier", 0x11CB770);
		Offsets::MapAddress("SlowMotionManager::RemoveTimescaleModifier", 0x11CA9F0);

		// 1.0.10.5
		Offsets::MapAddress("ExportedSymbolGroupArray", 0x2A142F0);
	}
}

void ApplyHooks()
{
	auto [moduleBase, moduleEnd] = Offsets::GetModule();

	if (g_GameType == GameType::DeathStranding)
	{
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		// Intercept fullgame load
		XUtil::PatchMemory(moduleBase + 0x0237557, { 0x90, 0x90 });// Force enable debug command list names (render order)
		XUtil::PatchMemory(moduleBase + 0x01D41BC, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force enable debug command list names (command list)

		XUtil::PatchMemory(moduleBase + 0x0379DF0, { 0xC3 });// Steam being retarded and they're not checking a nullptr when offline
		XUtil::PatchMemory(moduleBase + 0x037AD22, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force disable exception handler

		XUtil::DetourCall(moduleBase + 0x023BBD0, &hk_SwapChainDX12_Present);

		XUtil::PatchMemoryNop(moduleBase + 0x04A7F82, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(moduleBase + 0x04A7F82, &PostLoadObjectHook);

		XUtil::PatchMemoryNop(moduleBase + 0x04A7F55, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(moduleBase + 0x04A7F55, &PreLoadObjectHook);

		XUtil::DetourJump(moduleBase + 0x0603D00, NodeGraphAlert);
		XUtil::DetourJump(moduleBase + 0x0603D10, NodeGraphAlertWithName);
		XUtil::DetourJump(moduleBase + 0x0603D30, NodeGraphTrace);
		XUtil::DetourJump(moduleBase + 0x031AEF0, InternalEngineLog);
		XUtil::DetourJump(moduleBase + 0x03716B0, InternalEngineLog);
		XUtil::DetourJump(moduleBase + 0x03714A0, InternalEngineLog);
		//*(bool *)(moduleBase + 0x71158F0) = true; audio logging

		XUtil::DetourCall(moduleBase + 0x01405F2, PackFileDevice_MountArchive);
		XUtil::DetourCall(moduleBase + 0x014065E, PackFileDevice_MountArchive);

		// Function to set 3rd person camera position
		XUtil::DetourCall(moduleBase + 0x13AB8FC, hk_call_1413AB8FC);

		// Kill one of the out of bounds checks
		XUtil::PatchMemoryNop(moduleBase + 0xEF5A4D, 2);
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
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
	const static auto addr = g_ModuleBase + 0x0147790;
	((void(__fastcall *)(PackFileDevice *, const String&, uint32_t))(addr))(Device, BinPath, Priority);

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

	CallOffset<0x0BB41A0, void(*)(CameraEntity *, WorldTransform&)>(Entity, Transform);
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

	auto scan = [](const char *Signature)
	{
		return XUtil::FindPattern(g_ModuleBase, g_ModuleSize, Signature);
	};

	if (g_GameType == GameType::DeathStranding)
	{
		strcpy_s(g_GamePrefix, "DS");

		g_OffsetMap["String::String"] = scan("40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
		g_OffsetMap["String::~String"] = scan("40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
		g_OffsetMap["RTTI::GetCoreBinaryTypeId"] = scan("4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");

		// 1.04
		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x4870440;
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		strcpy_s(g_GamePrefix, "HZD");

		g_OffsetMap["String::Assign"] = scan("48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B 39 48");
		g_OffsetMap["String::Concat"] = scan("40 53 48 83 EC 20 80 3A");
		g_OffsetMap["String::CRC32"] = scan("48 8B 11 8B 42 F4");
		g_OffsetMap["RTTI::GetCoreBinaryTypeId"] = scan("48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		g_OffsetMap["ShaderCachePresent"] = scan("48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D");
		g_OffsetMap["Player::GetLocalPlayer"] = g_ModuleBase + 0x0C2F710;
		g_OffsetMap["SwapChainDX12::Present"] = g_ModuleBase + 0x02027A0;
		g_OffsetMap["Entity::AddComponent"] = g_ModuleBase + 0x0B97960;
		g_OffsetMap["Entity::RemoveComponent"] = g_ModuleBase + 0x0BAF7C0;
		g_OffsetMap["SlowMotionManager::AddTimescaleModifier"] = g_ModuleBase + 0x11CB770;
		g_OffsetMap["SlowMotionManager::RemoveTimescaleModifier"] = g_ModuleBase + 0x11CA9F0;

		// 1.0.10.5
		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x2A142F0;
	}
}

void ApplyHooks()
{
	if (g_GameType == GameType::DeathStranding)
	{
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		XUtil::GetPESectionRange(g_ModuleBase, ".text", &g_CodeBase, &g_CodeEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".rdata", &g_RdataBase, &g_RdataEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".data", &g_DataBase, &g_DataEnd);

		MSRTTI::Initialize();
		RTTIScanner::ScanForRTTIStructures();

		// Intercept fullgame load
		XUtil::PatchMemory(g_ModuleBase + 0x0237557, { 0x90, 0x90 });// Force enable debug command list names (render order)
		XUtil::PatchMemory(g_ModuleBase + 0x01D41BC, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force enable debug command list names (command list)

		XUtil::PatchMemory(g_ModuleBase + 0x0379DF0, { 0xC3 });// Steam being retarded and they're not checking a nullptr when offline
		XUtil::PatchMemory(g_ModuleBase + 0x037AD22, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force disable exception handler

		XUtil::DetourCall(g_ModuleBase + 0x023BBD0, &hk_SwapChainDX12_Present);

		XUtil::PatchMemoryNop(g_ModuleBase + 0x04A7F82, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(g_ModuleBase + 0x04A7F82, &PostLoadObjectHook);

		XUtil::PatchMemoryNop(g_ModuleBase + 0x04A7F55, 7);// Rewriting instructions since the 5 byte call doesn't fit
		XUtil::DetourCall(g_ModuleBase + 0x04A7F55, &PreLoadObjectHook);

		XUtil::DetourJump(g_ModuleBase + 0x0603D00, NodeGraphAlert);
		XUtil::DetourJump(g_ModuleBase + 0x0603D10, NodeGraphAlertWithName);
		XUtil::DetourJump(g_ModuleBase + 0x0603D30, NodeGraphTrace);
		XUtil::DetourJump(g_ModuleBase + 0x031AEF0, InternalEngineLog);
		XUtil::DetourJump(g_ModuleBase + 0x03716B0, InternalEngineLog);
		XUtil::DetourJump(g_ModuleBase + 0x03714A0, InternalEngineLog);
		//*(bool *)(g_ModuleBase + 0x71158F0) = true; audio logging

		XUtil::DetourCall(g_ModuleBase + 0x01405F2, PackFileDevice_MountArchive);
		XUtil::DetourCall(g_ModuleBase + 0x014065E, PackFileDevice_MountArchive);

		// Function to set 3rd person camera position
		XUtil::DetourCall(g_ModuleBase + 0x13AB8FC, hk_call_1413AB8FC);

		// Kill one of the out of bounds checks
		XUtil::PatchMemoryNop(g_ModuleBase + 0xEF5A4D, 2);
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		auto moduleBase = reinterpret_cast<uintptr_t>(GetModuleHandle(nullptr));
		auto ntHeaders = reinterpret_cast<PIMAGE_NT_HEADERS64>(moduleBase + reinterpret_cast<PIMAGE_DOS_HEADER>(moduleBase)->e_lfanew);

		// Determine the module/code section addresses and sizes
		g_ModuleBase = moduleBase;
		g_ModuleSize = ntHeaders->OptionalHeader.SizeOfImage;

		LoadSignatures();
		ApplyHooks();
	}

	return TRUE;
}
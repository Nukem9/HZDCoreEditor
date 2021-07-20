#include <atomic>
#include <stdlib.h>

#include "common.h"
#include "MSRTTI.h"
#include "RTTICSharpExporter.h"
#include "RTTIIDAExporter.h"

using namespace HRZ;

std::unordered_set<const GGRTTI *> AllRegisteredTypeInfo;

using CoreLibraryInitializerPfn = void(*)(void *, void *, void *);

void hk_RunCoreLibraryInitializer(const String& ImportFunctionName, CoreLibraryInitializerPfn Callback)
{
	const static auto addr = g_OffsetMap["RunCoreLibraryInitializer"];
	((void(__fastcall *)(const String&, CoreLibraryInitializerPfn))(addr))(ImportFunctionName, Callback);
}

void RegisterTypeInfoRecursively(const GGRTTI *Info)
{
	if (AllRegisteredTypeInfo.count(Info))
		return;

	AllRegisteredTypeInfo.emplace(Info);

	if (auto asClass = Info->AsClass(); asClass)
	{
		// Register base classes
		for (auto& base : asClass->ClassInheritance())
			RegisterTypeInfoRecursively(base.m_Type);

		for (auto& member : asClass->ClassMembers())
		{
			const GGRTTI *memberType = member.m_Type;

			// Drill down to the basic type for containers and references
			while (memberType)
			{
				RegisterTypeInfoRecursively(memberType);

				auto baseType = memberType->GetContainedType();

				if (memberType == baseType)
					break;

				memberType = baseType;
			}
		}
	}
}

void(__fastcall * RTTIFactory__RegisterTypeInfo)(__int64 a1, GGRTTI *TypeInfo);
void __fastcall hk_RTTIFactory__RegisterTypeInfo(__int64 a1, GGRTTI *TypeInfo)
{
	static std::atomic_uint32_t listIndex;
	listIndex++;

	RegisterTypeInfoRecursively(TypeInfo);

	// If the final list is registered, dump everything to disk
	if (g_GameType == GameType::DeathStranding && listIndex == 12290)
	{
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EVertexElementStorageType"]));

		RTTIIDAExporter::ExportAll("C:\\ggrtti_export");
		RTTICSharpExporter::ExportAll("C:\\ggrtti_export");
	}
	else if (g_GameType == GameType::HorizonZeroDawn && listIndex == 30644)
	{
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EDataBufferFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EIndexFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_ERenderBufferFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_ETextureType"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EVertexElementStorageType"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EDX12HeapType"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_ED3D12CommandListType"]));

		RTTIIDAExporter::ExportAll("C:\\ggrtti_export");
		RTTICSharpExporter::ExportAll("C:\\ggrtti_export");
	}

	RTTIFactory__RegisterTypeInfo(a1, TypeInfo);
}

#include "HRZ/PGraphics3D/SwapChainDX12.h"
#include "HRZ/DebugUI/DebugUI.h"

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
		g_OffsetMap["GGRTTI::GetCoreBinaryTypeId"] = scan("4C 8B DC 55 53 56 41 56 49 8D 6B A1 48 81 EC C8 00 00 00");
		g_OffsetMap["RTTIFactory::RegisterTypeInfo"] = scan("40 55 53 56 57 41 56 41 57 48 8D 6C 24 D1 48 81 EC F8 00 00 00 48 8B ? ? ? ? ? 48 33 C4 48 89 45 1F 66 83 3A FE");

		// 1.04
		g_OffsetMap["GGRTTI_EVertexElementStorageType"] = g_ModuleBase + 0x3ED1728;

		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x4870440;
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		strcpy_s(g_GamePrefix, "HZD");

		g_OffsetMap["String::Assign"] = scan("48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B 39 48");
		g_OffsetMap["String::Concat"] = scan("40 53 48 83 EC 20 80 3A");
		g_OffsetMap["String::CRC32"] = scan("48 8B 11 8B 42 F4");
		g_OffsetMap["GGRTTI::GetCoreBinaryTypeId"] = scan("48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		g_OffsetMap["RTTIFactory::RegisterTypeInfo"] = scan("48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 E8 ? ? ? ? 84 C0 75 77 48 8D");
		g_OffsetMap["hk_RunCoreLibraryInitializer"] = scan("E8 ? ? ? ? 48 8D 4C 24 58 B3 01 E8 ? ? ? ? 48 8D 4C 24 50 E8");
		g_OffsetMap["RunCoreLibraryInitializer"] = scan("48 8B C2 4C 8D 05 ? ? ? ? 48 8D 15 ? ? ? ? 48 8D 0D ? ? ? ? 48 FF E0");
		g_OffsetMap["ShaderCachePresent"] = scan("48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D");
		g_OffsetMap["Player::GetLocalPlayer"] = g_ModuleBase + 0x0C2F710;
		g_OffsetMap["SwapChainDX12::Present"] = g_ModuleBase + 0x02027A0;
		g_OffsetMap["Entity::AddComponent"] = g_ModuleBase + 0x0B97960;
		g_OffsetMap["Entity::RemoveComponent"] = g_ModuleBase + 0x0BAF7C0;
		g_OffsetMap["SlowMotionManager::AddTimescaleModifier"] = g_ModuleBase + 0x11CB770;
		g_OffsetMap["SlowMotionManager::RemoveTimescaleModifier"] = g_ModuleBase + 0x11CA9F0;

		// 1.0.10.5
		g_OffsetMap["GGRTTI_EDataBufferFormat"] = g_ModuleBase + 0x2314278;
		g_OffsetMap["GGRTTI_EIndexFormat"] = g_ModuleBase + 0x2313E18;
		g_OffsetMap["GGRTTI_ERenderBufferFormat"] = g_ModuleBase + 0x23142B8;
		g_OffsetMap["GGRTTI_ETextureType"] = g_ModuleBase + 0x23139E8;
		g_OffsetMap["GGRTTI_EVertexElementStorageType"] = g_ModuleBase + 0x2314178;
		g_OffsetMap["GGRTTI_EDX12HeapType"] = g_ModuleBase + 0x2325708;
		g_OffsetMap["GGRTTI_ED3D12CommandListType"] = g_ModuleBase + 0x23242A8;

		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x2A142F0;
	}
}

void ApplyHooks()
{
	if (g_GameType == GameType::DeathStranding)
	{
		// Intercept all type info registrations
		*(uintptr_t *)&RTTIFactory__RegisterTypeInfo = Detours::X64::DetourFunctionClass(g_OffsetMap["RTTIFactory::RegisterTypeInfo"], &hk_RTTIFactory__RegisterTypeInfo);
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		XUtil::GetPESectionRange(g_ModuleBase, ".text", &g_CodeBase, &g_CodeEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".rdata", &g_RdataBase, &g_RdataEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".data", &g_DataBase, &g_DataEnd);

		MSRTTI::Initialize();

		// Intercept all type info registrations
		*(uintptr_t *)&RTTIFactory__RegisterTypeInfo = Detours::X64::DetourFunctionClass(g_OffsetMap["RTTIFactory::RegisterTypeInfo"], &hk_RTTIFactory__RegisterTypeInfo);

		// Intercept fullgame load
		XUtil::DetourCall(g_OffsetMap["hk_RunCoreLibraryInitializer"], &hk_RunCoreLibraryInitializer);

		XUtil::PatchMemory(g_ModuleBase + 0x0237557, { 0x90, 0x90 });// Force enable debug command list names (render order)
		XUtil::PatchMemory(g_ModuleBase + 0x01D41BC, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force enable debug command list names (command list)

		XUtil::PatchMemory(g_ModuleBase + 0x0379DF0, { 0xC3 });// Steam being retarded and they're not checking a nullptr when offline
		XUtil::PatchMemory(g_ModuleBase + 0x037AD22, { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });// Force disable exception handler

		XUtil::DetourCall(g_ModuleBase + 0x023BBD0, &hk_SwapChainDX12_Present);
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		if (AllocConsole())
		{
			freopen("CONOUT$", "w", stdout);
			freopen("CONOUT$", "w", stderr);
		}

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
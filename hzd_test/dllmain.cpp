#include <atomic>
#include <stdlib.h>
#include "common.h"
#include "MSRTTI.h"
#include "PCore.h"
#include "RTTI.h"
#include "RTTICSharpExporter.h"
#include "RTTIIDAExporter.h"

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

void __fastcall hk_sub_1402EE8D0(__int64 a1, GGRTTI **TypeInfoList)
{
	static std::atomic_uint32_t listIndex;
	listIndex++;

	for (auto i = TypeInfoList; *i != nullptr; i++)
	{
		RegisterTypeInfoRecursively(*i);

		(*(__int64(__fastcall **)(__int64, GGRTTI *))(*(__int64 *)a1 + 8))(a1, *i);
	}

	// If the final list is registered, dump everything to disk
	if (g_GameType == GameType::DeathStranding && listIndex == 117)
	{
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EVertexElementStorageType"]));

		RTTIIDAExporter::ExportAll("C:\\ggrtti_export");
		RTTICSharpExporter::ExportAll("C:\\ggrtti_export");
	}
	else if (g_GameType == GameType::HorizonZeroDawn && listIndex == 59)
	{
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EDataBufferFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EIndexFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_ERenderBufferFormat"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_ETextureType"]));
		RegisterTypeInfoRecursively(reinterpret_cast<GGRTTI *>(g_OffsetMap["GGRTTI_EVertexElementStorageType"]));

		RTTIIDAExporter::ExportAll("C:\\ggrtti_export");
		RTTICSharpExporter::ExportAll("C:\\ggrtti_export");
	}
}

void LoadSignatures()
{
	wchar_t modulePath[MAX_PATH];
	GetModuleFileNameW(GetModuleHandle(nullptr), modulePath, MAX_PATH);

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
		g_OffsetMap["sub_1402EE8D0"] = scan("48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 48 8B 12 48 85 D2 74 1D 0F 1F 84 00 00 00 00 00 48");

		// 1.04
		g_OffsetMap["GGRTTI_EVertexElementStorageType"] = g_ModuleBase + 0x3ED1728;

		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x4870440;
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		strcpy_s(g_GamePrefix, "HZD");

		g_OffsetMap["String::String"] = scan("48 89 5C 24 10 48 89 6C 24 18 48 89 7C 24 20 41 56 48 83 EC 20 33 FF 48 8B EA 48 89 39 4C 8B F1 48 C7 C3 FF FF FF FF 48 FF C3");
		g_OffsetMap["String::~String"] = scan("40 53 48 83 EC 20 48 8B 19 48 85 DB 74 37 48 83 C3 F0");
		g_OffsetMap["GGRTTI::GetCoreBinaryTypeId"] = scan("48 8B C4 44 89 40 18 48 89 50 10 48 89 48 08 55 53 56 41 56 48 8D 68 A1 48 81 EC 98 00 00 00 4C 89 60 D0");
		g_OffsetMap["sub_1402EE8D0"] = scan("48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 48 8B 12 48 85 D2 74 1E 0F 1F 84 00 00 00 00 00");
		g_OffsetMap["hk_RunCoreLibraryInitializer"] = scan("E8 ? ? ? ? 48 8D 4C 24 58 B3 01 E8 ? ? ? ? 48 8D 4C 24 50 E8");
		g_OffsetMap["RunCoreLibraryInitializer"] = scan("48 8B C2 4C 8D 05 ? ? ? ? 48 8D 15 ? ? ? ? 48 8D 0D ? ? ? ? 48 FF E0");
		g_OffsetMap["ShaderCachePresent"] = scan("48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D");

		// 1.05
		g_OffsetMap["GGRTTI_EDataBufferFormat"] = g_ModuleBase + 0x2317898;
		g_OffsetMap["GGRTTI_EIndexFormat"] = g_ModuleBase + 0x2317528;
		g_OffsetMap["GGRTTI_ERenderBufferFormat"] = g_ModuleBase + 0x23179A8;
		g_OffsetMap["GGRTTI_ETextureType"] = g_ModuleBase + 0x2316FA0;
		g_OffsetMap["GGRTTI_EVertexElementStorageType"] = g_ModuleBase + 0x2317678;

		g_OffsetMap["ExportedSymbolGroupArray"] = g_ModuleBase + 0x2A16CF0;
	}
}

void ApplyHooks()
{
	if (g_GameType == GameType::DeathStranding)
	{
		// Intercept all type info registrations
		XUtil::DetourJump(g_OffsetMap["sub_1402EE8D0"], &hk_sub_1402EE8D0);
	}
	else if (g_GameType == GameType::HorizonZeroDawn)
	{
		XUtil::GetPESectionRange(g_ModuleBase, ".text", &g_CodeBase, &g_CodeEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".rdata", &g_RdataBase, &g_RdataEnd);
		XUtil::GetPESectionRange(g_ModuleBase, ".data", &g_DataBase, &g_DataEnd);

		MSRTTI::Initialize();

		// Intercept all type info registrations
		XUtil::DetourJump(g_OffsetMap["sub_1402EE8D0"], &hk_sub_1402EE8D0);

		// Intercept fullgame load
		XUtil::DetourCall(g_OffsetMap["hk_RunCoreLibraryInitializer"], &hk_RunCoreLibraryInitializer);

		// Disable shader compilation on startup (return true)
		XUtil::PatchMemory(g_OffsetMap["ShaderCachePresent"], { 0xB0, 0x01, 0xC3 });
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
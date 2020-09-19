#include <atomic>
#include "common.h"
#include "MSRTTI.h"
#include "PCore.h"
#include "RTTI.h"
#include "RTTICSharpExporter.h"
#include "RTTIIDAExporter.h"

std::unordered_set<const RTTI *> AllRegisteredTypeInfo;

using CoreLibraryInitializerPfn = void(*)(void *, void *, void *);

void hk_RunCoreLibraryInitializer(const String& ImportFunctionName, CoreLibraryInitializerPfn Callback)
{
#if HORIZON_ZERO_DAWN
	const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 8B C2 4C 8D 05 ? ? ? ? 48 8D 15 ? ? ? ? 48 8D 0D ? ? ? ? 48 FF E0");
	((void(__fastcall *)(const String&, CoreLibraryInitializerPfn))(addr))(ImportFunctionName, Callback);
#endif
}

void RegisterTypeInfoRecursively(const RTTI *Info)
{
	if (AllRegisteredTypeInfo.count(Info))
		return;

	AllRegisteredTypeInfo.emplace(Info);

	if (Info->m_InfoType == RTTI::INFO_TYPE_CLASS)
	{
		// Register base classes
		for (auto& base : Info->ClassInheritance())
			RegisterTypeInfoRecursively(base.m_Type);

		for (auto& member : Info->ClassMembers())
		{
			const RTTI *memberType = member.m_Type;

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

void __fastcall hk_sub_1402EE8D0(__int64 a1, RTTI **TypeInfoList)
{
	static std::atomic<uint32_t> listIndex;

	for (auto i = TypeInfoList; *i != nullptr; i++)
	{
		RegisterTypeInfoRecursively(*i);

		(*(__int64(__fastcall **)(__int64, RTTI *))(*(__int64 *)a1 + 8))(a1, *i);
	}

	listIndex++;

#if HORIZON_ZERO_DAWN
	if (listIndex == 59)
	{
		RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x23158E0));// EDataBufferFormat
		RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2315528));// EIndexFormat
		RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x23159F0));// ERenderBufferFormat
		RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2314FF0));// ETextureType
		RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2315678));// EVertexElementStorageType

		RTTIIDAExporter::ExportAll("C:\\export_hzd");
		RTTICSharpExporter::ExportAll("C:\\export_hzd");
	}
#elif DEATH_STRANDING
	if (listIndex == 117)
	{
		RTTIIDAExporter::ExportAll("C:\\export_ds");
		RTTICSharpExporter::ExportAll("C:\\export_ds");
	}
#endif
}

void ApplyHooks()
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

#if HORIZON_ZERO_DAWN
	XUtil::GetPESectionRange(moduleBase, ".text", &g_CodeBase, &g_CodeEnd);
	XUtil::GetPESectionRange(moduleBase, ".rdata", &g_RdataBase, &g_RdataEnd);
	XUtil::GetPESectionRange(moduleBase, ".data", &g_DataBase, &g_DataEnd);

	MSRTTI::Initialize();

	// Disable shader compilation on startup (return true)
	XUtil::PatchMemory(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D"), { 0xB0, 0x01, 0xC3 });

	// Intercept all type info registrations
	XUtil::DetourJump(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 48 8B 12 48 85 D2 74 1E 0F 1F 84 00 00 00 00 00"), &hk_sub_1402EE8D0);

	// Intercept fullgame load
	XUtil::DetourCall(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "E8 ? ? ? ? 48 8D 4C 24 58 B3 01 E8 ? ? ? ? 48 8D 4C 24 50 E8"), &hk_RunCoreLibraryInitializer);
#elif DEATH_STRANDING
	// Intercept all type info registrations
	XUtil::DetourJump(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 48 8B 12 48 85 D2 74 1D 0F 1F 84 00 00 00 00 00 48"), &hk_sub_1402EE8D0);
#endif
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
		ApplyHooks();

	return TRUE;
}
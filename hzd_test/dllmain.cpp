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
	// All RTTI will be initialized by the time fullgame is loaded
	RTTIIDAExporter::ExportAll("C:\\testcscode");
	RTTICSharpExporter::ExportAll("C:\\testcscode");

	//TerminateProcess(GetCurrentProcess(), 0);

	const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 8B C2 4C 8D 05 ? ? ? ? 48 8D 15 ? ? ? ? 48 8D 0D ? ? ? ? 48 FF E0");
	((void(__fastcall *)(const String&, CoreLibraryInitializerPfn))(addr))(ImportFunctionName, Callback);
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
	RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2328898));// EDataBufferFormat
	RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2328528));// EIndexFormat
	RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x23289A8));// ERenderBufferFormat
	RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2327FF0));// ETextureType
	RegisterTypeInfoRecursively((RTTI *)(g_ModuleBase + 0x2328678));// EVertexElementStorageType

	for (auto i = TypeInfoList; *i != nullptr; i++)
	{
		RegisterTypeInfoRecursively(*i);

		(*(__int64(__fastcall **)(__int64, RTTI *))(*(__int64 *)a1 + 8))(a1, *i);
	}
}

bool hk_sub_14021BBD0()
{
	return true;
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

	XUtil::GetPESectionRange(moduleBase, ".text", &g_CodeBase, &g_CodeEnd);
	XUtil::GetPESectionRange(moduleBase, ".rdata", &g_RdataBase, &g_RdataEnd);
	XUtil::GetPESectionRange(moduleBase, ".data", &g_DataBase, &g_DataEnd);

	MSRTTI::Initialize();

	// Disable shader compilation on startup
	XUtil::DetourJump(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 18 48 89 74 24 20 57 48 81 EC 90 00 00 00 48 8D"), &hk_sub_14021BBD0);

	// Intercept all type info registrations
	XUtil::DetourJump(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 08 57 48 83 EC 20 48 8B DA 48 8B F9 48 8B 12 48 85 D2 74 1E 0F 1F 84 00 00 00 00 00"), &hk_sub_1402EE8D0);

	// Intercept fullgame load
	XUtil::DetourCall(XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "E8 ? ? ? ? 48 8D 4C 24 58 B3 01 E8 ? ? ? ? 48 8D 4C 24 50 E8"), &hk_RunCoreLibraryInitializer);
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
		ApplyHooks();

	return TRUE;
}
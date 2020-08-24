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

	((void(__fastcall *)(const String&, CoreLibraryInitializerPfn))(g_ModuleBase + 0xF8E00))(ImportFunctionName, Callback);
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
	for (auto i = TypeInfoList; *i != nullptr; i++)
	{
		RegisterTypeInfoRecursively(*i);

		(*(__int64(__fastcall **)(__int64, RTTI *))(*(__int64 *)a1 + 8))(a1, *i);
	}
}

thread_local bool IsSpecialClass;
FILE *f = fopen("C:\\datalog.txt", "w");

__int64 (__fastcall * RTTIBinaryReader__BinaryReaderStream__ReadClass)(__int64 a1, const RTTI *Type, __int64 a3);
__int64 __fastcall hk_RTTIBinaryReader__BinaryReaderStream__ReadClass(__int64 a1, const RTTI *Type, __int64 a3)
{
	__int64& readOffset = *(__int64 *)(a1 + 0x20);
	__int64 oldOffset = readOffset;

	if (Type == (RTTI *)(g_ModuleBase + 0x26C6CA0))
		IsSpecialClass = true;

	if (IsSpecialClass)
	{
		fprintf(f, "Begin reading class type %s -- %llX\n", Type->GetSymbolName().c_str(), readOffset);

		if (Type == (RTTI *)(g_ModuleBase + 0x267ACA0))
			__debugbreak();
	}

	auto result = RTTIBinaryReader__BinaryReaderStream__ReadClass(a1, Type, a3);

	if (IsSpecialClass)
	{
		fprintf(f, "End reading class type %s -- %llX (%llX)\n", Type->GetSymbolName().c_str(), readOffset, readOffset - oldOffset);
	}

	if (IsSpecialClass && Type == (RTTI *)(g_ModuleBase + 0x26C6CA0))
		IsSpecialClass = false;

	return result;
}

__int64(__fastcall * RTTIBinaryReader__BinaryReaderStream__ReadPrimitive)(__int64 a1, const RTTI *Type, __int64 a3);
__int64 __fastcall hk_RTTIBinaryReader__BinaryReaderStream__ReadPrimitive(__int64 a1, const RTTI *Type, __int64 a3)
{
	__int64& readOffset = *(__int64 *)(a1 + 0x20);
	__int64 oldOffset = readOffset;

	if (IsSpecialClass)
	{
		fprintf(f, "Read primitive %s -- %llX\n", Type->GetSymbolName().c_str(), readOffset);
	}

	auto result = RTTIBinaryReader__BinaryReaderStream__ReadPrimitive(a1, Type, a3);

	if (IsSpecialClass)
	{
		fprintf(f, "End read primitive %s -- %llX (%llX)\n", Type->GetSymbolName().c_str(), readOffset, readOffset - oldOffset);
	}

	return result;
}

__int64(__fastcall * RTTIBinaryReader__BinaryReaderStream__ReadContainer)(__int64 a1, const RTTI *Type, __int64 a3);
__int64 __fastcall hk_RTTIBinaryReader__BinaryReaderStream__ReadContainer(__int64 a1, const RTTI *Type, __int64 a3)
{
	__int64& readOffset = *(__int64 *)(a1 + 0x20);
	__int64 oldOffset = readOffset;

	if (IsSpecialClass)
	{
		fprintf(f, "Read container %s -- %llX\n", Type->GetSymbolName().c_str(), readOffset);
	}

	auto result = RTTIBinaryReader__BinaryReaderStream__ReadContainer(a1, Type, a3);

	if (IsSpecialClass)
	{
		fprintf(f, "End read container %s -- %llX (%llX)\n", Type->GetSymbolName().c_str(), readOffset, readOffset - oldOffset);
	}

	return result;
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

	XUtil::DetourCall(g_ModuleBase + 0x87E3D3, &hk_RunCoreLibraryInitializer);
	XUtil::DetourJump(g_ModuleBase + 0x2EF010, &hk_sub_1402EE8D0);

	//*(uintptr_t *)&RTTIBinaryReader__BinaryReaderStream__ReadClass = Detours::X64::DetourFunction(g_ModuleBase + 0x48C7C0, (uintptr_t)&hk_RTTIBinaryReader__BinaryReaderStream__ReadClass);
	//*(uintptr_t *)&RTTIBinaryReader__BinaryReaderStream__ReadPrimitive = Detours::X64::DetourFunction(g_ModuleBase + 0x48BBC0, (uintptr_t)&hk_RTTIBinaryReader__BinaryReaderStream__ReadPrimitive);
	//*(uintptr_t *)&RTTIBinaryReader__BinaryReaderStream__ReadContainer = Detours::X64::DetourFunction(g_ModuleBase + 0x48C390, (uintptr_t)&hk_RTTIBinaryReader__BinaryReaderStream__ReadContainer);
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
		ApplyHooks();

	return TRUE;
}
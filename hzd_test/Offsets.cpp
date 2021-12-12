#include "Offsets.h"
#include "XUtil.h"

namespace Offsets
{

std::unordered_map<uint64_t, uintptr_t> OffsetMapping;

std::pair<uintptr_t, uintptr_t> GetModule()
{
	static uintptr_t moduleBase = reinterpret_cast<uintptr_t>(GetModuleHandleW(nullptr));
	static uintptr_t moduleEnd = [&]()
	{
		auto ntHeaders = reinterpret_cast<PIMAGE_NT_HEADERS64>(moduleBase + reinterpret_cast<PIMAGE_DOS_HEADER>(moduleBase)->e_lfanew);
		return moduleBase + ntHeaders->OptionalHeader.SizeOfImage;
	}();

	return { moduleBase, moduleEnd };
}

std::pair<uintptr_t, uintptr_t> GetModuleSection(const std::string_view Section)
{
	uintptr_t base = 0;
	uintptr_t end = 0;

	if (!XUtil::GetPESectionRange(GetModule().first, Section.data(), &base, &end))
		__debugbreak();

	return { base, end };
}

std::pair<uintptr_t, uintptr_t> GetCodeSection()
{
	static auto section = GetModuleSection(".text");
	return section;
}

std::pair<uintptr_t, uintptr_t> GetRdataSection()
{
	static auto section = GetModuleSection(".rdata");
	return section;
}

std::pair<uintptr_t, uintptr_t> GetDataSection()
{
	static auto section = GetModuleSection(".data");
	return section;
}

void MapAddress(const std::string_view ID, uintptr_t Offset)
{
	auto hash = LiteralHash::FNV1A(ID);
	auto found = OffsetMapping.find(hash);

	if (found != OffsetMapping.end())
		__debugbreak();

	OffsetMapping.emplace(hash, Offset);
}

void MapSignature(const std::string_view ID, const std::string_view Signature)
{
	auto [moduleBase, moduleEnd] = GetModule();
	uintptr_t address = XUtil::FindPattern(moduleBase, moduleEnd - moduleBase, Signature.data());

	if (address == 0)
		__debugbreak();

	MapAddress(ID, address - moduleBase);
}

uintptr_t FindOffset(const std::string_view ID)
{
	return OffsetMapping.at(LiteralHash::FNV1A(ID));
}

uintptr_t FindOffset(uint64_t IDHash)
{
	return OffsetMapping.at(IDHash);
}

}
#pragma once

#include <cstdint>
#include <atomic>
#include <utility>
#include <string_view>

namespace Offsets
{

std::pair<uintptr_t, uintptr_t> GetModule();
std::pair<uintptr_t, uintptr_t> GetModuleSection(const std::string_view Section);
std::pair<uintptr_t, uintptr_t> GetCodeSection();
std::pair<uintptr_t, uintptr_t> GetRdataSection();
std::pair<uintptr_t, uintptr_t> GetDataSection();

void MapAddress(const std::string_view ID, uintptr_t Offset);
void MapSignature(const std::string_view ID, const std::string_view Signature, int Adjustment = 0);
uintptr_t FindOffset(const std::string_view ID);
uintptr_t FindOffset(uint64_t IDHash);

struct LiteralHash
{
	uint64_t Value;

	LiteralHash() = delete;

	template<size_t Size>
	consteval LiteralHash(const char(&Value)[Size]) : Value(FNV1A(Value, Size - 1))
	{
	}

	static constexpr uint64_t FNV1A(const std::string_view Buffer)
	{
		return FNV1A(Buffer.data(), Buffer.length());
	}

	static constexpr uint64_t FNV1A(const char *Buffer, size_t Length)
	{
		uint64_t hash = 0x100000001B3ull;

		for (size_t i = 0; i < Length; i++)
		{
			hash ^= Buffer[i];
			hash *= 0xCBF29CE484222325ull;
		}

		return hash;
	}
};

template<typename T>
auto Resolve(uintptr_t Offset)
{
	return (T)(GetModule().first + Offset);
}

template<LiteralHash Hash, typename T = uintptr_t>
auto ResolveID()
{
	return (T)(GetModule().first + FindOffset(Hash.Value));
}

template<uintptr_t Offset, typename T, typename... TArgs>
__declspec(noinline) auto Call(TArgs&&... Args)
{
	static std::atomic<uintptr_t> address;

	if (address == 0)
		address.store(GetModule().first + Offset);

	return (reinterpret_cast<T>(address.load()))(std::forward<TArgs>(Args)...);
}

template<LiteralHash Hash, typename T, typename... TArgs>
__declspec(noinline) auto CallID(TArgs&&... Args)
{
	static std::atomic<uintptr_t> address;

	if (address == 0)
		address.store(GetModule().first + FindOffset(Hash.Value));

	return (reinterpret_cast<T>(address.load()))(std::forward<TArgs>(Args)...);
}

}
#pragma once

#include <string>
#include <unordered_map>
#include <type_traits>
#include <atomic>

#include <windows.h>

extern std::unordered_map<std::string, uintptr_t> g_OffsetMap;
extern uintptr_t g_ModuleBase;

namespace HRZ
{

template<size_t A, size_t B, typename = std::enable_if_t<A == B>>
struct is_offset_equal : std::true_type {};

template<size_t A, size_t B>
inline constexpr bool is_offset_equal_v = is_offset_equal<A, B>::value;

#define assert_offset(Structure, Member, Offset) static_assert(HRZ::is_offset_equal_v<offsetof(Structure, Member), (Offset)>, "Structure member offset is unexpected");
#define assert_size(Structure, Size) static_assert(HRZ::is_offset_equal_v<sizeof(Structure), (Size)>, "Structure size is unexpected");

template<size_t Size>
struct LiteralHasher
{
	LiteralHasher() = delete;

	consteval LiteralHasher(const char (&Value)[Size])
	{
		Hash = 0x100000001b3ull;

		for (size_t i = 0; i < Size; i++)
		{
			Hash ^= Value[i];
			Hash *= 0xcbf29ce484222325ull;
		}

		std::copy_n(Value, Size, Str);
	}

	uint64_t Hash;
	char Str[Size];
};

template<typename T>
auto ResolveOffset(uintptr_t Address)
{
	return reinterpret_cast<T>((uintptr_t)GetModuleHandleW(nullptr) + Address);
}

template<typename T, typename... TArgs>
__forceinline auto Call(uintptr_t Address, TArgs&&... Args)
{
	return (reinterpret_cast<T>(Address))(std::forward<TArgs>(Args)...);
}

template<typename T, typename... TArgs>
__declspec(noinline) auto Call(const char *Symbol, TArgs&&... Args)
{
	static std::atomic<uintptr_t> Address = 0;

	if (Address == 0)
		Address.store(g_OffsetMap[Symbol]);

	return (reinterpret_cast<T>(Address.load()))(std::forward<TArgs>(Args)...);
}

template<LiteralHasher LiteralHash, typename T, typename... TArgs>
__declspec(noinline) auto CallID(TArgs&&... Args)
{
	static std::atomic<uintptr_t> Address = 0;

	if (Address == 0)
		Address.store(g_OffsetMap[LiteralHash.Str]);

	return (reinterpret_cast<T>(Address.load()))(std::forward<TArgs>(Args)...);
}

template<uintptr_t Offset, typename T, typename... TArgs>
__declspec(noinline) auto CallOffset(TArgs&&... Args)
{
	static std::atomic<uintptr_t> Address = 0;

	if (Address == 0)
		Address.store((uintptr_t)GetModuleHandleW(nullptr) + Offset);

	return (reinterpret_cast<T>(Address.load()))(std::forward<TArgs>(Args)...);
}

}
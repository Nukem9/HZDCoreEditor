#pragma once

#include "common.h"

#pragma warning(disable:4094) // untagged 'struct' declared no symbols

#define static_assert_offset(Structure, Member, Offset) struct __declspec(empty_bases) : CheckOffset<offsetof(Structure, Member), Offset> { }

template <size_t Offset, size_t RequiredOffset>
struct __declspec(empty_bases) CheckOffset
{
	static_assert(Offset <= RequiredOffset, "Offset is larger than expected");
	static_assert(Offset >= RequiredOffset, "Offset is smaller than expected");
};

namespace XUtil
{
	uintptr_t FindPattern(uintptr_t StartAddress, uintptr_t MaxSize, const char *Mask);
	std::vector<uintptr_t> FindPatterns(uintptr_t StartAddress, uintptr_t MaxSize, const char *Mask);
	bool GetPESectionRange(uintptr_t ModuleBase, const char *Section, uintptr_t *Start, uintptr_t *End);

	void PatchMemory(uintptr_t Address, const uint8_t *Data, size_t Size);
	void PatchMemory(uintptr_t Address, std::initializer_list<uint8_t> Data);
	void PatchMemoryNop(uintptr_t Address, size_t Size);
	void DetourJump(uintptr_t Target, uintptr_t Destination);
	void DetourCall(uintptr_t Target, uintptr_t Destination);

	template<typename T>
	void DetourJump(uintptr_t Target, T Destination)
	{
		static_assert(std::is_member_function_pointer_v<T> || (std::is_pointer_v<T> && std::is_function_v<typename std::remove_pointer<T>::type>));

		DetourJump(Target, *(uintptr_t *)&Destination);
	}

	template<typename T>
	void DetourCall(uintptr_t Target, T Destination)
	{
		static_assert(std::is_member_function_pointer_v<T> || (std::is_pointer_v<T> && std::is_function_v<typename std::remove_pointer<T>::type>));

		DetourCall(Target, *(uintptr_t *)&Destination);
	}
}
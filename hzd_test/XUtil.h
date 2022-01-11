#pragma once

#include <stddef.h>
#include <vector>
#include <type_traits>

namespace XUtil
{

uintptr_t FindPattern(uintptr_t StartAddress, uintptr_t MaxSize, const char *Mask);
std::vector<uintptr_t> FindPatterns(uintptr_t StartAddress, uintptr_t MaxSize, const char *Mask);
bool GetPESectionRange(uintptr_t ModuleBase, const char *Section, uintptr_t *Start, uintptr_t *End);

void PatchMemory(uintptr_t Address, const uint8_t *Data, size_t Size);
void PatchMemory(uintptr_t Address, std::initializer_list<uint8_t> Data);
void PatchMemoryNop(uintptr_t Address, size_t Size);
uintptr_t DetourJump(uintptr_t Target, uintptr_t Destination);
uintptr_t DetourCall(uintptr_t Target, uintptr_t Destination);

template<typename T>
requires(std::is_member_function_pointer_v<T> || (std::is_pointer_v<T> && std::is_function_v<typename std::remove_pointer<T>::type>))
auto DetourJump(uintptr_t Target, T Destination)
{
	return reinterpret_cast<T>(DetourJump(Target, *reinterpret_cast<uintptr_t *>(&Destination)));
}

template<typename T>
requires(std::is_member_function_pointer_v<T> || (std::is_pointer_v<T> && std::is_function_v<typename std::remove_pointer<T>::type>))
auto DetourCall(uintptr_t Target, T Destination)
{
	return reinterpret_cast<T>(DetourCall(Target, *reinterpret_cast<uintptr_t *>(&Destination)));
}

}
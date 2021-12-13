#include "../Offsets.h"

#include "MSRTTI.h"

extern "C"
{
	typedef void* (*malloc_func_t)(size_t);
	typedef void(*free_func_t)(void*);
	char *__unDNameEx(char *outputString, const char *name, int maxStringLength, malloc_func_t pAlloc, free_func_t pFree, char *(__fastcall *pGetParameter)(int), unsigned int disableFlags);
}

namespace MSRTTI
{
	using namespace detail;

	std::vector<Info> Tables;

	void Initialize()
	{
		auto [rdataBase, rdataEnd] = Offsets::GetRdataSection();

		for (uintptr_t i = rdataBase; i < (rdataEnd - sizeof(uintptr_t) - sizeof(uintptr_t)); i++)
		{
			// Skip all non-2-aligned addresses. Not sure if this is OK or it skips tables.
			if (i % 2 != 0)
				continue;

			//
			// This might be a valid RTTI entry, so check if:
			// - The COL points to somewhere in .rdata
			// - The COL has a valid signature
			// - The first virtual function points to .text
			//
			auto addr = *reinterpret_cast<uintptr_t *>(i);
			auto vfuncAddr = *reinterpret_cast<uintptr_t *>(i + sizeof(uintptr_t));

			if (!IsWithinData(addr) || !IsWithinCode(vfuncAddr))
				continue;

			auto locator = reinterpret_cast<CompleteObjectLocator *>(addr);

			if (!IsValidCOL(locator))
				continue;

			Info info
			{
				.VTableAddress = i + sizeof(uintptr_t),
				.VTableOffset = locator->Offset,
				.VFunctionCount = 0,
				.RawName = locator->TypeDescriptor.Get()->name,
				.Locator = locator,
			};

			// Demangle
			info.Name = __unDNameEx(nullptr, info.RawName + 1, 0, malloc, free, nullptr, 0x2800);

			// Determine number of virtual functions
			for (uintptr_t j = info.VTableAddress; j < (rdataEnd - sizeof(uintptr_t)); j += sizeof(uintptr_t))
			{
				if (!IsWithinCode(*reinterpret_cast<uintptr_t *>(j)))
					break;

				info.VFunctionCount++;
			}

			Tables.emplace_back(info);
		}
	}

	void Dump(FILE *File)
	{
		uintptr_t moduleBase = Offsets::GetModule().first;

		for (const Info& info : Tables)
			fprintf(File, "`%s`: VTable [0x%llX, 0x%llX offset, %lld functions] `%s`\n", info.Name, info.VTableAddress - moduleBase, info.VTableOffset, info.VFunctionCount, info.RawName);
	}

	const Info *Find(const char *Name, bool Exact)
	{
		for (const Info& info : Tables)
		{
			if (Exact)
			{
				if (!strcmp(info.Name, Name))
					return &info;
			}
			else
			{
				if (strcasestr(info.Name, Name))
					return &info;
			}
		}

		return nullptr;
	}

	std::vector<const Info *> FindAll(const char *Name, bool Exact)
	{
		// Multiple classes can have identical names but different vtable displacements,
		// so return all that match
		std::vector<const Info *> results;

		for (const Info& info : Tables)
		{
			if (Name)
			{
				if (Exact)
				{
					if (!strcmp(info.Name, Name))
						results.push_back(&info);
				}
				else
				{
					if (strcasestr(info.Name, Name))
						results.push_back(&info);
				}
			}
			else
			{
				results.push_back(&info);
			}
		}

		return results;
	}

	namespace detail
	{
		bool IsWithinData(uintptr_t Address)
		{
			const static auto [rdataBase, rdataEnd] = Offsets::GetRdataSection();
			const static auto [dataBase, dataEnd] = Offsets::GetDataSection();

			return (Address >= rdataBase && Address <= rdataEnd) || (Address >= dataBase && Address <= dataEnd);
		}

		bool IsWithinCode(uintptr_t Address)
		{
			const static auto [codeBase, codeEnd] = Offsets::GetCodeSection();

			return Address >= codeBase && Address <= codeEnd;
		}

		bool IsValidCOL(CompleteObjectLocator *Locator)
		{
			return Locator->Signature == CompleteObjectLocator::COL_Signature64 && IsWithinData(Locator->TypeDescriptor.Address());
		}

		const char *strcasestr(const char *String, const char *Substring)
		{
			for (; *String; String++)
			{
				auto a = String;
				auto b = Substring;

				while (toupper(*a++) == toupper(*b++))
				{
					if (!*b)
						return String;
				}
			}

			return nullptr;
		}
	}
}
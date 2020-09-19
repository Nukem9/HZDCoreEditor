#pragma once

#include "common.h"

class String
{
private:
	const char *m_Data = nullptr;

public:
	String()
	{
	}

	String(const char *Value)
	{
#if HORIZON_ZERO_DAWN
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 5C 24 10 48 89 6C 24 18 48 89 7C 24 20 41 56 48 83 EC 20 33 FF 48 8B EA 48 89 39 4C 8B F1 48 C7 C3 FF FF FF FF 48 FF C3");
#elif DEATH_STRANDING
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "40 53 48 83 EC 20 48 8B D9 48 C7 01 00 00 00 00 49 C7 C0 FF FF FF FF");
#endif

		((void(__fastcall *)(String *, const char *))(addr))(this, Value);
	}

	~String()
	{
#if HORIZON_ZERO_DAWN
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "40 53 48 83 EC 20 48 8B 19 48 85 DB 74 37 48 83 C3 F0");
#elif DEATH_STRANDING
		const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "40 53 48 83 EC 20 48 8B 19 48 8D 05 ? ? ? ? 48 83 EB 10");
#endif

		((void(__fastcall *)(String *))(addr))(this);
	}

	const char *c_str() const
	{
		return m_Data;
	}
};

template<typename T>
class Array
{
private:
	int m_Count;
	int m_Capacity;
	T *m_Entries;

public:
	class iterator
	{
	private:
		T *m_Current;

	public:
		iterator(T *Current) : m_Current(Current)
		{
		}

		iterator operator++()
		{
			m_Current++;
			return *this;
		}

		bool operator!=(const iterator& Other) const
		{
			return m_Current != Other.m_Current;
		}

		const T& operator*() const
		{
			return *m_Current;
		}
	};

	iterator begin() const
	{
		return iterator(&m_Entries[0]);
	}

	iterator end() const
	{
		return iterator(&m_Entries[m_Count]);
	}

	T& operator[](size_t Pos)
	{
		return m_Entries[Pos];
	}

	const T& operator[](size_t Pos) const
	{
		return m_Entries[Pos];
	}

	size_t size() const
	{
		return m_Count;
	}
};
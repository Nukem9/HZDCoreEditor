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
		const static auto addr = g_OffsetMap["String::String"];
		((void(__fastcall *)(String *, const char *))(addr))(this, Value);
	}

	~String()
	{
		const static auto addr = g_OffsetMap["String::~String"];
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
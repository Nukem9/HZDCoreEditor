#pragma once

#include <stdint.h>
#include <vector>
#include <functional>

namespace HRZ
{

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
		explicit iterator(T *Current) : m_Current(Current)
		{
		}

		iterator& operator++()
		{
			m_Current++;
			return *this;
		}

		bool operator==(iterator Other) const
		{
			return m_Current == Other.m_Current;
		}

		bool operator!=(iterator Other) const
		{
			return m_Current != Other.m_Current;
		}

		T& operator *() const
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

	T *data() const
	{
		return m_Entries;
	}

	size_t size() const
	{
		return m_Count;
	}

	T& operator[](size_t Pos)
	{
		return m_Entries[Pos];
	}

	const T& operator[](size_t Pos) const
	{
		return m_Entries[Pos];
	}
};

template<typename T>
void PCore_Quicksort_Impl(T*& Left, T*& Right, const std::function<bool(const T *, const T *)>& Compare, uint32_t& PivotSeed)
{
	if (Left >= Right)
		return;

	PivotSeed = 0x19660D * PivotSeed + 0x3C6EF35F;
	uint32_t pivot = (PivotSeed >> 8) % (Right - Left);

	std::swap(Left[pivot], Right[0]);

	auto start = Left - 1;
	auto end = Right;

	while (start < end)
	{
		// Partition left side
		do
		{
			start++;

			if (!Compare(start, Right))
				break;

		} while (start < end);

		if (end <= start)
			break;

		// Partition right side
		do
		{
			end--;

			if (!Compare(Right, end))
				break;

		} while (end > start);

		if (start >= end)
			break;

		std::swap(*end, *start);
	}

	std::swap(*start, Right[0]);

	auto newRight = start - 1;
	PCore_Quicksort_Impl<T>(Left, newRight, Compare, PivotSeed);

	auto newLeft = start + 1;
	PCore_Quicksort_Impl<T>(newLeft, Right, Compare, PivotSeed);
}

template<typename T>
void PCore_Quicksort(std::vector<T>& Elements, std::function<bool(const T *, const T *)> Compare, uint32_t PivotSeed = 0)
{
	if (Elements.size() <= 1)
		return;

	auto begin = Elements.data();
	auto end = &begin[Elements.size() - 1];

	PCore_Quicksort_Impl<T>(begin, end, Compare, PivotSeed);
}

}
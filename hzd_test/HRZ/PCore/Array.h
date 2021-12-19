#pragma once

#include <stdint.h>
#include <vector>
#include <functional>

namespace HRZ
{

template<typename T>
class Array
{
public:
	template<bool Const, typename PtrType = std::conditional_t<Const, const_pointer, pointer>>
	class internal_iterator;

	using value_type = T;
	using size_type = size_t;
	using difference_type = ptrdiff_t;

	using reference = value_type&;
	using const_reference = const value_type&;

	using pointer = value_type*;
	using const_pointer = const value_type*;

	using iterator = internal_iterator<true>;
	using const_iterator = internal_iterator<false>;

private:
	uint32_t m_Count = 0;
	uint32_t m_Capacity = 0;
	T *m_Entries = nullptr;

public:
	template<bool Const, typename PtrType>
	class internal_iterator
	{
	private:
		PtrType m_Current = nullptr;

	public:
		internal_iterator() = delete;

		explicit internal_iterator(PtrType Current) : m_Current(Current)
		{
		}

		internal_iterator& operator++()
		{
			m_Current++;
			return *this;
		}

		bool operator==(const internal_iterator& Other) const
		{
			return m_Current == Other.m_Current;
		}

		bool operator!=(const internal_iterator& Other) const
		{
			return m_Current != Other.m_Current;
		}

		template<typename = void>
		requires(!Const)
		reference operator*()
		{
			return *m_Current;
		}

		const_reference operator*() const
		{
			return *m_Current;
		}
	};

	Array() = default;

	Array(const Array&) = delete;

	~Array()
	{
		if constexpr (!std::is_trivially_destructible_v<T>)
		{
			for (int i = 0; i < m_Count; i++)
				std::destroy_at(&m_Entries[i]);
		}

		Offsets::Call<0x0376D20, void(*)(void *)>(m_Entries);
	}

	iterator begin()
	{
		return iterator(&m_Entries[0]);
	}

	iterator end()
	{
		return iterator(&m_Entries[m_Count]);
	}

	const_iterator begin() const
	{
		return const_iterator(&m_Entries[0]);
	}

	const_iterator end() const
	{
		return const_iterator(&m_Entries[m_Count]);
	}

	T *data()
	{
		return m_Entries;
	}

	bool empty() const
	{
		return m_Count != 0;
	}

	size_type size() const
	{
		return m_Count;
	}

	Array& operator=(const Array&) = delete;

	reference operator[](size_type Pos)
	{
		return m_Entries[Pos];
	}

	const_reference operator[](size_type Pos) const
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
	const uint32_t pivot = (PivotSeed >> 8) % (Right - Left);

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
#pragma once

#include <cstddef>

namespace HRZ
{

template<typename Key, typename Hash>
class HashContainerBase
{
public:
	template<bool Const, typename PtrType>
	class internal_iterator;

	using key_type = Key;
	using value_type = Key;
	using size_type = size_t;
	using difference_type = ptrdiff_t;
	using hasher = Hash;

	using reference = value_type&;
	using const_reference = const value_type&;

	using pointer = void;
	using const_pointer = void;

private:
	struct KVPContainer
	{
		hasher m_Hash;
		value_type m_Value;
	};

	KVPContainer *m_Entries = nullptr;
	uint32_t m_Count = 0;
	uint32_t m_Capacity = 0;

public:
	using iterator = internal_iterator<false, KVPContainer *>;
	using const_iterator = internal_iterator<true, const KVPContainer *>;

	template<bool Const, typename PtrType>
	class internal_iterator
	{
	private:
		PtrType m_Current = nullptr;
		PtrType m_End = nullptr;

	public:
		internal_iterator() = delete;

		explicit internal_iterator(PtrType Table, int Start, int End)
		{
			if (Table)
			{
				m_Current = &Table[Start];
				m_End = &Table[End];

				SkipEmptyElements();
			}
		}

		internal_iterator& operator++()
		{
			m_Current++;
			SkipEmptyElements();

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
			return m_Current->m_Value;
		}

		const_reference operator*() const
		{
			return m_Current->m_Value;
		}

	private:
		void SkipEmptyElements()
		{
			while (!AtEnd() && m_Current->m_Hash == 0)
				m_Current++;
		}

		bool AtEnd() const
		{
			return m_Current == m_End;
		}
	};

public:

	HashContainerBase() = default;

	HashContainerBase(const HashContainerBase&) = delete;

	~HashContainerBase() = delete;

	iterator begin()
	{
		return iterator(m_Entries, 0, m_Capacity);
	}

	iterator end()
	{
		return iterator(m_Entries, m_Capacity, m_Capacity);
	}

	const_iterator begin() const
	{
		return const_iterator(m_Entries, 0, m_Capacity);
	}

	const_iterator end() const
	{
		return const_iterator(m_Entries, m_Capacity, m_Capacity);
	}

	HashContainerBase& operator=(const HashContainerBase&) = delete;
};

}
#pragma once

#include "Util.h"
#include "HashContainerBase.h"

namespace HRZ
{

template<typename Key>
class HashSet : public HashContainerBase<const Key, uint32_t>
{
public:
	using key_type = Key;
	using value_type = Key;

	using reference = value_type&;
	using const_reference = const value_type&;

public:
	HashSet() = default;
	HashSet(const HashSet&) = delete;
	~HashSet() = delete;
	HashSet& operator=(const HashSet&) = delete;
};

using __TestHashSet = HashSet<void *>;
assert_size(__TestHashSet, 0x10);
assert_size(__TestHashSet::value_type, 0x8);

}
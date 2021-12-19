#pragma once

#include "Util.h"
#include "HashContainerBase.h"

namespace HRZ
{

template<typename Key, typename Value>
struct HashMapKVP : std::pair<Key, Value>
{
};

template<typename Key, typename Value>
class HashMap : public HashContainerBase<HashMapKVP<const Key, Value>, uint32_t>
{
public:
	using key_type = Key;
	using mapped_type = Value;
	using value_type = HashMapKVP<const Key, Value>;

	using reference = value_type&;
	using const_reference = const value_type&;

public:
	HashMap() = default;
	HashMap(const HashMap&) = delete;
	~HashMap() = delete;
	HashMap& operator=(const HashMap&) = delete;
};

using __TestHashMap = HashMap<uint32_t, void *>;
assert_size(__TestHashMap, 0x10);
assert_size(__TestHashMap::value_type, 0x10);

}
#pragma once

#include <stdint.h>

#include "Util.h"

namespace HRZ
{

template<typename Key, typename Value>
struct HashMapKVP
{
	Key m_Key;
	Value m_Value;
};

template<typename Key, typename Value>
class HashMap
{
	using key_type = Key;
	using mapped_type = Value;

public:
	key_type **m_Table;
	uint32_t m_Count;
	uint32_t m_AllocatedCount;
};

using __xTestHashMap = HashMap<uint32_t, void *>;
assert_size(__xTestHashMap, 0x10);

}
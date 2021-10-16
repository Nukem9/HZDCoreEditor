#pragma once

#include "../PCore/Common.h"

#include "PropertyContainerData.h"

namespace HRZ
{

class PropertyContainerResource;

class PropertyContainer
{
public:
	char _pad0[0x8];
	PropertyContainerResource *m_Resource;	// 0x8
	PropertyContainerData m_Data;			// 0x10
	char _pad48[0x10];
	SharedLock m_Lock;						// 0x58
	char _pad60[0x18];
};
assert_offset(PropertyContainer, m_Resource, 0x8);
assert_offset(PropertyContainer, m_Lock, 0x58);
assert_size(PropertyContainer, 0x78);

}
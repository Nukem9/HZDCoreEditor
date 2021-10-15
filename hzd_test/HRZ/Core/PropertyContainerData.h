#pragma once

#include "../PCore/Common.h"

#include "StateObject.h"

namespace HRZ
{

class CoreObject;

DECL_RTTI(PropertyContainerData);

class PropertyContainerData : public StateObject
{
public:
	TYPE_RTTI(PropertyContainerData);

	Array<uint8_t> m_POD;					// 0x8
	Array<String> m_StringTable;			// 0x18
	Array<Ref<CoreObject>> m_CoreObjects;	// 0x28

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~PropertyContainerData() override;		// 1
};
assert_size(PropertyContainerData, 0x38);

}
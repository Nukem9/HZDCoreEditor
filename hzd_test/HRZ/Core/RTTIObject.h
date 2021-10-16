#pragma once

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(RTTIObject);

class RTTIObject
{
public:
	TYPE_RTTI(RTTIObject);

	virtual const RTTI *GetRTTI() const;			// 0
	virtual ~RTTIObject();							// 1
};
assert_size(RTTIObject, 0x8);

}
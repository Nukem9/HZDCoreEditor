#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class RTTI;

class RTTIObject
{
public:
	virtual const RTTI *GetRTTI() const;			// 0
	virtual ~RTTIObject();							// 1
};
assert_size(RTTIObject, 0x8);

}
#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class GGRTTI;

class RTTIObject
{
public:
	virtual const GGRTTI *GetRTTI() const;			// 0
	virtual ~RTTIObject();							// 1
};
assert_size(RTTIObject, 0x8);

}
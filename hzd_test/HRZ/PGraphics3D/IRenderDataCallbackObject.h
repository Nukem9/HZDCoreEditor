#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class IRenderDataCallbackObject
{
public:
	virtual ~IRenderDataCallbackObject();					// 0
	virtual void IRenderDataCallbackObjectUnknown01() = 0;	// 1
	virtual void IRenderDataCallbackObjectUnknown02() = 0;	// 2
};
assert_size(IRenderDataCallbackObject, 0x8);

}
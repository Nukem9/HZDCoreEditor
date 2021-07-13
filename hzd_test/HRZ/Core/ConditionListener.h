#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class ConditionListener
{
public:
	virtual ~ConditionListener();					// 0
	virtual void ConditionListenerUnknown1() = 0;	// 1
	virtual void ConditionListenerUnknown2();		// 2
};
assert_size(ConditionListener, 0x8);

}
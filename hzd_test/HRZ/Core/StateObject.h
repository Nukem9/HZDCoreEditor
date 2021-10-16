#pragma once

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(StateObject);

class StateObject
{
public:
	TYPE_RTTI(StateObject);

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~StateObject();					// 1
	virtual bool StateObjectUnknown02();	// 2
	virtual void StateObjectUnknown03();	// 3
	virtual void StateObjectUnknown04();	// 4
};
assert_size(StateObject, 0x8);

}
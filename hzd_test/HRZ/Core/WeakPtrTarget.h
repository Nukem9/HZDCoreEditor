#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class RTTI;

class WeakPtrTarget
{
public:
	void *m_UnknownList = nullptr;

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~WeakPtrTarget();				// 1
};
assert_size(WeakPtrTarget, 0x10);

}
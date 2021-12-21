#pragma once

#include "../PCore/Util.h"
#include "../PCore/WeakPtr.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(WeakPtrTarget);

class WeakPtrTarget
{
public:
	TYPE_RTTI(WeakPtrTarget);

	WeakPtr<WeakPtrTarget> *m_WeakPtrList = nullptr;	// 0x8

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~WeakPtrTarget();				// 1
};
assert_size(WeakPtrTarget, 0x10);

}
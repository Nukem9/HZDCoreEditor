#pragma once

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(StreamingRefTarget);

class StreamingRefTarget
{
public:
	TYPE_RTTI(StreamingRefTarget);

	String m_Location; // 0x8

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~StreamingRefTarget();			// 1
};
assert_size(StreamingRefTarget, 0x10);

}
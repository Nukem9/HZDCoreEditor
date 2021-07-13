#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class GGRTTI;

class StreamingRefTarget
{
public:
	String m_Location; // 0x8

	virtual const GGRTTI *GetRTTI() const;	// 0
	virtual ~StreamingRefTarget();			// 1
};
assert_size(StreamingRefTarget, 0x10);

}
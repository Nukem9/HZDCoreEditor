#pragma once

#include "../PCore/Util.h"

#include "HwManaged.h"

namespace HRZ
{

class HwResource : public HwManaged
{
public:
	virtual ~HwResource() override;		// 0
	virtual void HwResourceUnknown01();	// 1

	char _pad18[0x8];	// 0x18
};
assert_size(HwResource, 0x20);

}
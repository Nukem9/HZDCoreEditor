#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"

namespace HRZ
{

class BaseResource : public CoreObject
{
public:
	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~BaseResource() override;							// 1
	virtual CoreObject *CreateResourceInstance(void *, bool);	// 16
};
assert_size(BaseResource, 0x20);

}
#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"

namespace HRZ
{

DECL_RTTI(BaseResource);

class BaseResource : public CoreObject
{
public:
	TYPE_RTTI(BaseResource);

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~BaseResource() override;							// 1
	virtual CoreObject *CreateResourceInstance(void *, bool);	// 16
};
assert_size(BaseResource, 0x20);

}
#pragma once

#include "../PCore/Common.h"

#include "BaseResource.h"

namespace HRZ
{

class ResourceWithoutLegacyName : public BaseResource
{
public:
	virtual const GGRTTI *GetRTTI() const override;		// 0
	virtual ~ResourceWithoutLegacyName() override;		// 1
	virtual String& GetName() override;					// 5
	virtual void ResourceWithoutLegacyNameUnknown17();	// 17
};
assert_size(ResourceWithoutLegacyName, 0x20);

}
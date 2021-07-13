#pragma once

#include "../PCore/Util.h"

#include "Entity.h"

namespace HRZ
{

class ViewDependentEntity : public Entity
{
public:
	virtual const GGRTTI *GetRTTI() const override;	// 0
	virtual ~ViewDependentEntity() override;		// 1
};
assert_size(ViewDependentEntity, 0x2C0);

}
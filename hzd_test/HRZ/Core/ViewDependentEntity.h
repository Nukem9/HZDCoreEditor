#pragma once

#include "../PCore/Util.h"

#include "Entity.h"

namespace HRZ
{

DECL_RTTI(ViewDependentEntity);

class ViewDependentEntity : public Entity
{
public:
	TYPE_RTTI(ViewDependentEntity);

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~ViewDependentEntity() override;		// 1
};
assert_size(ViewDependentEntity, 0x2C0);

}
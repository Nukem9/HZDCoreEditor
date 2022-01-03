#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"

namespace HRZ
{

DECL_RTTI(StackableComponent);
DECL_RTTI(StackableComponentResource);

class StackableComponentResource : public EntityComponentResource
{
public:
	TYPE_RTTI(StackableComponentResource);

	char _pad28[0x10];
};
assert_size(StackableComponentResource, 0x38);

class StackableComponent : public EntityComponent
{
public:
	TYPE_RTTI(StackableComponent);

	int m_Amount;	// 0x58

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~StackableComponent() override;						// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(StackableComponent, 0x60);

}
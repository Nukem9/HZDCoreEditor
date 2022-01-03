#pragma once

#include "../PCore/Common.h"

#include "RTTIObject.h"
#include "StateObject.h"
#include "Resource.h"

namespace HRZ
{

DECL_RTTI(StatModification);
DECL_RTTI(StatModificationResource);

class StatModificationResource : public Resource
{
public:
	TYPE_RTTI(StatModificationResource);

	char _pad28[0x10];

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~StatModificationResource() override;				// 1
};
assert_size(StatModificationResource, 0x38);

class StatModification : public RTTIObject, public StateObject
{
public:
	TYPE_RTTI(StatModification);

	char _pad10[0x10];

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~StatModification();			// 1
};
assert_size(StatModification, 0x20);

}
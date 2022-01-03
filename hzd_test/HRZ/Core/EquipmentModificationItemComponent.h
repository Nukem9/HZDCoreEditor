#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "StatModification.h"

namespace HRZ
{

DECL_RTTI(EquipmentModificationItemComponent);
DECL_RTTI(EquipmentModificationItemComponentResource);

class EquipmentModificationItemComponentResource : public EntityComponentResource
{
public:
	TYPE_RTTI(EquipmentModificationItemComponentResource);

	char _pad28[0x48];
};
assert_size(EquipmentModificationItemComponentResource, 0x70);

class EquipmentModificationItemComponent : public EntityComponent
{
public:
	TYPE_RTTI(EquipmentModificationItemComponent);

	Array<StatModification> m_Modifications;	// 0x58

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~EquipmentModificationItemComponent() override;		// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(EquipmentModificationItemComponent, 0x68);

}
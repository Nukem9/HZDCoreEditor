#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "StatModification.h"

namespace HRZ
{

DECL_RTTI(EquipmentModificationComponent);
DECL_RTTI(EquipmentModificationComponentResource);

class EquipmentModificationComponentResource : public EntityComponentResource
{
public:
	TYPE_RTTI(EquipmentModificationComponentResource);

	char _pad28[0x20];
};
assert_size(EquipmentModificationComponentResource, 0x48);

class EquipmentModificationComponent : public EntityComponent
{
public:
	TYPE_RTTI(EquipmentModificationComponent);

	Array<WeakPtr<Entity>> m_ModificationItems;			// 0x58
	Array<StatModification> m_PreSocketedModifications;	// 0x68
	char _pad78[0x8];

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~EquipmentModificationComponent() override;			// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(EquipmentModificationComponent, 0x80);

}
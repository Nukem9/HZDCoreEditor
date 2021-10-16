#pragma once

#include "../PCore/Common.h"

#include "ItemDescriptionComponent.h"

namespace HRZ
{

class EquipSlotType;
class GraphProgramResource;
class SoundResource;

DECL_RTTI(InventoryItemComponent);
DECL_RTTI(InventoryItemComponentResource);

class InventoryItemComponentResource : public ItemDescriptionComponentResource
{
public:
	TYPE_RTTI(InventoryItemComponentResource);

	Array<Ref<EquipSlotType>> m_EquipSlotTypes;						// 0x80
	bool m_IsDroppable;												// 0x90
	bool m_UseSafePlacementForDrop;									// 0x91
	Array<Ref<EntityComponentResource>> m_DroppedComponents;		// 0x98
	Ref<GraphProgramResource> m_DroppedComponentCondition;			// 0xA8
	Array<Ref<EntityComponentResource>> m_PickedUpOnlyComponents;	// 0xB0
	Ref<SoundResource> m_PickUpSound;								// 0xC0
	Ref<EquipSlotType> m_AutoEquipSlotType;							// 0xC8
	bool m_MustBeWieldedOrDropped;									// 0xD0
	bool m_PreventSaveToInventory;									// 0xD1

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~InventoryItemComponentResource() override;			// 1
	virtual const RTTI *GetComponentRTTI() const override;		// 18
	virtual const RTTI *GetComponentRepRTTI() const override;	// 19
	virtual bool EntityComponentResourceUnknown20() override;	// 20
};
assert_size(InventoryItemComponentResource, 0xD8);

class InventoryItemComponent : public ItemDescriptionComponent
{
public:
	TYPE_RTTI(InventoryItemComponent);

	char _pad58[0x8];			// 0x58
	Ref<void *> m_UnknownRef60;	// 0x60
	bool m_IsDroppable;			// 0x68

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~InventoryItemComponent() override;					// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(InventoryItemComponent, 0x70);

}
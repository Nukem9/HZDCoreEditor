#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "InventoryEntity.h"

namespace HRZ
{

class Entity;
class EntityResource;

DECL_RTTI(Inventory);

class Inventory : public EntityComponent
{
public:
	TYPE_RTTI(Inventory);

	Array<Ref<InventoryEntity>> m_Items; // 0x58 TODO: Is this actually a Ref<>?
	char _pad68[0x68];

	virtual const RTTI *GetRTTI() const override;							// 0
	virtual ~Inventory() override;											// 1
	virtual const RTTI *GetComponentRepRTTI() const override;				// 4
	virtual void EntityComponentUnknown07() override;						// 7
	virtual void EntityComponentUnknown08() override;						// 8
	virtual NetEntityComponentState *CreateNetState() override;				// 9
	virtual InventoryEntity *AddItem(EntityResource *, int, bool, bool);	// 10
	virtual void RemoveItem();												// 11
	virtual void InventoryUnknown12();										// 12
	virtual void InventoryUnknown13();										// 13
	virtual void InventoryUnknown14();										// 14
};
assert_offset(Inventory, m_Items, 0x58);
assert_size(Inventory, 0xD0);

}
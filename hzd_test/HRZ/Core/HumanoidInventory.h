#pragma once

#include "../PCore/Common.h"

#include "Inventory.h"
#include "ConditionListener.h"

namespace HRZ
{

extern const RTTI *RTTI_HumanoidInventory;

class HumanoidInventory : public Inventory, public ConditionListener
{
public:
	static inline auto& TypeInfo = RTTI_HumanoidInventory;

	char _padD8[0xD0];

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~HumanoidInventory() override;						// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
	virtual void EntityComponentUnknown07() override;			// 7
	virtual void EntityComponentUnknown08() override;			// 8
	virtual NetEntityComponentState *CreateNetState() override;	// 9
	virtual void RemoveItem() override;							// 11
	virtual void InventoryUnknown12() override;					// 12
	virtual void InventoryUnknown13() override;					// 13
	virtual void InventoryUnknown14() override;					// 14

	virtual void ConditionListenerUnknown1() override;			// 1
};
assert_size(HumanoidInventory, 0x1A8);

}
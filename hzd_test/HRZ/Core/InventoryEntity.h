#pragma once

#include "../PCore/Util.h"

#include "ViewDependentEntity.h"

namespace HRZ
{

class InventoryEntity : public ViewDependentEntity
{
public:
	char _pad2C0[0x50];

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~InventoryEntity() override;			// 1
	virtual void InventoryEntityUnknown39();		// 39
	virtual void InventoryEntityUnknown40();		// 40
	virtual void InventoryEntityUnknown41();		// 41
	virtual void InventoryEntityUnknown42();		// 42
	virtual void InventoryEntityUnknown43();		// 43
	virtual void InventoryEntityUnknown44();		// 44
	virtual void InventoryEntityUnknown45();		// 45
	virtual void InventoryEntityUnknown46();		// 46
	virtual void InventoryEntityUnknown47();		// 47
	virtual void InventoryEntityUnknown48();		// 48
	virtual void InventoryEntityUnknown49();		// 49
	virtual void InventoryEntityUnknown50();		// 50
	virtual void InventoryEntityUnknown51();		// 51
	virtual void InventoryEntityUnknown52();		// 52
	virtual void InventoryEntityUnknown53();		// 53
	virtual void InventoryEntityUnknown54();		// 54
	virtual void InventoryEntityUnknown55();		// 55
	virtual void InventoryEntityUnknown56();		// 56
	virtual void InventoryEntityUnknown57();		// 57
	virtual void InventoryEntityUnknown58();		// 58
	virtual void InventoryEntityUnknown59();		// 59
	virtual void InventoryEntityUnknown60();		// 60
	virtual void InventoryEntityUnknown61();		// 61
	virtual void InventoryEntityUnknown62();		// 62
	virtual void InventoryEntityUnknown63();		// 63
	virtual void InventoryEntityUnknown64();		// 64
	virtual void InventoryEntityUnknown65();		// 65
	virtual void InventoryEntityUnknown66();		// 66
	virtual void InventoryEntityUnknown67();		// 67
	virtual void InventoryEntityUnknown68();		// 68
	virtual void InventoryEntityUnknown69();		// 69
	virtual void InventoryEntityUnknown70();		// 70
	virtual void InventoryEntityUnknown71();		// 71
	virtual void InventoryEntityUnknown72();		// 72
	virtual void InventoryEntityUnknown73();		// 73
	virtual void InventoryEntityUnknown74();		// 74
	virtual void InventoryEntityUnknown75();		// 75
	virtual void InventoryEntityUnknown76();		// 76
	virtual void InventoryEntityUnknown77();		// 77
	virtual void InventoryEntityUnknown78();		// 78
	virtual void InventoryEntityUnknown79();		// 79
	virtual void InventoryEntityUnknown80();		// 80
	virtual void InventoryEntityUnknown81();		// 81
};
assert_size(InventoryEntity, 0x310);

}
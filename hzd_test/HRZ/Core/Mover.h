#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "Vec3.h"
#include "WorldTransform.h"

namespace HRZ
{

DECL_RTTI(Mover);

class Mover : public EntityComponent
{
public:
	TYPE_RTTI(Mover);

	char _pad58[0x8];

	virtual const RTTI *GetRTTI() const override;														// 0
	virtual ~Mover() override;																			// 1
	virtual const RTTI *GetComponentRepRTTI() const override;											// 4
	virtual void EntityComponentUnknown07() override;													// 7
	virtual void EntityComponentUnknown08() override;													// 8
	virtual NetEntityComponentState *CreateNetState() override;											// 9
	virtual void MoverUnknown10();																		// 10
	virtual void MoverUnknown11();																		// 11
	virtual void MoveToWorldTransform(const WorldTransform& Transform, float Unknown1, bool Unknown2);	// 12
	virtual void SetWorldTransform(const WorldTransform& Transform);									// 13
	virtual void MoverUnknown14();																		// 14
	virtual void MoverUnknown15();																		// 15
	virtual void MoverUnknown16();																		// 16
	virtual void MoverUnknown17();																		// 17
	virtual void MoverUnknown18();																		// 18
	virtual void MoverUnknown19();																		// 19
	virtual void MoverUnknown20();																		// 20
	virtual void MoverUnknown21();																		// 21
	virtual void MoverUnknown22();																		// 22
	virtual void SetVelocity(const Vec3& Velocity);														// 23
	virtual Vec3 GetVelocity();																			// 24
	virtual void MoverUnknown25();																		// 25
	virtual void MoverUnknown26();																		// 26
	virtual void MoverUnknown27();																		// 27
	virtual void MoverUnknown28();																		// 28
	virtual void MoverUnknown29();																		// 29
	virtual void MoverUnknown30();																		// 30
	virtual void MoverUnknown31();																		// 31
	virtual void MoverUnknown32();																		// 32
	virtual void MoverUnknown33();																		// 33
	virtual void MoverUnknown34();																		// 34
	virtual void MoverUnknown35();																		// 35
};
assert_size(Mover, 0x60);

}
#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class PhysicsCollisionListener
{
public:
	virtual ~PhysicsCollisionListener();		// 0
	virtual bool OnPhysicsContactValidate();	// 1
	virtual void OnPhysicsContactAdded();		// 2
	virtual void OnPhysicsContactProcess();		// 3
	virtual void OnPhysicsContactRemoved();		// 4
	virtual bool OnPhysicsExitBroadPhase();		// 5
};
assert_size(PhysicsCollisionListener, 0x8);

}
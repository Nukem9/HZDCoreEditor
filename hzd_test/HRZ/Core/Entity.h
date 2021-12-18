#pragma once

#include <mutex>

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "WorldPosition.h"
#include "WorldTransform.h"
#include "PhysicsCollisionListener.h"
#include "RTTIObject.h"
#include "CoreObject.h"
#include "EntityComponent.h"
#include "StreamingRefTarget.h"

namespace HRZ
{

class AIEntity;
class AIFaction;
class AttackEventContext;
class CameraPropertiesSet;
class Destructibility;
class EntityActivationPolicy;
class InstigatorData;
class Model;
class Mover;
class MoverResource;

DECL_RTTI(Entity);
DECL_RTTI(EntityRep);
DECL_RTTI(EntityResource);

template<typename T>
class ListNode
{
public:
	T *m_Value;
	ListNode<T> *m_Next;
};

namespace EntityMessaging
{
class Queue
{
public:
	char _pad0[0x20];
};
assert_size(Queue, 0x20);
}

class EntityResource : public Resource, public StreamingRefTarget
{
public:
	TYPE_RTTI(EntityResource);

	char _pad38[0xF8];

	virtual const RTTI *GetRTTI() const override;							// 0
	virtual ~EntityResource() override;										// 1
	virtual bool CoreObjectUnknown06() override;							// 6
	virtual void CoreObjectUnknown07() override;							// 7 GetAnimatorResource()?
	virtual void CoreObjectUnknown08() override;							// 8 SetAnimatorResource(Ref<>)?
	virtual const RTTI *GetInstanceRTTI() const;							// 18
	virtual const RTTI *GetRepRTTI() const;									// 19
	virtual const RTTI *GetNetRTTI() const;									// 20
	virtual Entity *CreateInstance(const WorldTransform& Transform);		// 21
	virtual Entity *EntityResourceUnknown22();								// 22 CreateInstance() with specific settings?
	virtual int EntityResourceUnknown23();									// 23
	virtual bool EntityResourceUnknown24();									// 24
	virtual void EntityResourceUnknown25();									// 25
	virtual void EntityResourceUnknown26();									// 26
	virtual void EntityResourceUnknown27();									// 27 Creates attack event?
};
assert_size(EntityResource, 0x130);

class EntityRep
{
public:
	TYPE_RTTI(EntityRep);
};

class Entity : public CoreObject, public WeakPtrTarget, public PhysicsCollisionListener
{
public:
	TYPE_RTTI(Entity);

	enum EntityFlag : uint32_t
	{
		WorldTransformChanged = 0x1,
		IsVisible = 0x2,
		IsDead = 0x80,
		IsSleeping = 0x100,
		HasParent = 0x200,
		IsDispensable = 0x800,
		HasCollisionVolume = 0x4000,
		VisualBoundsUpdatePending = 0x800000,
		// PreviousWorldTransformChanged = 0x8000000, ?
		PreventComponentModification = 0x80000000,
	};

	String m_Name;										// +0x38
	char _pad40[0x8];
	AIEntity *m_AIEntity;								// +0x48 Pointer is deleted in dtor
	RecursiveLock m_DataLock;							// +0x50
	char _pad78[0x10];
	uint32_t m_UpdateStepTicks;							// +0x88
	char _pad90[0x50];
	WorldTransform m_PreviousOrientation;				// +0xE0 Used by EntityRep code (MsgUpdateBlending)
	WorldTransform m_Orientation;						// +0x120 Lock
	char _pad160[0x20];
	StreamingRef<EntityResource> m_Resource;			// +0x180
	Ref<EntityActivationPolicy> m_ActivationPolicy;		// +0x1A0
	char _pad1A8[0x20];
	Entity *m_Parent;									// +0x1C8
	ListNode<Entity *> m_Children;						// +0x1D0
	Mover *m_Mover;										// +0x1E0 Lock
	Model *m_Model;										// +0x1E8 Lock
	Destructibility *m_Destructibility;					// +0x1F0 Lock
	EntityMessaging::Queue m_MessageQueue;				// +0x1F8 Lock
	char _pad218[0x10];
	AIFaction *m_Faction;								// +0x228
	uint32_t m_Flags;									// +0x230
	char _pad238[0x8];
	InstigatorData *m_InstigatorData;					// +0x240 Lock WeakPtr<>
	char _pad248[0x38];
	Ref<AttackEventContext> m_AttackEventContext;		// +0x280 Lock
	EntityComponentContainer m_Components;				// +0x288 Lock
	char _pad290[0x18];

	virtual const RTTI *GetRTTI() const override;													// 0
	virtual ~Entity() override;																		// 1
	virtual String& GetName() const override;														// 5
	virtual void CoreObjectUnknown09() override;													// 9
	virtual void CoreObjectUnknown10() override;													// 10
	virtual void CoreObjectUnknown11() override;													// 11
	virtual void SetName(String Name);																// 16
	virtual void PlaceOnWorldTransform(const WorldTransform& Transform, bool RelativeCoordinates);	// 17
	virtual WorldTransform EntityUnknown18(WorldTransform& Transform);								// 18 No idea. Assigns a2 to a3.
	virtual void SetResource(EntityResource *Resource);												// 19
	virtual void EntityUnknown20();																	// 20 Calculates AABB from current Model?
	virtual void EntityUnknown21();																	// 21
	virtual void EntityUnknown22();																	// 22
	virtual void EntityUnknown23();																	// 23
	virtual void EntityUnknown24();																	// 24
	virtual void EntityUnknown25();																	// 25
	virtual void EntityUnknown26();																	// 26
	virtual void EntityUnknown27();																	// 27 SetSleeping(bool)?
	virtual void EntityUnknown28();																	// 28 WakeUp()?
	virtual void EntityUnknown29();																	// 29
	virtual void EntityUnknown30();																	// 30
	virtual class Player *IsPlayer();																// 31
	virtual void EntityUnknown32();																	// 32
	virtual void EntityUnknown33();																	// 33
	virtual void EntityUnknown34();																	// 34
	virtual void ToggleCollisionVolume(bool Enable);												// 35
	virtual void EntityUnknown36();																	// 36
	virtual void EntityUnknown37();																	// 37
	virtual void EntityUnknown38();																	// 38

	virtual bool OnPhysicsContactValidate() override;												// 1
	virtual void OnPhysicsContactAdded() override;													// 2
	virtual void OnPhysicsContactProcess() override;												// 3
	virtual void OnPhysicsContactRemoved() override;												// 4
	virtual bool OnPhysicsExitBroadPhase() override;												// 5

	void SetTransform(const WorldTransform& Transform)
	{
		std::lock_guard lock(m_DataLock);

		m_Orientation = Transform;
		m_Flags |= WorldTransformChanged;
	}

	void AddComponent(EntityComponent *Component)
	{
		return Offsets::CallID<"Entity::AddComponent", void(*)(Entity *, EntityComponent *)>(this, Component);
	}

	void RemoveComponent(EntityComponent *Component)
	{
		return Offsets::CallID<"Entity::RemoveComponent", void(*)(Entity *, EntityComponent *)>(this, Component);
	}
};
assert_offset(Entity, m_Name, 0x38);
assert_offset(Entity, m_DataLock, 0x50);
assert_offset(Entity, m_UpdateStepTicks, 0x88);
assert_offset(Entity, m_PreviousOrientation, 0xE0);
assert_offset(Entity, m_Orientation, 0x120);
assert_offset(Entity, m_Resource, 0x180);
assert_offset(Entity, m_ActivationPolicy, 0x1A0);
assert_offset(Entity, m_Parent, 0x1C8);
assert_offset(Entity, m_Children, 0x1D0);
assert_offset(Entity, m_Mover, 0x1E0);
assert_offset(Entity, m_Model, 0x1E8);
assert_offset(Entity, m_Destructibility, 0x1F0);
assert_offset(Entity, m_MessageQueue, 0x1F8);
assert_offset(Entity, m_Flags, 0x230);
assert_offset(Entity, m_InstigatorData, 0x240);
assert_offset(Entity, m_AttackEventContext, 0x280);
assert_offset(Entity, m_Components, 0x288);
assert_size(Entity, 0x2C0);

}
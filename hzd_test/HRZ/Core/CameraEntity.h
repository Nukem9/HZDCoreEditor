#pragma once

#include "../PCore/Common.h"

#include "Entity.h"

namespace HRZ
{

DECL_RTTI(CameraEntity);
DECL_RTTI(CameraEntityRep);
DECL_RTTI(CameraEntityResource);

class CameraEntityResource : public EntityResource
{
public:
	TYPE_RTTI(CameraEntityResource);

	char _pad130[0xA8];

	virtual const RTTI *GetRTTI() const override;								// 0
	virtual ~CameraEntityResource() override;									// 1
	virtual const RTTI *GetInstanceRTTI() const override;						// 18
	virtual const RTTI *GetRepRTTI() const override;							// 19
	virtual const RTTI *GetNetRTTI() const override;							// 20
	virtual Entity *CreateInstance(const WorldTransform& Transform) override;	// 21
	virtual void CameraEntityResourceUnknown28();								// 28
	virtual void CameraEntityResourceUnknown29();								// 29
};
assert_size(CameraEntityResource, 0x1D8);

class CameraEntityRep : public EntityRep
{
	TYPE_RTTI(CameraEntityRep);
};

class CameraEntity : public Entity
{
public:
	TYPE_RTTI(CameraEntity);

	char _pad2C0[0x84];
	float m_FOV;			// 0x344
	char _pad34C[0x28];
	float m_NearFuzzy;		// 0x370
	float m_NearSharp;		// 0x374
	float m_FarFuzzy;		// 0x378
	float m_FarSharp;		// 0x37C
	float m_MaxFuzzyNear;	// 0x380
	float m_MaxFuzzyFar;	// 0x384
	char _pad388[0x2C];
	float m_NearPlane;		// 0x3B4
	float m_FarPlane;		// 0x3B8
	float m_StereoDepth;	// 0x3BC
	char _pad3C0[0xB0];

	virtual const RTTI *GetRTTI() const override;					// 0
	virtual ~CameraEntity() override;								// 1
	virtual void SetResource(EntityResource *Resource) override;	// 19
	virtual void EntityUnknown29() override;						// 29
	virtual void EntityUnknown30() override;						// 30
	virtual class Player *IsPlayer() override;						// 31
	virtual void CameraEntityUnknown39();							// 39
	virtual void CameraEntityUnknown40();							// 40
	virtual void CameraEntityUnknown41();							// 41
	virtual void CameraEntityUnknown42();							// 42
	virtual void CameraEntityUnknown43();							// 43
	virtual void CameraEntityUnknown44();							// 44
	virtual void CameraEntityUnknown45();							// 45
	virtual void CameraEntityUnknown46();							// 46
	virtual void CameraEntityUnknown47();							// 47
};
assert_offset(CameraEntity, m_FOV, 0x344);
assert_offset(CameraEntity, m_NearFuzzy, 0x370);
assert_offset(CameraEntity, m_NearPlane, 0x3B4);
assert_size(CameraEntity, 0x470);

}
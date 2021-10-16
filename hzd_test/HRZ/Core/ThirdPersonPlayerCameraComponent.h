#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "WorldTransform.h"

namespace HRZ
{

class ThirdPersonPlayerCameraComponent : public EntityComponent
{
public:
	char _pad58[0x50];
	WorldTransform m_Transform; // 0xA8
	float m_FOV;				// 0xE8
	char _padEC[0x184];

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~ThirdPersonPlayerCameraComponent() override;		// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_offset(ThirdPersonPlayerCameraComponent, m_Transform, 0xA8);
assert_offset(ThirdPersonPlayerCameraComponent, m_FOV, 0xE8);
assert_size(ThirdPersonPlayerCameraComponent, 0x270);

}
#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"

namespace HRZ
{

class AmbienceCycle;

class ClimateBase : public CoreObject
{
public:
	Ref<AmbienceCycle> m_AmbienceCycle;	// 0x20
	float m_NightTemperature;			// 0x28
	float m_DayTemperature;				// 0x2C

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~ClimateBase() override;				// 1
	virtual void ClimateBaseUnknown16() = 0;		// 16
};
assert_size(ClimateBase, 0x30);

}
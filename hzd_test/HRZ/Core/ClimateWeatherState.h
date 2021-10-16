#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"
#include "FRange.h"

namespace HRZ
{

class WeatherSetup;

DECL_RTTI(ClimateWeatherState);

class ClimateWeatherState : public CoreObject
{
public:
	TYPE_RTTI(ClimateWeatherState);

	Ref<WeatherSetup> m_WeatherSetup;	// 0x20
	FRange m_DurationInMinutes;			// 0x28
	float m_TransitionTime;				// 0x30
	float m_Probability;				// 0x34
	float m_TimeOfDayStart;				// 0x38
	float m_TimeOfDayEnd;				// 0x3C

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~ClimateWeatherState() override;		// 1
};
assert_size(ClimateWeatherState, 0x40);

}
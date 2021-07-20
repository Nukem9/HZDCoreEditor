#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"
#include "FRange.h"

namespace HRZ
{

class WeatherSetup;

class ClimateWeatherState : public CoreObject
{
public:
	Ref<WeatherSetup> m_WeatherSetup;	// 0x20
	FRange m_DurationInMinutes;			// 0x28
	float m_TransitionTime;				// 0x30
	float m_Probability;				// 0x34
	float m_TimeOfDayStart;				// 0x38
	float m_TimeOfDayEnd;				// 0x3C

	virtual const GGRTTI *GetRTTI() const override;	// 0
	virtual ~ClimateWeatherState() override;		// 1
};
assert_size(ClimateWeatherState, 0x40);

}
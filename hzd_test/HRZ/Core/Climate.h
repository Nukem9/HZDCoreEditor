#pragma once

#include "../PCore/Common.h"

#include "ClimateBase.h"

namespace HRZ
{

class ClimateWeatherState;
class WorldDataType;

DECL_RTTI(Climate);

class Climate : public ClimateBase
{
public:
	TYPE_RTTI(Climate);

	Array<Ref<ClimateWeatherState>> m_WeatherStates;// 0x30
	Ref<WorldDataType> m_WorldDataType;				// 0x40

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~Climate() override;					// 1
	virtual void ClimateBaseUnknown16() override;	// 16
};
assert_size(Climate, 0x48);

}
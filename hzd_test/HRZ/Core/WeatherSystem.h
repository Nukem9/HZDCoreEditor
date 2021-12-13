#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "CoreObject.h"
#include "FRange.h"
#include "BoundingBox3.h"

namespace HRZ
{

class Climate;
class ClimateWeatherState;
class ForceFieldInstance;
class RenderEffectResource;
class WeatherSetup;
class WindSimulationForceField;

DECL_RTTI(WeatherSystem);

class WeatherSystem : public CoreObject
{
public:
	TYPE_RTTI(WeatherSystem);

	char _pad20[0x110];
	SharedLock m_Mutex;											// 0x130
	char _pad138[0x28];
	Ref<RenderEffectResource> m_SimulationRenderEffectResource;	// 0x160
	char _pad168[0x28];
	Ref<Climate> m_Climate;										// 0x190
	Ref<ClimateWeatherState> m_ClimateWeatherState;				// 0x198
	Ref<WeatherSetup> m_DefaultWeatherSetup;					// 0x1A0
	Ref<WeatherSetup> m_CurrentWeatherSetup;					// 0x1A8
	Ref<WeatherSetup> m_PreviousWeatherSetup;					// 0x1B0
	char _pad1B8[0x10];
	Array<ForceFieldInstance> m_ForceFieldInstances;			// 0x1E8
	Array<WindSimulationForceField> m_WindSimulationForceFields;// 0x1D8
	char _pad1E8[0x48];
	BoundingBox3 m_WorldBounds;									// 0x230
	FRange m_TemperatureRange;									// 0x250
	char _pad260[0xC];
	float m_IndoorClimateFraction;								// 0x264
	char _pad268[0xEC];
	float m_WetnessDryingTime;									// 0x354
	float m_WetnessSaturationTime;								// 0x358
	char _pad35C[0xA4];

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~WeatherSystem() override;				// 1

	void SetWeatherOverride(const WeatherSetup *Setup, float TransitionTime, int Unknown)
	{
		Offsets::CallID<"WeatherSystem::SetWeatherOverride", void(*)(WeatherSystem *, const WeatherSetup *, float, int)>(this, Setup, TransitionTime, Unknown);
	}
};
assert_offset(WeatherSystem, m_SimulationRenderEffectResource, 0x160);
assert_offset(WeatherSystem, m_Climate, 0x190);
assert_offset(WeatherSystem, m_WindSimulationForceFields, 0x1D8);
assert_offset(WeatherSystem, m_WorldBounds, 0x230);
assert_offset(WeatherSystem, m_TemperatureRange, 0x250);
assert_offset(WeatherSystem, m_IndoorClimateFraction, 0x264);
assert_offset(WeatherSystem, m_WetnessSaturationTime, 0x358);
assert_size(WeatherSystem, 0x400);

}
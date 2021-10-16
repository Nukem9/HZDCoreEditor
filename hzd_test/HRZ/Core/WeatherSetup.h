#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"
#include "StreamingRefTarget.h"
#include "FRange.h"

namespace HRZ
{

class Texture;
class WeatherAmbienceCycle;

DECL_RTTI(WeatherSetup);
DECL_RTTI(WeatherSetupSettings);

class WeatherSetupSettings
{
public:
	TYPE_RTTI(WeatherSetupSettings);

	float m_CloudCoverage;						// 0x0
	float m_CloudCoverageVariation;				// 0x4
	float m_CloudCoverageVariationFrequency;	// 0x8
	float m_CloudCoverageNoise1Amplitude;		// 0xC
	float m_CloudCoverageNoise1Frequency;		// 0x10
	float m_CloudCoverageNoise2Amplitude;		// 0x14
	float m_CloudCoverageNoise2Frequency;		// 0x18
	float m_CloudConnectivity;					// 0x1C
	float m_CloudDensityExponent;				// 0x20
	float m_CloudDensityScale;					// 0x24
	float m_CloudType;							// 0x28
	float m_CloudTypeVariation;					// 0x2C
	float m_CloudTypeVariationFrequency;		// 0x30
	float m_CloudScrollSpeed;					// 0x34
	float m_CloudAnvilAmount;					// 0x38
	float m_CloudAnvilSkew;						// 0x3C
	float m_CloudCustomWindDirectionBlendFactor;// 0x40
	float m_CloudCustomWindDirection;			// 0x44
	float m_CloudCustomWindSpeed;				// 0x48
	float m_CloudHeightOffset;					// 0x4C
	float m_CloudNoiseFrequency;				// 0x50
	float m_Precipitation;						// 0x54
	float m_PrecipitationVariation;				// 0x58
	float m_PrecipitationVariationFrequency;	// 0x5C
	float m_RainbowIntensity;					// 0x60
	float m_SundogIntensity;					// 0x64
	float m_CirrusCloudDensity;					// 0x68
	float m_Humidity;							// 0x6C
	FRange m_WindSpeed;							// 0x70
	FRange m_WindDirectionAngle;				// 0x78
	FRange m_TemperatureLimits;					// 0x80
};
assert_size(WeatherSetupSettings, 0x88);

class WeatherSetup : public CoreObject, public StreamingRefTarget
{
public:
	TYPE_RTTI(WeatherSetup);

	String m_Name;								// 0x30
	WeatherSetupSettings m_Settings;			// 0x38
	Ref<WeatherAmbienceCycle> m_AmbienceCycle;	// 0xC0
	Ref<Texture> m_CustomWeatherMap;			// 0xC8

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~WeatherSetup() override;				// 1
	virtual String& GetName() override;				// 5
	virtual void SetName(String Name);				// 16
};
assert_size(WeatherSetup, 0xD0);

}
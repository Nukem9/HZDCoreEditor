#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "RTTIRefObject.h"
#include "PropertyContainer.h"

namespace HRZ
{

class WorldStateResource;

DECL_RTTI(WorldState);

class WorldState : public RTTIRefObject
{
public:
	TYPE_RTTI(WorldState);

	PropertyContainer m_PropertyContainer;		// 0x20
	float m_TimeOfDay;							// 0x98
	float m_Unknown9C;							// 0x9C
	uint32_t m_DayNightCycleCount;				// 0xA0
	bool m_DayNightCycleEnabled;				// 0xA4
	bool m_PauseTimeOfDay;						// 0xA5
	float m_FastForwardAmount;					// 0xA8
	float m_FastForwardAmountPerTick;			// 0xAC
	Ref<WorldStateResource> m_Resource;			// 0xB0

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~WorldState() override;					// 1

	void SetTimeOfDay(float Time, float FastForwardDuration)
	{
		const float currentTime = m_TimeOfDay;

		if (FastForwardDuration <= 0)
		{
			// The original game code is incredibly stupid and complex for what amounts to this (simplified) equation
			float newTime = Time;

			if (Time < currentTime)
				newTime = Time + 24;

			while (newTime >= 24)
			{
				newTime -= 24;
				m_DayNightCycleCount++;
			}

			m_TimeOfDay = newTime;
			m_FastForwardAmount = 0;
		}
		else
		{
			float fastForwardHours = (Time < currentTime) ? (24 - currentTime + Time) : (Time - currentTime);

			m_FastForwardAmount = fastForwardHours * 3600;
			m_FastForwardAmountPerTick = m_FastForwardAmount / FastForwardDuration;
		}
	}
};
assert_offset(WorldState, m_PropertyContainer, 0x20);
assert_offset(WorldState, m_TimeOfDay, 0x98);
assert_offset(WorldState, m_Resource, 0xB0);
assert_size(WorldState, 0xB8);

}
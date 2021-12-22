#pragma once

#include <algorithm>

#include "../PCore/Common.h"

#include "Module.h"

namespace HRZ
{

class LevelSettings;
class SlowMotionManager;
class WeatherSystem;
class WorldState;

DECL_RTTI(GameModule);

class GameModule : public Module/*, public NetMessageListener, private NetSubSystemListener*/
{
public:
	TYPE_RTTI(GameModule);

	char _pad28[0x28];
	Ref<LevelSettings> m_LevelSettings;		// 0x50
	Ref<WorldState> m_WorldState;			// 0x58
	char _pad60[0x20];
	float m_ActiveTimescale;				// 0x80
	char _pad84[0x3C];
	float m_UnknownTimescaleModifier;		// 0xC0
	char _padC4[0xC8];
	float m_TimescaleTransitionCurrent;		// 0x18C
	float m_TimescaleTransitionTarget;		// 0x190
	float m_TimescaleTransitionDelta;		// 0x194
	char _pad190[0x138];
	Ref<WeatherSystem> m_WeatherSystem;		// 0x2D0
	char _pad2D8[0x6B0];
	SlowMotionManager *m_SlowMotionManager;	// 0x988
	char _pad990[0xF0];

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~GameModule() override;					// 1
	virtual bool Initialize() override;				// 16
	virtual void Shutdown() override;				// 17
	virtual void ModuleUnknown18() override;		// 18
	virtual void ModuleUnknown19() override;		// 19
	virtual bool Suspend() override;				// 20
	virtual bool Resume() override;					// 21

	void SetTimescale(float Timescale, float TransitionTime)
	{
		Timescale = std::clamp(Timescale, 0.0f, 1.0f);
		m_TimescaleTransitionTarget = Timescale;

		if (TransitionTime > 0.0f)
		{
			m_TimescaleTransitionDelta = (Timescale - m_TimescaleTransitionCurrent) / TransitionTime;
		}
		else
		{
			m_TimescaleTransitionCurrent = Timescale;

			if (!IsSuspended())
			{
				m_ActiveTimescale = Timescale;
				m_UnknownTimescaleModifier = 1.0f;
			}
			else
			{
				m_ActiveTimescale = 0.0f;
				m_UnknownTimescaleModifier = 0.0f;
			}
		}
	}
};
assert_offset(GameModule, m_LevelSettings, 0x50);
assert_offset(GameModule, m_ActiveTimescale, 0x80);
assert_offset(GameModule, m_UnknownTimescaleModifier, 0xC0);
assert_offset(GameModule, m_TimescaleTransitionCurrent, 0x18C);
assert_offset(GameModule, m_WeatherSystem, 0x2D0);
assert_offset(GameModule, m_SlowMotionManager, 0x988);
assert_size(GameModule, 0xA80);

}
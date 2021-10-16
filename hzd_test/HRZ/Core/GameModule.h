#pragma once

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
	char _pad60[0x270];
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
};
assert_offset(GameModule, m_LevelSettings, 0x50);
assert_offset(GameModule, m_WeatherSystem, 0x2D0);
assert_offset(GameModule, m_SlowMotionManager, 0x988);
assert_size(GameModule, 0xA80);

}
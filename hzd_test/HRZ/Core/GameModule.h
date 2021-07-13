#pragma once

#include "../PCore/Common.h"

#include "Module.h"

namespace HRZ
{

class SlowMotionManager;

class GameModule : public Module/*, public NetMessageListener, private NetSubSystemListener*/
{
public:
	static inline auto*& Instance = *ResolveOffset<GameModule **>(0x711F040);

	char _pad28[0x960];
	SlowMotionManager *m_SlowMotionManager;
	char _pad990[0xF0];

	virtual const GGRTTI *GetRTTI() const override;	// 0
	virtual ~GameModule() override;					// 1
	virtual bool Initialize() override;				// 16
	virtual void Shutdown() override;				// 17
	virtual void ModuleUnknown18() override;		// 18
	virtual void ModuleUnknown19() override;		// 19
	virtual bool Suspend() override;				// 20
	virtual bool Resume() override;					// 21
};
assert_offset(GameModule, m_SlowMotionManager, 0x988);
assert_size(GameModule, 0xA80);

}
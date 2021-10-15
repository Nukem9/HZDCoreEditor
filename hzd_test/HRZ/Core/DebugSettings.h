#pragma once

#include "../PCore/Common.h"

#include "RTTIObject.h"

namespace HRZ
{

DECL_RTTI(DebugSettings);

enum class EGodMode : int
{
	Off = 0,
	On = 1,
	Invulnerable = 2,
};

class DebugSettings : public RTTIObject
{
public:
	TYPE_RTTI(DebugSettings);

	char _pad8[0x4];
	bool m_PlayerCoverEnabled;		// 0xC
	char _padD[0x3];
	bool m_DrawDebugCoordinates;	// 0x10
	char _pad11[0xE];
	bool m_SimulateGameFinished;	// 0x1F
	bool m_UnlockAll;				// 0x20
	char _pad21[0x1];
	bool m_DisableInactivityCheck;	// 0x22
	char _pad23[0x1];
	EGodMode m_GodModeState;		// 0x24
	bool m_InfiniteAmmoReserves;	// 0x28
	char _pad29[0x2];
	bool m_Unknown2B;				// 0x2B
	char _pad2C[0x8];
	bool m_InfiniteAmmo;			// 0x34
	char _pad35[0x8];

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~DebugSettings() override;				// 1
};
assert_offset(DebugSettings, m_PlayerCoverEnabled, 0xC);
assert_offset(DebugSettings, m_DrawDebugCoordinates, 0x10);
assert_offset(DebugSettings, m_SimulateGameFinished, 0x1F);
assert_offset(DebugSettings, m_DisableInactivityCheck, 0x22);
assert_offset(DebugSettings, m_GodModeState, 0x24);
assert_offset(DebugSettings, m_InfiniteAmmoReserves, 0x28);
assert_offset(DebugSettings, m_Unknown2B, 0x2B);
assert_offset(DebugSettings, m_InfiniteAmmo, 0x34);
assert_size(DebugSettings, 0x40);

}
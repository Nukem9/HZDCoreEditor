#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

class CursorManager;
class DebugSettings;
class Game;
class GameModule;
class GameState;
class LevelData;
class MenuModule;
class PlayerProfile;

DECL_RTTI(Application);

class Application
{
public:
	TYPE_RTTI(Application);

	char _pad8[0x10];
	StreamingRef<Game> m_Game;				// 0x18
	StreamingRef<LevelData> m_LevelData;	// 0x38
	char _pad58[0xEE0];
	Ref<GameModule> m_GameModule;			// 0xF38
	Ref<MenuModule> m_MenuModule;			// 0xF40
	Ref<GameState> m_GameState;				// 0xF48
	Ref<PlayerProfile> m_PlayerProfile;		// 0xF50
	DebugSettings *m_DebugSettings;			// 0xF58
	char _padF60[0x158];
	String m_ApplicationTitle;				// 0x10B8
	CursorManager *m_CursorManager;			// 0x10C0
	char _pad10C8[0x118];

	virtual ~Application(); // 0

	static Application& Instance()
	{
		return *Offsets::Resolve<Application *>(0x7120E40);
	}
};
assert_offset(Application, m_Game, 0x18);
assert_offset(Application, m_LevelData, 0x38);
assert_offset(Application, _pad58, 0x58);
assert_offset(Application, m_GameModule, 0xF38);
assert_offset(Application, m_DebugSettings, 0xF58);
assert_offset(Application, m_ApplicationTitle, 0x10B8);
assert_offset(Application, m_CursorManager, 0x10C0);
assert_size(Application, 0x11E0);

}
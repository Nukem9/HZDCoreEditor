#pragma once

#include "../PCore/Common.h"

#include "Player.h"

namespace HRZ
{

DECL_RTTI(PlayerGame);

class PlayerGame : public Player
{
public:
	TYPE_RTTI(PlayerGame);

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~PlayerGame() override;								// 1
	virtual void NetReplicatedObjectUnknown02() override;		// 2
	virtual void NetReplicatedObjectUnknown03() override;		// 3
	virtual void NetReplicatedObjectUnknown04() override;		// 4
	virtual void NetReplicatedObjectUnknown05() override;		// 5
	virtual void NetReplicatedObjectUnknown10() override;		// 10
	virtual void PlayerUnknown11() override;					// 11
	virtual void PlayerUnknown12() override;					// 12
	virtual void PlayerUnknown13() override;					// 13
	virtual void PlayerUnknown14() override;					// 14
	virtual void PlayerUnknown15() override;					// 15
	virtual void PlayerUnknown16() override;					// 16
	virtual void PlayerUnknown17() override;					// 17
	virtual bool PlayerUnknown18() override;					// 18
	virtual bool PlayerUnknown19() override;					// 19
	virtual void SetPlayerFaction(AIFaction *Faction) override;	// 20
	virtual void PlayerUnknown21() override;					// 21
	virtual void PlayerUnknown22() override;					// 22
	virtual void PlayerUnknown23() override;					// 23
	virtual void SerializeToStream(void *Stream) override;		// 24
	virtual void DeserializeFromStream(void *Stream) override;	// 25

	char _pad128[0x248];		// 0x128
	bool m_RestartOnSpawned;	// 0x370
	char _pad371[0xF];			// 0x371
};
assert_offset(PlayerGame, m_RestartOnSpawned, 0x370);
assert_size(PlayerGame, 0x380);

}
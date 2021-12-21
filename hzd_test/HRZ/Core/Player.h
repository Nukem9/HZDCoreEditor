#pragma once

#include <mutex>

#include "../../Offsets.h"
#include "../PCore/Common.h"

#include "CameraEntity.h"
#include "NetReplicatedObject.h"

namespace HRZ
{

class AIFaction;

DECL_RTTI(Player);

class Player : public NetReplicatedObject, public WeakPtrTarget
{
public:
	TYPE_RTTI(Player);

	char _pad50[0x8];
	String m_Name;							// 0x58
	String m_Unknown60;						// 0x60 Clan tag?
	Entity *m_PlayerEntity;					// 0x68
	Entity *m_Entity;						// 0x70
	char _pad78[0x8];
	AIFaction *m_Faction;					// 0x80
	char _pad88[0x50];
	Array<WeakPtr<CameraEntity>> m_Cameras;	// 0xD8
	mutable RecursiveLock m_DataLock;		// 0xE8
	char _pad[0x18];

	virtual const RTTI *GetRTTI() const override;			// 0
	virtual ~Player() override;								// 1
	virtual void NetReplicatedObjectUnknown04() override;	// 4
	virtual void NetReplicatedObjectUnknown05() override;	// 5
	virtual void PlayerUnknown11();							// 11
	virtual void PlayerUnknown12();							// 12
	virtual void PlayerUnknown13();							// 13
	virtual void PlayerUnknown14();							// 14
	virtual void PlayerUnknown15();							// 15
	virtual void PlayerUnknown16();							// 16
	virtual void PlayerUnknown17();							// 17
	virtual bool PlayerUnknown18();							// 18
	virtual bool PlayerUnknown19();							// 19
	virtual void SetFaction(AIFaction *Faction);			// 20
	virtual void PlayerUnknown21();							// 21
	virtual void PlayerUnknown22();							// 22
	virtual void PlayerUnknown23();							// 23
	virtual void SerializeToStream(void *Stream);			// 24
	virtual void DeserializeFromStream(void *Stream);		// 25

	// WARNING: Do NOT call this function while other entity locks are held. It can deadlock the main thread.
	CameraEntity *GetLastActivatedCamera() const
	{
		std::lock_guard lock(m_DataLock);

		if (!m_Cameras.empty())
			return m_Cameras[m_Cameras.size() - 1].get();

		return nullptr;
	}

	static Player *GetLocalPlayer(int Index = 0)
	{
		return Offsets::CallID<"Player::GetLocalPlayer", Player *(*)(int)>(Index);
	}
};
assert_offset(Player, m_Name, 0x58);
assert_offset(Player, m_Unknown60, 0x60);
assert_offset(Player, m_PlayerEntity, 0x68);
assert_offset(Player, m_Entity, 0x70);
assert_offset(Player, m_Faction, 0x80);
assert_offset(Player, m_Cameras, 0xD8);
assert_size(Player, 0x128);

}
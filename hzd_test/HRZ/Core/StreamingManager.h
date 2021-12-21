#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "RTTIRefObject.h"

namespace HRZ
{

class CoreFileManager;

class StreamingManager
{
public:
	char _pad0[0x90];
	CoreFileManager *m_CoreFileManager;					// 0x90
	char _pad98[0x20];
	HashSet<Ref<RTTIRefObject>> m_ActiveStreamObjects;	// 0xB8
	RecursiveLock m_StreamObjectsLock;					// 0xC8

	static StreamingManager *Instance()
	{
		return *Offsets::Resolve<StreamingManager **>(0x712D0C0);
	}
};
assert_offset(StreamingManager, m_CoreFileManager, 0x90);
assert_offset(StreamingManager, m_ActiveStreamObjects, 0xB8);
assert_offset(StreamingManager, m_StreamObjectsLock, 0xC8);
//assert_size(StreamingManager, 0x200F8);

}
#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "IStreamingManager.h"

namespace HRZ
{

class CoreFileManager;

class StreamingManager : public IStreamingManager
{
public:
	virtual ~StreamingManager() override;																				// 0
	virtual void CreateHandleFromLink(AssetLink& Link) override;														// 1
	virtual void CreateHandleFromPath(StreamingRefHandle& Handle, const AssetPath& Path, const GGUUID& UUID) override;	// 2
	virtual void CreateHandleFromObject(StreamingRefHandle& Handle, RTTIRefObject *Object, uint8_t Flags) override;		// 3
	virtual void IStreamingManagerUnknown04(StreamingRefHandle&) override;												// 4
	virtual void IStreamingManagerUnknown05() override;																	// 5
	virtual void UpdateLoadState(StreamingRefHandle& Handle, uint8_t Flags) override;									// 6
	virtual void IStreamingManagerUnknown07(StreamingRefHandle&, uint8_t) override;										// 7

	char _pad8[0x88];
	CoreFileManager *m_CoreFileManager;					// 0x90
	char _pad98[0x20];
	HashSet<Ref<RTTIRefObject>> m_ActiveStreamObjects;	// 0xB8
	RecursiveLock m_StreamObjectsLock;					// 0xC8

	static StreamingManager *Instance()
	{
		return *Offsets::ResolveID<"StreamingManager::Instance", StreamingManager **>();
	}
};
assert_offset(StreamingManager, m_CoreFileManager, 0x90);
assert_offset(StreamingManager, m_ActiveStreamObjects, 0xB8);
assert_offset(StreamingManager, m_StreamObjectsLock, 0xC8);
//assert_size(StreamingManager, 0x200F8);

}
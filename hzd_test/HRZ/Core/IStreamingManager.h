#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class AssetPath;
class RTTIRefObject;

class IStreamingManager
{
public:
	struct AssetLink
	{
		StreamingRefHandle *m_Handle = nullptr;
		void *m_Unused8 = nullptr;
		String m_Path;
		GGUUID m_UUID;
	};

	virtual ~IStreamingManager();																					// 0
	virtual void CreateHandleFromLink(AssetLink& Link) = 0;															// 1
	virtual void CreateHandleFromPath(StreamingRefHandle& Handle, const AssetPath& Path, const GGUUID& UUID) = 0;	// 2
	virtual void CreateHandleFromObject(StreamingRefHandle& Handle, RTTIRefObject *Object, uint8_t Flags) = 0;		// 3
	virtual void IStreamingManagerUnknown04(StreamingRefHandle&) = 0;												// 4
	virtual void IStreamingManagerUnknown05() = 0;																	// 5
	virtual void UpdateLoadState(StreamingRefHandle& Handle, uint8_t Flags) = 0;									// 6
	virtual void IStreamingManagerUnknown07(StreamingRefHandle&, uint8_t) = 0;										// 7
};
assert_size(IStreamingManager, 0x8);

}
#pragma once

#include "../PCore/Util.h"
#include "../PCore/UUID.h"

#include "HwResource.h"
#include "IRenderDataCallbackObject.h"
#include "IRenderDataStreamingEventHandler.h"

namespace HRZ
{

class HwTexture : public HwResource, public IRenderDataCallbackObject, public IRenderDataStreamingEventHandler
{
public:
	struct MapData
	{
		uint32_t m_Unknown0 = 0;
		uint32_t m_Unknown4 = 0;
		uint32_t m_Unknown8 = 0;
		uint32_t m_UnknownC = 0;
		uint32_t m_Unknown10 = 0;
		void *m_Buffer = nullptr;
		uint32_t m_Unknown20 = 0;
		uint32_t m_Unknown24 = 0xFFFFFFFF;
		uint32_t m_Unknown28 = 6;
	};
	assert_size(MapData, 0x30);

	virtual ~HwTexture() override;														// 0
	virtual void InitBufferData(void *BinaryReaderStream);								// 2
	virtual void UnmapRange(MapData& Data) = 0;											// 3
	virtual void HwTextureUnknown04();													// 4
	virtual void HwTextureUnknown05();													// 5
	virtual void HwTextureUnknown06();													// 6
	virtual void HwTextureUnknown07() = 0;												// 7
	virtual void HwTextureUnknown08() = 0;												// 8
	virtual void HwTextureUnknown09();													// 9
	virtual void MapRange(MapData& Data, uint32_t, uint32_t, uint32_t, uint32_t) = 0;	// 10
	virtual void HwTextureUnknown11();													// 11

	virtual void IRenderDataCallbackObjectUnknown01() override;	// 1
	virtual void IRenderDataCallbackObjectUnknown02() override;	// 2

	virtual void IRenderDataStreamingEventHandlerUnknown00() override;	// 0
	virtual void IRenderDataStreamingEventHandlerUnknown01() override;	// 1
	virtual void IRenderDataStreamingEventHandlerUnknown02() override;	// 2
	virtual void IRenderDataStreamingEventHandlerUnknown03() override;	// 3
	virtual void IRenderDataStreamingEventHandlerUnknown04() override;	// 4

	MapData Map(uint32_t Unknown1, uint32_t Unknown2)
	{
		MapData data;
		MapRange(data, Unknown1, 0xFFFFFFFF, 6, Unknown2);

		return data;
	}

	// +0x31 EPixelFormat == (*(byte *)(this + 0x31))
	// +0x38 Flags
	// +0x3B ETextureType == (*(byte *)(this + 0x3B) & 7)

	char _pad30[0x50];			// 0x30
	GGUUID m_TextureDataHash;	// 0x80 Actually MurMurHashValue
	char _pad90[0x8];			// 0x90
};
assert_offset(HwTexture, m_TextureDataHash, 0x80);
assert_size(HwTexture, 0x98);

}
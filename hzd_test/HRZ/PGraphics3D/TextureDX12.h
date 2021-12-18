#pragma once

#include "../PCore/Util.h"

#include "CommonDX12.h"
#include "HwTexture.h"
#include "RenderingConfiguration.h"

namespace HRZ
{

class TextureDX12 : public HwTexture
{
public:
	struct BackingResource
	{
		DX12HeapResource m_State;						// 0x0
		CpuDescriptorHandle m_CPUDescriptorHandle;		// 0x10
		Array<CpuDescriptorHandle> m_UnknownCPUHandles;	// 0x18
		char _pad28[0x18];								// 0x28
	};
	assert_size(BackingResource, 0x40);

	virtual ~TextureDX12() override;														// 0
	virtual void HwResourceUnknown01() override;											// 1
	virtual void InitBufferData(void *BinaryReaderStream) override;							// 2
	virtual void UnmapRange(MapData& Data) override;										// 3
	virtual void HwTextureUnknown04() override;												// 4
	virtual void HwTextureUnknown07() override;												// 7
	virtual void HwTextureUnknown08() override;												// 8
	virtual void HwTextureUnknown09() override;												// 9
	virtual void MapRange(MapData& Data, uint32_t, uint32_t, uint32_t, uint32_t) override;	// 10

	virtual void IRenderDataCallbackObjectUnknown01() override;	// 1
	virtual void IRenderDataCallbackObjectUnknown02() override;	// 2

	virtual void IRenderDataStreamingEventHandlerUnknown00() override;	// 0
	virtual void IRenderDataStreamingEventHandlerUnknown01() override;	// 1
	virtual void IRenderDataStreamingEventHandlerUnknown02() override;	// 2
	virtual void IRenderDataStreamingEventHandlerUnknown03() override;	// 3

	uint8_t m_LoadState;								// 0x98
	char _pad99[0xF];									// 0x99
	BackingResource m_Copies[2];						// 0xA8
	Array<BackingResource> m_UploadBuffers;				// 0x128
	Array<BackingResource> m_ReadbackTextureBuffers;	// 0x138
	Array<uint64_t> m_TotalResourceSizes;				// 0x148
	char _pad158[0x60];									// 0x158
	ID3D12Heap *m_Heap;									// 0x1B8
	uint64_t m_HeapStartOffset;							// 0x1C0
	uint64_t m_HeapEndOffset;							// 0x1C8
};
assert_offset(TextureDX12, m_LoadState, 0x98);
assert_offset(TextureDX12, m_Copies, 0xA8);
assert_offset(TextureDX12, m_Heap, 0x1B8);
assert_size(TextureDX12, 0x1D0);

}
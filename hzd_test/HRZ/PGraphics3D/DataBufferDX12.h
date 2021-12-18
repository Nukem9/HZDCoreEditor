#pragma once

#include "../PCore/Common.h"

#include "CommonDX12.h"
#include "HwBuffer.h"

namespace HRZ
{

class DataBufferDX12 : public HwBuffer
{
public:
	struct BackingResource
	{
		DX12HeapResource m_HeapState;								// 0x0
		uint32_t m_BufferByteOffset;								// 0x10 Used in conjunction with GetGPUVirtualAddress
		char _pad14[0x8];
		uint32_t m_SecondaryByteOffset;								// 0x1C
		char _pad20[0x8];
		Microsoft::WRL::ComPtr<ID3D12Resource> m_SecondaryResource;	// 0x28
		char _pad30[0x10];
	};
	assert_offset(BackingResource, m_BufferByteOffset, 0x10);
	assert_offset(BackingResource, m_SecondaryResource, 0x28);
	assert_size(BackingResource, 0x40);

	virtual ~DataBufferDX12() override;															// 0
	virtual void HwResourceUnknown01() override;												// 1
	virtual bool HwBufferUnknown02(uint32_t LoadType) override;									// 2
	virtual void LoadBufferData() override;														// 3
	virtual bool CreateGPUResources() override;													// 5
	virtual void DestroyGPUResources() override;												// 6
	virtual void *MapRange(uint8_t MapFlags, uint32_t StartOffset, uint32_t Length) override;	// 7
	virtual void UnmapRange(void *Unknown1, void *Unknown2) override;							// 8

	virtual void IRenderDataCallbackObjectUnknown01() override;	// 1
	virtual void IRenderDataCallbackObjectUnknown02() override;	// 2

	uint8_t m_LoadState;							// 0x80
	BackingResource m_Copies[3];					// 0x88
	char _pad148[0x8];								// 0x148
	void *m_PersistentlyMappedData;					// 0x150 ID3D12Resource::Map m_Copies[0]
	void *m_VertexCacheUploadData;					// 0x158 Unknown structure
	bool m_UsingGlobalVertexCacheBuffer;			// 0x160 True if m_VertexCacheUploadData is allocated from the global buffer
	char _pad161[0xF];								// 0x161
	CpuDescriptorHandle m_CPUHandles[2];			// 0x170
};
assert_offset(DataBufferDX12, m_LoadState, 0x80);
assert_offset(DataBufferDX12, m_PersistentlyMappedData, 0x150);
assert_offset(DataBufferDX12, m_CPUHandles, 0x170);
assert_size(DataBufferDX12, 0x180);

}
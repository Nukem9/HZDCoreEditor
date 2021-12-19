#pragma once

#include <wrl/client.h>
#include <d3d12.h>

#include "../PCore/Common.h"

namespace HRZ
{

using CpuDescriptorHandle = D3D12_CPU_DESCRIPTOR_HANDLE;
using GpuDescriptorHandle = D3D12_GPU_DESCRIPTOR_HANDLE;

enum class ED3D12CommandListType : uint8_t
{
	Direct = 0,
	Bundle = 1,
	Compute = 2,
	Copy = 3,
	VideoDecode = 4,
	VideoProcess = 5,

	Count = 6,
};

enum class EDX12HeapType : uint8_t
{
	Upload = 0,
	ReadBack = 1,
	VRAM = 2,

	Invalid = 3,
	Count = Invalid,
};

struct DX12HeapResource
{
	uint8_t m_Unknown1 = 4;									// 0x0 Defaults to 4
	EDX12HeapType m_HeapType = EDX12HeapType::Invalid;		// 0x1
	Microsoft::WRL::ComPtr<ID3D12Resource> m_D3DResource;	// 0x8
};
assert_size(DX12HeapResource, 0x10);

}
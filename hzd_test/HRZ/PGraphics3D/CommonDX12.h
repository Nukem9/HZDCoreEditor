#pragma once

#include <wrl/client.h>
#include <d3d12.h>

#include "../PCore/Common.h"

namespace HRZ
{

using CpuDescriptorHandle = D3D12_CPU_DESCRIPTOR_HANDLE;
using GpuDescriptorHandle = D3D12_GPU_DESCRIPTOR_HANDLE;

enum class EDX12HeapType : uint8_t
{
	Upload = 0,
	ReadBack = 1,
	VRAM = 2,
	Custom = 3,
};

struct DX12HeapResource
{
	uint8_t m_Unknown1 = 4;									// 0x0 Defaults to 4
	EDX12HeapType m_HeapType = EDX12HeapType::Custom;		// 0x1
	Microsoft::WRL::ComPtr<ID3D12Resource> m_D3DResource;	// 0x8
};
assert_size(DX12HeapResource, 0x10);

}
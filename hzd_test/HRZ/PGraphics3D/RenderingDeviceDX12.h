#pragma once

#include <d3d12.h>
#include <dxgi.h>

#include "../PCore/Common.h"

namespace HRZ
{

class RenderingDeviceDX12
{
public:
	static inline auto& Instance = *ResolveOffset<RenderingDeviceDX12 *>(0x2D28AA0);

	ID3D12Device *m_Device;
	IDXGIFactory *m_Factory;
	IDXGIAdapter *m_Adapter;
	ID3D12Device *m_RawDevice;

	uint32_t m_MinimumWaveLaneCount;
	bool m_ShaderModelTierSupported;
	bool m_DebugInterfaceEnabled;
	uint64_t m_ResourceUsageCounters[3];
	bool m_DebugBreadcrumbsEnabled;

	HashMap<uint32_t, ID3D12Heap *> m_HeapTable;
	SharedLock m_HeapTableLock;
	HashMap<uint32_t, ID3D12Resource *> m_DefaultResourceTable;
	SharedLock m_DefaultResourceTableLock;
	HashMap<uint32_t, ID3D12DescriptorHeap *> m_DescriptorHeapTable;
	SharedLock m_DescriptorHeapTableLock;

	Array<D3D12_FEATURE_DATA_FORMAT_SUPPORT> m_TypedUAVLoadAdditionalFormats;
	char _pad90[0x10];

	SharedLock m_CommandListTableLock;
	Array<ID3D12CommandList *> m_CommandListTable[6];
};
assert_offset(RenderingDeviceDX12, m_Device, 0x0);
assert_offset(RenderingDeviceDX12, m_RawDevice, 0x18);
assert_offset(RenderingDeviceDX12, m_MinimumWaveLaneCount, 0x20);
assert_offset(RenderingDeviceDX12, m_DebugBreadcrumbsEnabled, 0x40);
assert_offset(RenderingDeviceDX12, m_HeapTable, 0x48);
assert_offset(RenderingDeviceDX12, m_TypedUAVLoadAdditionalFormats, 0x90);
assert_offset(RenderingDeviceDX12, m_CommandListTableLock, 0xB0);
assert_offset(RenderingDeviceDX12, m_CommandListTable, 0xB8);
assert_size(RenderingDeviceDX12, 0x118);

}
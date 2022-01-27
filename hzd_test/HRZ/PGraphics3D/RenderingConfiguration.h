#pragma once

#include <d3d12.h>

#include "../../Offsets.h"

namespace HRZ
{

class RenderingConfiguration
{
public:
	ID3D12DescriptorHeap *GetDescriptorHeap() const
	{
		const static auto structOffset = *Offsets::ResolveID<"RenderingConfigDescriptorHeapOffsetPtr", uint32_t *>();

		return *(ID3D12DescriptorHeap **)((uintptr_t)this + structOffset);
	}

	uint32_t GetDescriptorHandleIncrementSize() const
	{
		const static auto structOffset = *Offsets::ResolveID<"RenderingConfigDescriptorHandleIncrementSizeOffsetPtr", uint32_t *>();

		return *(uint32_t *)((uintptr_t)this + structOffset);
	}

	ID3D12CommandQueue *GetCommandQueue() const
	{
		const static auto structOffset = *Offsets::ResolveID<"RenderingConfigDescriptorCommandQueueOffsetPtr", uint32_t *>();

		return *(ID3D12CommandQueue **)((uintptr_t)this + structOffset);
	}
};

}
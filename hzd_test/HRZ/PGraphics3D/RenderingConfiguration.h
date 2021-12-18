#pragma once

#include <d3d12.h>

namespace HRZ
{

class RenderingConfiguration
{
public:
	uint32_t GetDescriptorHandleIncrementSize() const
	{
		return *(uint32_t *)((uintptr_t)this + 0x48F7C8);
	}

	ID3D12DescriptorHeap *GetDescriptorHeap() const
	{
		return *(ID3D12DescriptorHeap **)((uintptr_t)this + 0x48F7C0);
	}

	ID3D12CommandQueue *GetCommandQueue() const
	{
		return *(ID3D12CommandQueue **)((uintptr_t)this + 0x48F808);
	}
};

}
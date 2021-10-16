#pragma once

#include <d3d12.h>

namespace HRZ
{

class RenderingConfiguration
{
public:
	ID3D12CommandQueue *GetCommandQueue() const
	{
		return *(ID3D12CommandQueue **)((uintptr_t)this + 0x48F840);
	}
};

}
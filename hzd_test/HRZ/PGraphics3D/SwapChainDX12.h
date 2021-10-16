#pragma once

#include <d3d12.h>
#include <dxgi.h>

#include "../PCore/Common.h"

namespace HRZ
{

class HwRenderBuffer;
class RenderingConfiguration;
class SystemWindow;
class TextureDX12;

class SwapChainDX12
{
public:
	constexpr static int BackBufferCount = 3;

	struct BackBuffer
	{
		ID3D12Resource *m_Resource;		// 0
		TextureDX12 *m_Texture;			// 8
		HwRenderBuffer *m_RenderBuffer;	// 10
		void *m_Unknown18;				// 18
		uint64_t m_FrameIndexSignal;	// 20
	};
	assert_size(BackBuffer, 0x28);

	RenderingConfiguration *m_RenderingConfig;	// 0x0
	bool m_Unknown8;							// 0x8
	uint32_t m_BackBufferFormat;				// 0xC Enum type unknown

	SystemWindow *m_SystemWindow;				// 0x10
	bool m_SystemWindowInitialized;				// 0x18
	HWND m_NativeWindowHandle;					// 0x20
	uint32_t m_Width;							// 0x28
	uint32_t m_Height;							// 0x2C

	IDXGISwapChain *m_SwapChain;				// 0x30
	ID3D12Fence *m_PresentFence;				// 0x38
	HANDLE m_FenceEventHandle;					// 0x40
	uint32_t m_CurrentFrameIndex;				// 0x48 First frame is index 1
	BackBuffer m_BackBuffers[BackBufferCount];	// 0x50

	bool m_Initialized;							// 0xC8
	uint32_t m_BaseWidth;						// 0xCC
	uint32_t m_BaseHeight;						// 0xD0
	uint32_t m_RefreshRate;						// 0xD4
	uint16_t m_UnknownD8;						// 0xD8
	bool m_DisplayAllowsTearing;				// 0xDA

	bool Present()
	{
		return CallID<"SwapChainDX12::Present", bool(*)(SwapChainDX12 *)>(this);
	}
};
assert_offset(SwapChainDX12, m_RenderingConfig, 0x0);
assert_offset(SwapChainDX12, m_SystemWindow, 0x10);
assert_offset(SwapChainDX12, m_NativeWindowHandle, 0x20);
assert_offset(SwapChainDX12, m_BackBuffers, 0x50);
assert_offset(SwapChainDX12, m_BaseWidth, 0xCC);
assert_offset(SwapChainDX12, m_BaseHeight, 0xD0);
assert_offset(SwapChainDX12, m_DisplayAllowsTearing, 0xDA);
assert_size(SwapChainDX12, 0xE0);

}
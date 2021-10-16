#pragma once

#include <d3d12.h>

#include "../PCore/Util.h"

namespace HRZ
{

class HwTexture;

class HwRenderBuffer
{
public:
	virtual ~HwRenderBuffer();

	char _pad8[0x8];
	HwTexture *m_Texture;// Ref counted pointer?
	char _pad18[0x10];
	D3D12_CPU_DESCRIPTOR_HANDLE m_DescriptorHandle;
};
assert_offset(HwRenderBuffer, m_Texture, 0x10);
assert_offset(HwRenderBuffer, m_DescriptorHandle, 0x28);

}
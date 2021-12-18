#pragma once

#include "../PCore/Util.h"

#include "CommonDX12.h"

namespace HRZ
{

class HwTexture;

class HwRenderBuffer
{
public:
	virtual ~HwRenderBuffer();

	char _pad8[0x8];							// 0x8
	Ref<HwTexture> m_Texture;					// 0x10
	char _pad18[0x10];							// 0x18
	CpuDescriptorHandle m_CPUDescriptorHandle;	// 0x28
	char _pad30[0x28];							// 0x30
};
assert_offset(HwRenderBuffer, m_Texture, 0x10);
assert_offset(HwRenderBuffer, m_CPUDescriptorHandle, 0x28);
assert_size(HwRenderBuffer, 0x58);

}
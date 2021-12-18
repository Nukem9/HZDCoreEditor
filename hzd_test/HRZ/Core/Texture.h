#pragma once

#include "../PCore/Common.h"

#include "Resource.h"

namespace HRZ
{

class HwTexture;

DECL_RTTI(Texture);

class Texture : public Resource
{
public:
	TYPE_RTTI(Texture);

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~Texture() override;					// 1

	Ref<HwTexture> m_HwTexture;	// 0x28
	uint32_t m_Dimensions[2];	// 0x30
};
assert_offset(Texture, m_Dimensions, 0x30);
assert_size(Texture, 0x38);

}
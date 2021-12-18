#pragma once

#include "../PCore/Util.h"

#include "HwReferencableBase.h"

namespace HRZ
{

class RenderingConfiguration;

class HwManaged : public HwReferencableBase<HwManaged>
{
public:
	virtual ~HwManaged();	// 0

	RenderingConfiguration *m_RenderingConfig;	// 0x10
};
assert_offset(HwManaged, m_RenderingConfig, 0x10);
assert_size(HwManaged, 0x18);

}
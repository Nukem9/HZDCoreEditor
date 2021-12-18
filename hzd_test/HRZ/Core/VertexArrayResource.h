#pragma once

#include "../PCore/Common.h"
#include "../PGraphics3D/HwVertexArray.h"

#include "BaseResource.h"

namespace HRZ
{

DECL_RTTI(VertexArrayResource);

class VertexArrayResource : public BaseResource
{
public:
	TYPE_RTTI(VertexArrayResource);

	Ref<HwVertexArray> m_VertexArray;

	virtual const RTTI *GetRTTI() const override;						// 0
	virtual ~VertexArrayResource() override;							// 1
	virtual CoreObject *CreateResourceInstance(void *, bool) override;	// 16
	virtual void VertexArrayResourceUnknown17();						// 17
};
assert_size(VertexArrayResource, 0x28);

}
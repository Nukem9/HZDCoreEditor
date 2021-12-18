#pragma once

#include "../PCore/Common.h"

#include "BaseResource.h"

namespace HRZ
{

DECL_RTTI(Resource);

class Resource : public BaseResource
{
public:
	TYPE_RTTI(Resource);

	String m_Name; // 0x20

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~Resource() override;								// 1
	virtual String& GetName() const override;					// 5
	virtual void SetName(String Name);							// 17
};
assert_size(Resource, 0x28);

}
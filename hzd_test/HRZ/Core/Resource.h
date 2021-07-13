#pragma once

#include "../PCore/Common.h"

#include "BaseResource.h"

namespace HRZ
{

class Resource : public BaseResource
{
public:
	String m_Name; // 0x20

	virtual const GGRTTI *GetRTTI() const override;				// 0
	virtual ~Resource() override;								// 1
	virtual String& GetName() override;							// 5
	virtual void SetName(String Name);							// 17
};
assert_size(Resource, 0x28);

}
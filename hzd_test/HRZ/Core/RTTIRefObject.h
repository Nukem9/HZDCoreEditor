#pragma once

#include <intrin.h>

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "RTTIObject.h"

namespace HRZ
{

DECL_RTTI(RTTIRefObject);

class RTTIRefObject : public RTTIObject
{
public:
	TYPE_RTTI(RTTIRefObject);

	uint32_t m_RefCount = 0;	// 0x8
	GGUUID m_ObjectUUID;		// 0x10

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~RTTIRefObject() override;				// 1
	virtual void RTTIRefObjectUnknown02();			// 2
	virtual void RTTIRefObjectUnknown03();			// 3

	void IncRef()
	{
		_InterlockedIncrement(&m_RefCount);
	}

	void DecRef()
	{
		Offsets::Call<0x02F5F00, void(*)(RTTIRefObject *)>(this);
	}
};
assert_size(RTTIRefObject, 0x20);

}
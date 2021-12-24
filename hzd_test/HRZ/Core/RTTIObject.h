#pragma once

#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

DECL_RTTI(RTTIObject);

class RTTIObject
{
public:
	TYPE_RTTI(RTTIObject);

	virtual const RTTI *GetRTTI() const;	// 0
	virtual ~RTTIObject();					// 1

	template<typename T>
	bool SetMemberValue(const char *MemberName, const T& Value) const
	{
		return static_cast<const RTTIClass *>(GetRTTI())->SetMemberValue<T>(this, MemberName, Value);
	}

	template<typename T>
	bool GetMemberValue(const char *MemberName, T *OutValue) const
	{
		return static_cast<const RTTIClass *>(GetRTTI())->GetMemberValue<T>(this, MemberName, OutValue);
	}

	template<typename T>
	T& GetMemberRefUnsafe(const char *MemberName)
	{
		return static_cast<const RTTIClass *>(GetRTTI())->GetMemberRefUnsafe<T>(this, MemberName);
	}
};
assert_size(RTTIObject, 0x8);

}
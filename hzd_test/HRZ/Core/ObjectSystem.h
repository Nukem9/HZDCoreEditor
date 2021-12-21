#pragma once

#include "../../Offsets.h"
#include "../PCore/Common.h"

#include "RTTIRefObject.h"

namespace HRZ
{

#if 0
class ObjectSystem
{
public:
	class Query
	{
	public:
		enum Flags : uint32_t
		{
			MatchAny = 0,
			MatchRTTI = 1,
			MatchName = 2,
		};

		const RTTI *m_RTTI = nullptr;
		String m_Name;
		Flags m_Flags = MatchAny;

		Query(const RTTI *RTTIType) : m_RTTI(RTTIType), m_Name("Default")
		{
			// Flags are wrong
			__debugbreak();
		}
	};
	assert_size(Query, 0x18);

	void GetObjects(const Query& Filter, Array<RTTIRefObject *>& Objects)
	{
		Offsets::Call<0x04A0830, void(*)(ObjectSystem *, const Query&, Array<RTTIRefObject *>&)>(this, Filter, Objects);
	}

	static ObjectSystem& Instance()
	{
		return **Offsets::Resolve<ObjectSystem **>(0x2F4F660);
	}
};
#endif

}
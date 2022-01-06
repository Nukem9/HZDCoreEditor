#pragma once

#include "../../Offsets.h"
#include "../PCore/Common.h"

#include "RTTI.h"

namespace HRZ
{

class RTTIObjectTweaker
{
public:
	class SetValueVisitor
	{
	public:
		String m_LastError;	// 0x8

		virtual ~SetValueVisitor() = default;						// 0
		virtual void SetValue(void *Object, const RTTI *Type) = 0;	// 1
		virtual int GetFlags() = 0;									// 2
	};
	assert_size(SetValueVisitor, 0x10);

	static void VisitObjectPath(void *Object, const RTTI *Type, const String& Path, SetValueVisitor *Visitor)
	{
		Offsets::CallID<"RTTIObjectTweaker::VisitObjectPath", void(*)(void *, void *, const RTTI *, const String&, int, SetValueVisitor *)>(nullptr, Object, Type, Path, 0, Visitor);
	}
};

}
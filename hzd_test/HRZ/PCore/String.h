#pragma once

#include <stdint.h>

#include "../../Offsets.h"

#include "Util.h"

namespace HRZ
{

class String final
{
private:
	struct StringRefData
	{
		constexpr static uint32_t InvalidCRC = 0xFFFFFFFF;

		uint32_t m_RefCount;	// 0x0  (- 0x10)
		uint32_t m_CRC;			// 0x4  (- 0xC)
		uint32_t m_Length;		// 0x8  (- 0x8)
		uint32_t _padC;
		//const char m_Text[];	// 0x10 (- 0x0)
	};
	assert_size(StringRefData, 0x10);

	const char *m_Data;

public:
	String()
	{
		Offsets::CallID<"String::CtorCString", String *(*)(String *, const char *)>(this, "");
	}

	String(const char *Value)
	{
		Offsets::CallID<"String::CtorCString", String *(*)(String *, const char*)>(this, Value);
	}

	String(const String& Other) : String()
	{
		Offsets::CallID<"String::AssignFromOther", String *(*)(String *, const String&)>(this, Other);
	}

	~String()
	{
		Offsets::CallID<"String::Dtor", void(*)(String *)>(this);
	}

	String& operator=(const String& Other)
	{
		Offsets::CallID<"String::AssignString", String *(*)(String *, const String&)>(this, Other);
		return *this;
	}

	const char *c_str() const
	{
		return m_Data;
	}

	const char *data() const
	{
		return m_Data;
	}

	size_t size() const
	{
		return InternalData()->m_Length;
	}

	size_t length() const
	{
		return size();
	}

private:
	StringRefData *InternalData() const
	{
		return reinterpret_cast<StringRefData *>(reinterpret_cast<ptrdiff_t>(m_Data) - sizeof(StringRefData));
	}
};

}
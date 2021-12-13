#pragma once

#include <stdint.h>

#include "../../Offsets.h"

#include "Util.h"

namespace HRZ
{

class WString final
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

	StringRefData *m_Data;

public:
	WString()
	{
		Offsets::CallID<"WString::Ctor", WString *(*)(WString *)>(this);
	}

	WString(const wchar_t *Value)
	{
		Offsets::CallID<"WString::CtorCString", WString *(*)(WString *, const wchar_t *)>(this, Value);
	}

	WString(const WString& Other) : WString()
	{
		Offsets::CallID<"WString::AssignFromOther", WString *(*)(WString *, const WString&)>(this, Other);
	}

	~WString()
	{
		Offsets::CallID<"WString::Dtor", void(*)(WString *)>(this);
	}

	WString& operator=(const WString& Other)
	{
		Offsets::CallID<"WString::AssignFromOther", WString *(*)(WString *, const WString&)>(this, Other);
		return *this;
	}

	const wchar_t *c_str() const
	{
		return InternalStr();
	}

	const wchar_t *data() const
	{
		return InternalStr();
	}

	size_t size() const
	{
		return m_Data->m_Length;
	}

	size_t length() const
	{
		return size();
	}

	String EncodeUTF8() const
	{
		String str;
		Offsets::CallID<"WString::EncodeUTF8", void(*)(const WString *, String&)>(this, str);

		return str;
	}

private:
	const wchar_t *InternalStr() const
	{
		return reinterpret_cast<const wchar_t *>(reinterpret_cast<ptrdiff_t>(m_Data) + sizeof(m_Data));
	}
};

}
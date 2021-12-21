#pragma once

#include "Util.h"
#include "String.h"
#include "Ref.h"

namespace HRZ
{

class IStreamingManager;

template<typename T>
class StreamingRef final
{
private:
	enum StreamFlags : uint16_t
	{
		Loaded = 0x80,
	};

	struct StreamData
	{
		uint32_t m_RefCount;			// 0x0
		String m_CorePath;				// 0x8
		uint64_t m_Hash1;				// 0x10
		uint64_t m_Hash2;				// 0x18
		uint64_t m_Unknown20;			// 0x20
		IStreamingManager *m_Manager;	// 0x28
		Ref<T> m_Ref;					// 0x30
	};
	assert_size(StreamData, 0x38);

	StreamData *m_Data;		// 0x0
	char _pad8[0x10];		// 0x8
	StreamFlags m_Flags;	// 0x18

public:
	StreamingRef() = delete;
	StreamingRef(const StreamingRef<T>&) = delete;
	~StreamingRef() = delete;
	StreamingRef<T>& operator=(const StreamingRef<T>&) = delete;

	T *get() const
	{
		if (m_Data && (m_Flags & Loaded) != 0)
			return m_Data->m_Ref.get();

		return nullptr;
	}

	explicit operator bool() const
	{
		return get() != nullptr;
	}

	T *operator->() const
	{
		return get();
	}

	T& operator*() const
	{
		return get();
	}
};

}
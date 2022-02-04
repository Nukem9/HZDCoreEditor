#pragma once

#include "Util.h"
#include "String.h"
#include "Ref.h"

namespace HRZ
{

class IStreamingManager;
class IStreamingRefCallback;
class RTTIRefObject;

class StreamingRefHandle
{
private:
	enum StreamFlags : uint8_t
	{
		None = 0,
		Loaded = 0x80,
	};

	struct StreamData
	{
		uint32_t m_RefCount;			// 0x0
		String m_CorePath;				// 0x8
		GGUUID m_UUID;					// 0x10
		uint64_t m_Unknown20;			// 0x20
		IStreamingManager *m_Manager;	// 0x28
		Ref<RTTIRefObject> m_Ref;		// 0x30
	};
	assert_size(StreamData, 0x38);

	StreamData *m_Data = nullptr;					// 0x0 Ref counted pointer
	IStreamingRefCallback *m_RefCallback = nullptr;	// 0x8
	void *m_Unknown10 = nullptr;					// 0x10
	StreamFlags m_Flags = None;						// 0x18
	uint8_t m_UnknownFlags = 0;						// 0x19

public:
	StreamingRefHandle() = default;
	StreamingRefHandle(const StreamingRefHandle& Other);
	~StreamingRefHandle();
	StreamingRefHandle& operator=(const StreamingRefHandle&);

	RTTIRefObject *get() const;
};
assert_size(StreamingRefHandle, 0x20);

template<typename T>
class StreamingRef final : public StreamingRefHandle
{
public:
	StreamingRef() = default;
	StreamingRef(const StreamingRef<T>&) = default;
	~StreamingRef() = default;
	StreamingRef<T>& operator=(const StreamingRef<T>&) = default;

	T *get() const
	{
		return static_cast<T *>(StreamingRefHandle::get());
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
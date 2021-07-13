#pragma once

namespace HRZ
{

class GGRTTI;

template<typename T>
class Ref final
{
private:
	T *m_Ptr;

public:
	Ref() = delete;

	Ref(T *Pointer) : m_Ptr(Pointer)
	{
		if (m_Ptr)
			m_Ptr->IncRef();
	}

	Ref(const Ref<T>&) = delete;

	~Ref()
	{
		auto temp = m_Ptr;
		m_Ptr = nullptr;

		if (temp)
			temp->DecRef();
	}

	Ref<T>& operator=(const Ref<T>&) = delete;

	T *get() const
	{
		return m_Ptr;
	}

	explicit operator bool() const
	{
		return get() != nullptr;
	}

	T *operator->() const
	{
		return get();
	}

	static Ref<T> Create()
	{
		auto memory = reinterpret_cast<T *>(CallOffset<0x02ED220, void *(*)(const GGRTTI *)>(T::TypeInfo));

		if (memory)
			T::TypeInfo->AsClass()->m_Constructor(nullptr, memory);

		return Ref<T>(memory);
	}
};

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
		uint32_t m_RefCount;				// 0x0
		String m_CorePath;					// 0x8
		uint64_t m_Hash1;					// 0x10
		uint64_t m_Hash2;					// 0x18
		uint64_t m_Unknown20;				// 0x20
		class IStreamingManager *m_Manager; // 0x28
		Ref<T> m_Ref;						// 0x30
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
};

}
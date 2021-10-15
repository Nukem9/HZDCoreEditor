#pragma once

namespace HRZ
{

class RTTI;

template<typename T>
class Ref final
{
private:
	T *m_Ptr = nullptr;

public:
	Ref()
	{
	}

	Ref(T *Pointer)
	{
		Assign(Pointer);
	}

	Ref(const Ref<T>& Other)
	{
		Assign(Other.m_Ptr);
	}

	~Ref()
	{
		Assign(nullptr);
	}

	Ref<T>& operator=(const Ref<T>& Other)
	{
		Assign(Other.m_Ptr);
		return *this;
	}

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
		auto memory = reinterpret_cast<T *>(CallOffset<0x02ED220, void *(*)(const RTTI *)>(T::TypeInfo));

		if (memory)
			T::TypeInfo->AsClass()->m_Constructor(nullptr, memory);

		return Ref<T>(memory);
	}

private:
	void Assign(T *Pointer)
	{
		auto temp = m_Ptr;

		if (Pointer)
			Pointer->IncRef();

		m_Ptr = Pointer;

		if (temp)
			temp->DecRef();
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
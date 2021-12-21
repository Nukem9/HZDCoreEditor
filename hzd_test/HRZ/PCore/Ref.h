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

}
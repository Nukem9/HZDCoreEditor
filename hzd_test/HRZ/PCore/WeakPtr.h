#pragma once

#include "Util.h"

namespace HRZ
{

class WeakPtrTarget;

template<typename T>
class WeakPtr final
{
private:
	WeakPtrTarget *m_Ptr = nullptr;
	WeakPtr<T> *m_NextRef = nullptr;
	WeakPtr<T> *m_PreviousRef = nullptr;

public:
	WeakPtr(T *Target) : m_Ptr(Target)
	{
		Acquire();
	}

	WeakPtr(const WeakPtr& Other) : m_Ptr(Other.m_Ptr)
	{
		Acquire();
	}

	~WeakPtr()
	{
		Release();
	}

	WeakPtr& operator=(const WeakPtr&) = delete;

	T *get() const
	{
		// TODO: Does the engine use dynamic_cast? I can't tell if it gets optimized away
		return static_cast<T *>(m_Ptr);
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

private:
	void Acquire()
	{
		Offsets::CallID<"WeakPtr::Acquire", void(*)(WeakPtr *)>(this);
	}

	void Release()
	{
		Offsets::CallID<"WeakPtr::Release", void(*)(WeakPtr *)>(this);
	}
};
assert_size(WeakPtr<WeakPtrTarget>, 0x18);

}
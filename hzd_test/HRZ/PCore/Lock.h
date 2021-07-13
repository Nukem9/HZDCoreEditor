#pragma once

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN

#include <windows.h>

#include "Util.h"

namespace HRZ
{

class SharedLock final
{
private:
	SRWLOCK m_Lock;

public:
	SharedLock()
	{
		InitializeSRWLock(&m_Lock);
	}

	SharedLock(const SharedLock&) = delete;

	~SharedLock()
	{
	}

	SharedLock& operator=(const SharedLock&) = delete;

	void lock()
	{
		AcquireSRWLockExclusive(&m_Lock);
	}

	bool try_lock()
	{
		return TryAcquireSRWLockExclusive(&m_Lock);
	}

	void unlock()
	{
		ReleaseSRWLockExclusive(&m_Lock);
	}

	void lock_shared()
	{
		AcquireSRWLockShared(&m_Lock);
	}

	bool try_lock_shared()
	{
		return TryAcquireSRWLockShared(&m_Lock);
	}

	void unlock_shared()
	{
		ReleaseSRWLockShared(&m_Lock);
	}
};
assert_size(SharedLock, 0x8);

class RecursiveLock final
{
private:
	CRITICAL_SECTION m_CriticalSection;

public:
	RecursiveLock()
	{
		InitializeCriticalSection(&m_CriticalSection);
	}

	RecursiveLock(const RecursiveLock&) = delete;

	~RecursiveLock()
	{
		DeleteCriticalSection(&m_CriticalSection);
	}

	RecursiveLock& operator=(const RecursiveLock&) = delete;

	void lock()
	{
		EnterCriticalSection(&m_CriticalSection);
	}

	bool try_lock()
	{
		return TryEnterCriticalSection(&m_CriticalSection);
	}

	void unlock()
	{
		LeaveCriticalSection(&m_CriticalSection);
	}
};
assert_size(RecursiveLock, 0x28);

}
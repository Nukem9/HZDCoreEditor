#pragma once

#include <atomic>

//
// This class is not part of the game code and is meant to map static variables (RTTI *) to dynamic addresses
// embedded in the game executable. Any alternative implementation requires me to manually define 7000 variables
// that point to each RTTI instance.
//
namespace HRZ
{
class RTTI;
}

class RTTILazyResolver
{
private:
	const char *m_Name = nullptr;
	mutable std::atomic<const HRZ::RTTI *> m_Ptr;

public:
	RTTILazyResolver(const char *Name);
	RTTILazyResolver(const RTTILazyResolver&) = delete;
	RTTILazyResolver& operator=(const RTTILazyResolver&) = delete;

	__declspec(noinline) const HRZ::RTTI *get() const;

	const HRZ::RTTI *operator->() const;
	operator const HRZ::RTTI *() const;
	RTTILazyResolver *operator&() = delete;
};
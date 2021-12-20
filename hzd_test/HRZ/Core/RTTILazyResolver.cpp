#include <stdexcept>

#include "../../RTTI/RTTIScanner.h"

#include "RTTILazyResolver.h"

RTTILazyResolver::RTTILazyResolver(const char *Name) : m_Name(Name)
{
}

const HRZ::RTTI *RTTILazyResolver::get() const
{
	if (!m_Ptr)
	{
		m_Ptr.store(RTTIScanner::GetTypeByName(m_Name));

		if (!m_Ptr)
			throw std::logic_error("RTTI type name couldn't be resovled");
	}

	return m_Ptr;
}

RTTILazyResolver::operator const HRZ::RTTI*() const
{
	return get();
}
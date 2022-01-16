#include "../Core/IStreamingManager.h"
#include "StreamingRef.h"

namespace HRZ
{

StreamingRefHandle::StreamingRefHandle(const StreamingRefHandle& Other)
{
	*this = Other;
}

StreamingRefHandle::~StreamingRefHandle()
{
	Offsets::CallID<"StreamingRefHandle::Dtor", void(*)(void *, StreamingRefHandle *)>(nullptr, this);
}

StreamingRefHandle& StreamingRefHandle::operator=(const StreamingRefHandle& Other)
{
	return *Offsets::CallID<"StreamingRefHandle::AssignFromOther", StreamingRefHandle*(*)(StreamingRefHandle *, const StreamingRefHandle&)>(this, Other);
}

RTTIRefObject *StreamingRefHandle::get() const
{
	if (m_Data && (m_Flags & Loaded) != 0)
		return m_Data->m_Ref.get();

	return nullptr;
}

}
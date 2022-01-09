#include "../Core/IStreamingManager.h"
#include "StreamingRef.h"

namespace HRZ
{

StreamingRefHandle::StreamingRefHandle(const StreamingRefHandle& Other)
{
	if (Other.m_Data)
	{
		IStreamingManager::AssetLink link
		{
			.m_Handle = this,
			.m_Path = Other.m_Data->m_CorePath,
			.m_UUID = Other.m_Data->m_UUID,
		};

		Other.m_Data->m_Manager->CreateHandleFromLink(link);
		Other.m_Data->m_Manager->UpdateLoadState(*this, Other.m_Flags);
	}
}

StreamingRefHandle::~StreamingRefHandle()
{
	Offsets::CallID<"StreamingRefHandle::Dtor", void(*)(void *, StreamingRefHandle *)>(nullptr, this);
}

RTTIRefObject *StreamingRefHandle::get() const
{
	if (m_Data && (m_Flags & Loaded) != 0)
		return m_Data->m_Ref.get();

	return nullptr;
}

}
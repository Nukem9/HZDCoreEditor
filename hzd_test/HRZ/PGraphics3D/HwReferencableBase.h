#pragma once

namespace HRZ
{

template<typename T>
class HwReferencableBase
{
public:
	uint32_t m_RefCount = 0;

	void IncRef()
	{
		_InterlockedIncrement(&m_RefCount);
	}

	void DecRef()
	{
		__debugbreak();

		// calls virtual dtor on T
	}
};

}
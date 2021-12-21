#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "RTTIRefObject.h"

namespace HRZ
{

class CoreFileManager
{
public:
	virtual ~CoreFileManager();

	char _pad[0x90];

	void RegisterEventListener(void *Listener)
	{
		Offsets::Call<0x04AA440, void(*)(CoreFileManager *, void *)>(this, Listener);
	}
};
assert_size(CoreFileManager, 0x98);

}
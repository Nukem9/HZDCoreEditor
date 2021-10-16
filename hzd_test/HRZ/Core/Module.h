#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"

namespace HRZ
{

DECL_RTTI(Module);

class Module : public CoreObject
{
public:
	TYPE_RTTI(Module);

	int m_SuspendCount;

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~Module() override;						// 1
	virtual bool Initialize() = 0;					// 16
	virtual void Shutdown() = 0;					// 17
	virtual void ModuleUnknown18() = 0;				// 18
	virtual void ModuleUnknown19() = 0;				// 19
	virtual bool Suspend();							// 20 Increments m_SuspendCount
	virtual bool Resume();							// 21 Decrements m_SuspendCount
};
assert_size(Module, 0x28);

}
#pragma once

#include "../../Offsets.h"
#include "../PCore/Common.h"

#include "RTTIRefObject.h"

namespace HRZ
{

class CoreFileManager
{
public:
	class Events
	{
	public:
		// System root assets loaded by game code
		virtual void OnBeginRootCoreLoad(const String& CorePath) {}

		// Marks the ending of a root asset load
		virtual void OnEndRootCoreLoad(const String& CorePath) {}

		// Called when a core is about to be unloaded but before any objects are unloaded
		virtual void OnBeginCoreUnload(const String& CorePath) {}

		// Called after the core has been destructed
		virtual void OnEndCoreUnload(/* 'const String& CorePath' argument is passed but the pointer is invalid */) {}

		// Objects are valid and all references are resolved
		virtual void OnCoreLoad(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects) {}

		// Objects and references to other objects are finalized at this stage (???)
		virtual void OnLoadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects) {}

		// Called right before the core's objects are destructed
		virtual void OnUnloadCoreObjects(const String& CorePath, const Array<Ref<RTTIRefObject>>& Objects) {}
	};
	assert_size(Events, 0x8);

	char _pad8[0x90];	// 0x8

	virtual ~CoreFileManager();

	void RegisterEventListener(const Events& EventListener)
	{
		Offsets::CallID<"CoreFileManager::RegisterEventListener", void(*)(CoreFileManager *, const Events&)>(this, EventListener);
	}
};
assert_size(CoreFileManager, 0x98);

}
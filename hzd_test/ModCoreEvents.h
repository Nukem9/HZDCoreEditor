#pragma once

#include "HRZ/PCore/Common.h"
#include "HRZ/Core/CoreFileManager.h"

class ModCoreEvents : public HRZ::CoreFileManager::Events
{
public:
	static ModCoreEvents& Instance();

	virtual void OnBeginRootCoreLoad(const HRZ::String& CorePath) override;
	virtual void OnEndRootCoreLoad(const HRZ::String& CorePath) override;
	virtual void OnBeginCoreUnload(const HRZ::String& CorePath) override;
	virtual void OnEndCoreUnload() override;
	virtual void OnCoreLoad(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;
	virtual void OnLoadCoreObjects(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;
	virtual void OnUnloadCoreObjects(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;
};
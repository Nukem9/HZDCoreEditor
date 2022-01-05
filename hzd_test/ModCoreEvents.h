#pragma once

#include <variant>
#include <unordered_map>

#include "HRZ/PCore/Common.h"
#include "HRZ/Core/CoreFileManager.h"
#include "HRZ/Core/RTTIObjectTweaker.h"

class ModCoreEvents : public HRZ::CoreFileManager::Events
{
private:
	struct RTTIValuePatch
	{
		HRZ::String m_Path;
		HRZ::String m_Value;
	};

	class ValuePatchVisitor : public HRZ::RTTIObjectTweaker::SetValueVisitor
	{
	private:
		const RTTIValuePatch& m_Patch;

	public:
		ValuePatchVisitor(const RTTIValuePatch& Patch);

		virtual void SetValue(void *Object, const HRZ::RTTI *Type) override;
		virtual int GetFlags() override;
	};

	std::unordered_map<const HRZ::RTTI *, std::vector<RTTIValuePatch>> m_RTTIPatchesByType;
	std::unordered_map<HRZ::GGUUID, std::vector<RTTIValuePatch>> m_RTTIPatchesByUUID;

public:
	ModCoreEvents();

	virtual void OnBeginRootCoreLoad(const HRZ::String& CorePath) override;
	virtual void OnEndRootCoreLoad(const HRZ::String& CorePath) override;
	virtual void OnBeginCoreUnload(const HRZ::String& CorePath) override;
	virtual void OnEndCoreUnload() override;
	virtual void OnCoreLoad(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;
	virtual void OnLoadCoreObjects(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;
	virtual void OnUnloadCoreObjects(const HRZ::String& CorePath, const HRZ::Array<HRZ::Ref<HRZ::RTTIRefObject>>& Objects) override;

	static ModCoreEvents& Instance();
};
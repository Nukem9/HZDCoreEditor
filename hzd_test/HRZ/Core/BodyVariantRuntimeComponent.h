#pragma once

#include "../../Offsets.h"

#include "../PCore/Common.h"

#include "EntityComponent.h"
#include "StreamingManager.h"

namespace HRZ
{

class HumanoidBodyVariant;

DECL_RTTI(BodyVariantRuntimeComponent);

class BodyVariantRuntimeComponent : public EntityComponent, public IStreamingRefCallback
{
public:
	TYPE_RTTI(BodyVariantRuntimeComponent);

	StreamingRef<HumanoidBodyVariant> m_BodyVariant;			// 0x60
	StreamingRef<HumanoidBodyVariant> m_PendingLoadBodyVariant;	// 0x80
	char _padA0[0x70];
	uint64_t m_LastVariantSwitchTime;							// 0x110

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~BodyVariantRuntimeComponent() override;			// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4

	virtual void OnStreamingRefLoad(RTTIRefObject *Object) override;	// 1
	virtual void OnStreamingRefUnload() override;						// 2

	void SetVariantByUUID(const GGUUID& UUID)
	{
		Offsets::CallID<"BodyVariantRuntimeComponent::SetVariantByUUID", void(*)(BodyVariantRuntimeComponent *, const GGUUID&)>(this, UUID);
	}

	void ForceSetUnlistedVariantByPath(const String CorePath, const GGUUID& UUID)
	{
		StreamingRef<HumanoidBodyVariant> newHandle;
		IStreamingManager::AssetLink link
		{
			.m_Handle = &newHandle,
			.m_Path = CorePath,
			.m_UUID = UUID,
		};

		// Don't call CreateHandleFromLink directly on m_PendingLoadBodyVariant. It won't unload the previous asset.
		StreamingManager::Instance()->CreateHandleFromLink(link);

		// Now set the loader callback
		m_PendingLoadBodyVariant = newHandle;
		StreamingManager::Instance()->IStreamingManagerUnknown05(m_PendingLoadBodyVariant, 1, this, nullptr);
		StreamingManager::Instance()->UpdateLoadState(m_PendingLoadBodyVariant, 7);
		m_LastVariantSwitchTime = __rdtsc();
	}
};
assert_offset(BodyVariantRuntimeComponent, m_BodyVariant, 0x60);
assert_offset(BodyVariantRuntimeComponent, m_PendingLoadBodyVariant, 0x80);
assert_offset(BodyVariantRuntimeComponent, m_LastVariantSwitchTime, 0x110);
assert_size(BodyVariantRuntimeComponent, 0x118);

}
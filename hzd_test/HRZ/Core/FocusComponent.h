#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"

namespace HRZ
{

class BooleanFact;
class EntityAction;
class EntityResource;
class FocusOutlineColorSettings;
class HUDCrosshairSettings;
class HUDLogicGroupResource;
class ParticleSystemResource;
class SoundResource;

DECL_RTTI(FocusComponent);
DECL_RTTI(FocusComponentResource);

class FocusComponentResource : public EntityComponentResource
{
public:
	Ref<EntityResource> m_OutlineEntityResource;
	float m_OutlineEntityRemoveDelay;
	Ref<FocusOutlineColorSettings> m_OutlineColorSettings;
	Ref<EntityAction> m_Activate;
	Ref<EntityAction> m_Deactivate;
	Ref<BooleanFact> m_ActiveFact;
	Ref<SoundResource> m_TaggedSoundEffect;
	Ref<SoundResource> m_UnTaggedSoundEffect;
	float m_LookInputTreshHold;
	float m_MaxActivationDelay;
	String m_AnimAction;
	Ref<HUDCrosshairSettings> m_Crosshair;
	float m_FocusTargetAngle;
	float m_FocusPatternSpeed;
	int m_FocusPatternRings;
	float m_FocusPatternTime;
	float m_FocusAimDistanceWeight;
	float m_FocusAimAngleWeight;
	float m_ScanningDelay;
	float m_TaggedPatrolPathRange;
	float m_FocusModeTagRange;
	float m_FocusModeTagFadeDistance;
	float m_TrackingRange;
	Ref<BooleanFact> m_IsTracksVisibleFact;
	Ref<BooleanFact> m_IsFocusAllowedInSequencesFact;
	Ref<SoundResource> m_SignalAlertSound;
	String m_SignalAlertSoundHelperName;
	float m_SignalScanningRadius;
	float m_SignalScanningDuration;
	Ref<BooleanFact> m_SignalScannedFact;
	Ref<ParticleSystemResource> m_SignalParticleSystemResource;
	Ref<ParticleSystemResource> m_SignalScannedParticleSystemResource;
	Ref<HUDLogicGroupResource> m_WorldIconHUDLogicGroup;
	Ref<BooleanFact> m_WorldIconsHiddenFact;
	float m_WorldIconsVisibleTimer;

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~FocusComponentResource() override;					// 1
	virtual const RTTI *GetComponentRTTI() const override;		// 18
	virtual const RTTI *GetComponentRepRTTI() const override;	// 19
};
assert_size(FocusComponentResource, 0x108);

class FocusComponent : public EntityComponent
{
public:
	TYPE_RTTI(FocusComponent);

	char _pad58[0x2B8];

	//Ref<Entity> m_DesktopSphereEntity;	// 0xB0

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~FocusComponent() override;							// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(FocusComponent, 0x310);

}
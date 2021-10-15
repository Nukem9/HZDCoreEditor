#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class SlowMotionManager
{
public:
	enum class TimescaleModifierId : uint32_t
	{
		Invalid = 0xFFFFFFFF,
	};

	struct TimescaleEntry
	{
		char _pad[0x1C];
	};

	uint32_t m_UnknownCounter;
	Array<TimescaleEntry> m_TimescaleModifiers;
	float m_CurrentTimescale;
	float m_CurrentCameraTimescale;

	TimescaleModifierId AddTimescaleModifier(float Timescale, float CameraTimescale, float BlendInTime)
	{
		return CallID<"SlowMotionManager::AddTimescaleModifier", TimescaleModifierId(*)(SlowMotionManager *, float, float, float)>(this, Timescale, CameraTimescale, BlendInTime);
	}

	void RemoveTimescaleModifier(TimescaleModifierId& Id, float BlendOutTime)
	{
		return CallID<"SlowMotionManager::RemoveTimescaleModifier", void(*)(SlowMotionManager *, TimescaleModifierId&, float)>(this, Id, BlendOutTime);
	}
};
assert_offset(SlowMotionManager, m_TimescaleModifiers, 0x8);
assert_offset(SlowMotionManager, m_CurrentTimescale, 0x18);
assert_offset(SlowMotionManager, m_CurrentCameraTimescale, 0x1C);

}
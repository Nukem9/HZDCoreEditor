#pragma once

#include "../../Offsets.h"

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
};
assert_offset(SlowMotionManager, m_TimescaleModifiers, 0x8);
assert_offset(SlowMotionManager, m_CurrentTimescale, 0x18);
assert_offset(SlowMotionManager, m_CurrentCameraTimescale, 0x1C);

}
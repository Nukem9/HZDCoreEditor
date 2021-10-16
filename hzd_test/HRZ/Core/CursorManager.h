#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class CursorManager
{
public:
	virtual ~CursorManager(); // 0

	bool m_IsShowingCursor;			// 0x8
	bool m_ForceHideCursor;			// 0x9
	char _padA[0x1];
	bool m_UnlockCursorBounds;		// 0xB
};
assert_offset(CursorManager, m_IsShowingCursor, 0x8);
assert_offset(CursorManager, m_UnlockCursorBounds, 0xB);

}
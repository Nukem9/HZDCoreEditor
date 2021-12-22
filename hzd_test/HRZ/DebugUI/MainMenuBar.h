#pragma once

#include <imgui.h>

#include "../Core/WorldPosition.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class MainMenuBar : public Window
{
public:
	enum class FreeCamMode
	{
		Off,
		Free,
		Noclip,
	};

	static inline FreeCamMode m_FreeCamMode;
	static inline WorldPosition m_FreeCamPosition;

	static inline bool m_TimescaleOverride;
	static inline bool m_TimescaleOverrideInMenus;
	static inline float m_Timescale = 1.0f;

	virtual void Render() override;
	virtual bool Close() override;

	void DrawWorldMenu();
	void DrawTimeMenu();
	void DrawCheatsMenu();
	void DrawSavesMenu();
	void DrawDebugMenu();
	void UpdateFreecam();
};

}
#pragma once

#include <string>
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

	enum class SaveType
	{
		Quick,
		Auto,
		Manual,
		NewGamePlus,
	};

	static inline bool m_IsVisible;
	static inline FreeCamMode m_FreeCamMode;
	static inline WorldPosition m_FreeCamPosition;
	static inline bool m_PauseAIProcessing;
	static inline bool m_TimescaleOverride;
	static inline bool m_TimescaleOverrideInMenus;
	static inline float m_Timescale = 1.0f;

	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

	static void ToggleVisibility();
	static void TogglePauseGameLogic();
	static void TogglePauseTimeOfDay();
	static void ToggleFreeflyCamera();
	static void ToggleNoclip();
	static void SavePlayerGame(SaveType Type);
	static void LoadPreviousSave();
	static void AdjustTimescale(float Adjustment);
	static void AdjustTimeOfDay(float Adjustment);

private:
	void DrawWorldMenu();
	void DrawTimeMenu();
	void DrawCheatsMenu();
	void DrawSavesMenu();
	void DrawDebugMenu();
	void DrawMiscellaneousMenu();
};

}
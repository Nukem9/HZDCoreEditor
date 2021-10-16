#pragma once

#include <imgui.h>

#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class DemoWindow : public Window
{
private:
	bool m_WindowOpen = true;

public:
	virtual void Render() override
	{
		ImGui::ShowDemoWindow(&m_WindowOpen);
	}

	virtual bool Close() override
	{
		return !m_WindowOpen;
	}
};

}
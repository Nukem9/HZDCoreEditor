#pragma once

#include <atomic>
#include <imgui.h>

#include "DebugUIWindow.h"

namespace HRZ
{
}

namespace HRZ::DebugUI
{

class BodyVariantWindow : public Window
{
private:
	bool m_WindowOpen = true;
	ImGuiTextFilter m_VariantNameFilter;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

private:
};

}
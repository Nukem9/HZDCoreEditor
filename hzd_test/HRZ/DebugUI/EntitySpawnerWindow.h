#pragma once

#include "DebugUIWindow.h"

namespace HRZ
{
}

namespace HRZ::DebugUI
{

class EntitySpawnerWindow : public Window
{
private:
	bool m_WindowOpen = true;
	ImGuiTextFilter m_SpawnerNameFilter;

public:
	virtual void Render() override;
	virtual bool Close() override;

private:
};

}
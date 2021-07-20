#pragma once

#include <imgui.h>

#include "../Core/Entity.h"
#include "../Core//WorldPosition.h"

#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class EntityWindow : public Window
{
private:
	Entity *m_Entity = nullptr;
	bool m_WindowOpen = true;
	ImGuiTextFilter m_ComponentListFilter;
	WorldPosition m_SavedWorldPosition;

public:
	EntityWindow(Entity *TargetEntity);

	virtual void Render() override;
	virtual bool Close() override;
};

}
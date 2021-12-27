#pragma once

#include <string>
#include <imgui.h>

#include "../Core/Entity.h"
#include "../Core//WorldPosition.h"

#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class EntityWindow : public Window
{
private:
	WeakPtr<Entity> m_Entity;
	bool m_WindowOpen = true;
	ImGuiTextFilter m_ComponentListFilter;
	WorldPosition m_SavedWorldPosition;

public:
	EntityWindow(WeakPtr<Entity> TargetEntity);

	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;
};

}
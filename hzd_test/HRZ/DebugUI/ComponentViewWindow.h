#pragma once

#include "../Core/EntityComponent.h"

#include "DebugUIWindow.h"

namespace HRZ
{

class EntityComponent;
class ItemDescriptionComponent;
class InventoryItemComponent;
class HumanoidInventory;

}

namespace HRZ::DebugUI
{

class ComponentViewWindow : public Window
{
private:
	EntityComponent *m_Component = nullptr;
	bool m_WindowOpen = true;
	ImGuiTextFilter m_ScratchFilter;

public:
	ComponentViewWindow(EntityComponent *Component);

	virtual void Render() override;
	virtual bool Close() override;

	void DrawComponent(EntityComponent *Component);
	void DrawComponent(ItemDescriptionComponent *Component);
	void DrawComponent(InventoryItemComponent *Component);
	void DrawComponent(HumanoidInventory *Component);
};

}
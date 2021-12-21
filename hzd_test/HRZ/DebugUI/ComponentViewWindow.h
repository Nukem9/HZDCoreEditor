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
	WeakPtr<EntityComponent> m_Component;
	bool m_WindowOpen = true;
	ImGuiTextFilter m_ScratchFilter;

public:
	ComponentViewWindow(WeakPtr<EntityComponent> Component);

	virtual void Render() override;
	virtual bool Close() override;

	void DrawComponent(EntityComponent *Component);
	void DrawComponent(ItemDescriptionComponent *Component);
	void DrawComponent(InventoryItemComponent *Component);
	void DrawComponent(HumanoidInventory *Component);
};

}
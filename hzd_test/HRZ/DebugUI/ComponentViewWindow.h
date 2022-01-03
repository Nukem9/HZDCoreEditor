#pragma once

#include <string>

#include "../PCore/Common.h"
#include "../Core/EntityComponent.h"

#include "DebugUIWindow.h"

namespace HRZ
{

class EntityComponent;
class EquipmentModificationComponent;
class EquipmentModificationItemComponent;
class InventoryItemComponent;
class Inventory;
class ItemDescriptionComponent;
class StackableComponent;

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
	virtual std::string GetId() const override;

private:
	void DrawComponent(EntityComponent *Component);
	void DrawComponent(EquipmentModificationComponent *Component);
	void DrawComponent(EquipmentModificationItemComponent *Component);
	void DrawComponent(Inventory *Component);
	void DrawComponent(InventoryItemComponent *Component);
	void DrawComponent(ItemDescriptionComponent *Component);
	void DrawComponent(StackableComponent *Component);

	template<typename T>
	void DrawEntityList(Array<T>& Items, const ImGuiTextFilter *Filter);
	String GetLocalizedResourceName(Resource *ResourcePtr);
};

}
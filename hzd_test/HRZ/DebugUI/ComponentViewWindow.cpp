#include <format>
#include <imgui.h>

#include "../Core/HumanoidInventory.h"
#include "../Core/ItemDescriptionComponent.h"
#include "../Core/InventoryItemComponent.h"
#include "../Core/LocalizedTextResource.h"
#include "../Core/EquipmentModificationComponent.h"
#include "../Core/EquipmentModificationItemComponent.h"
#include "../Core/StackableComponent.h"

#include "DebugUI.h"
#include "EntityWindow.h"
#include "ComponentViewWindow.h"

namespace HRZ::DebugUI
{

ComponentViewWindow::ComponentViewWindow(WeakPtr<EntityComponent> Component) : m_Component(Component)
{
}

void ComponentViewWindow::Render()
{
	if (!m_Component)
		return;

	ImGui::SetNextWindowSize(ImVec2(500.0f, 500.0f), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen, ImGuiWindowFlags_NoSavedSettings))
	{
		ImGui::End();
		return;
	}

	auto component = m_Component.get();

	if (m_Component->GetRTTI()->IsKindOf(Inventory::TypeInfo))
		DrawComponent(static_cast<Inventory *>(component));
	else if (m_Component->GetRTTI()->IsKindOf(InventoryItemComponent::TypeInfo))
		DrawComponent(static_cast<InventoryItemComponent *>(component));
	else if (m_Component->GetRTTI()->IsKindOf(ItemDescriptionComponent::TypeInfo))
		DrawComponent(static_cast<ItemDescriptionComponent *>(component));
	else if (m_Component->GetRTTI()->IsKindOf(EquipmentModificationComponent::TypeInfo))
		DrawComponent(static_cast<EquipmentModificationComponent *>(component));
	else if (m_Component->GetRTTI()->IsKindOf(EquipmentModificationItemComponent::TypeInfo))
		DrawComponent(static_cast<EquipmentModificationItemComponent *>(component));
	else if (m_Component->GetRTTI()->IsKindOf(StackableComponent::TypeInfo))
		DrawComponent(static_cast<StackableComponent *>(component));

	ImGui::End();
}

bool ComponentViewWindow::Close()
{
	return !m_WindowOpen || !m_Component;
}

std::string ComponentViewWindow::GetId() const
{
	auto c = m_Component.get();

	return std::format("Component \"{0:}\" for Entity \"{1:}\" ({2:016X})", c->GetUnderlyingName().c_str(), c->m_Entity->GetName().c_str(), reinterpret_cast<uintptr_t>(c));
}

void DrawHeader(const char *Label, bool Separator = true)
{
	if (Separator)
		ImGui::Separator();

	ImGui::SetCursorPosX(ImGui::GetWindowContentRegionMax().x - ImGui::CalcTextSize(Label).x);
	ImGui::TextDisabled(Label);
}

void ComponentViewWindow::DrawComponent(EquipmentModificationComponent *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));
	DrawHeader("EquipmentModificationComponent");

	// Draw filter
	m_ScratchFilter.Draw();

	// Draw item table
	DrawEntityList(Component->m_ModificationItems, &m_ScratchFilter);
}

void ComponentViewWindow::DrawComponent(EquipmentModificationItemComponent *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));
	DrawHeader("EquipmentModificationItemComponent");

	// Draw stat ratings
	for (auto& statMod : Component->m_Modifications)
	{
		auto& rating = statMod.GetMemberRefUnsafe<int>("Rating");
		auto& type = statMod.GetMemberRefUnsafe<Ref<Resource>>("Type");

		ImGui::PushID(&statMod);
		ImGui::Text("Type: %s", type ? type->GetName().c_str() : "");
		ImGui::InputInt("Rating", &rating);
		ImGui::PopID();
	}
}

void ComponentViewWindow::DrawComponent(EntityComponent *Component)
{
	DrawHeader("EntityComponent", false);
}

void ComponentViewWindow::DrawComponent(Inventory *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));
	DrawHeader("Inventory");

	// Draw filter
	m_ScratchFilter.Draw();

	// Draw item list
	DrawEntityList(Component->m_Items, &m_ScratchFilter);
}

void ComponentViewWindow::DrawComponent(InventoryItemComponent *Component)
{
	DrawComponent(static_cast<ItemDescriptionComponent *>(Component));
	DrawHeader("InventoryItemComponent");

	if (Component->m_Resource)
	{
		auto resource = static_cast<InventoryItemComponentResource *>(Component->m_Resource.get());
	}
}

void ComponentViewWindow::DrawComponent(ItemDescriptionComponent *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));
	DrawHeader("ItemDescriptionComponent");

	if (Component->m_Resource)
	{
		auto resource = static_cast<ItemDescriptionComponentResource *>(Component->m_Resource.get());

		ImGui::TextWrapped("Item Name: %s", resource->m_LocalizedItemName ? resource->m_LocalizedItemName->GetTranslation().EncodeUTF8().c_str() : "<None>");
		ImGui::TextWrapped("Item Description: %s", resource->m_LocalizedItemDescription ? resource->m_LocalizedItemDescription->GetTranslation().EncodeUTF8().c_str() : "<None>");
		ImGui::Text("Price Info: %p", resource->m_PriceInfo.get());
		ImGui::Text("Item Weight: %d", resource->m_ItemWeight);
		ImGui::Text("Icon Texture: %p", resource->m_UIIconTexture.get());
		ImGui::Text("Icon Texture Inactive: %p", resource->m_UIIconInactiveTexture.get());
		ImGui::Text("Loot Description Resource: %p", resource->m_LootItemDescriptionResource.get());
		ImGui::Text("Stats Display Resource: %p", resource->m_StatsDisplayResource.get());
		ImGui::Text("Movie: %p", resource->m_Movie.get());
	}
}

void ComponentViewWindow::DrawComponent(StackableComponent *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));
	DrawHeader("StackableComponent");

	ImGui::InputInt("Amount", &Component->m_Amount);
}

template<typename T>
void ComponentViewWindow::DrawEntityList(Array<T>& Items, const ImGuiTextFilter *Filter)
{
	constexpr ImGuiTableFlags flags =
		ImGuiTableFlags_Resizable | ImGuiTableFlags_RowBg | ImGuiTableFlags_Borders | ImGuiTableFlags_NoBordersInBody
		| ImGuiTableFlags_ScrollX | ImGuiTableFlags_ScrollY | ImGuiTableFlags_SizingFixedFit;

	constexpr std::array columns
	{
		"Item",
		"Type",
		"Name",
		"Pointer"
	};

	if (ImGui::BeginTable("entity_list_table", static_cast<int>(columns.size()), flags))
	{
		for (auto col : columns)
			ImGui::TableSetupColumn(col);
		ImGui::TableSetupScrollFreeze(1, 1);
		ImGui::TableHeadersRow();

		for (auto& item : Items)
		{
			if (!item)
				continue;

			auto showContextMenu = [&]()
			{
				ImGui::PushID((ImGui::TableGetRowIndex() * ImGui::TableGetColumnCount()) + ImGui::TableGetColumnIndex());

				if (ImGui::BeginPopupContextItem("entity_list_row_popup"))
				{
					if (ImGui::Selectable("View Entity"))
					{
						AddWindow(std::make_shared<EntityWindow>(item.get()));
						ImGui::CloseCurrentPopup();
					}

					ImGui::EndPopup();
				}

				if (ImGui::IsItemHovered())
					ImGui::SetTooltip("Right click for options");

				ImGui::PopID();
			};

			auto& itemName = item->GetName();
			auto rttiSymName = item->GetRTTI()->GetSymbolName();

			if (Filter)
			{
				if (!Filter->PassFilter(itemName.c_str()) && !Filter->PassFilter(rttiSymName.c_str()))
					continue;
			}

			ImGui::PushID(item.get());
			ImGui::TableNextRow();

			ImGui::TableSetColumnIndex(0);
			ImGui::Text("%s", itemName.c_str());
			showContextMenu();

			ImGui::TableSetColumnIndex(1);
			ImGui::Text("%s", rttiSymName.c_str());
			showContextMenu();

			ImGui::TableSetColumnIndex(2);
			auto desc = item->m_Components.FindComponent<ItemDescriptionComponent>();
			if (desc)
				ImGui::Text("%s", GetLocalizedResourceName(desc->m_Resource).c_str());
			else
				ImGui::Text("");
			showContextMenu();

			ImGui::TableSetColumnIndex(3);
			ImGui::Text("%p", item.get());
			showContextMenu();

			ImGui::PopID();
		}

		ImGui::EndTable();
	}
}

String ComponentViewWindow::GetLocalizedResourceName(Resource *ResourcePtr)
{
	if (ResourcePtr)
	{
		auto rtti = ResourcePtr->GetRTTI()->AsClass();

		if (rtti->IsKindOf(ItemDescriptionComponentResource::TypeInfo))
			return ResourcePtr->GetMemberRefUnsafe<Ref<LocalizedTextResource>>("LocalizedItemName")->GetTranslation().EncodeUTF8();
	}

	return "";
}

}
#include <imgui.h>

#include "../PCore/Common.h"
#include "../Core/HumanoidInventory.h"

#include "DebugUI.h"
#include "EntityWindow.h"
#include "ComponentViewWindow.h"

#include "../Core/ItemDescriptionComponent.h"
#include "../Core/InventoryItemComponent.h"
#include "../Core/LocalizedTextResource.h"

namespace HRZ::DebugUI
{

ComponentViewWindow::ComponentViewWindow(EntityComponent *Component) : m_Component(Component)
{
}

void ComponentViewWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500.0f, 500.0f), ImGuiCond_FirstUseEver);

	char windowName[512];
	sprintf_s(windowName, "Component \"%s\" for Entity \"%s\" (%p)", m_Component->GetUnderlyingName().c_str(), m_Component->m_Entity->GetName().c_str(), m_Component);

	if (!ImGui::Begin(windowName, &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	if (m_Component->GetRTTI()->IsKindOf(HumanoidInventory::TypeInfo))
		DrawComponent(static_cast<HumanoidInventory *>(m_Component));
	else if (m_Component->GetRTTI()->IsKindOf(InventoryItemComponent::TypeInfo))
		DrawComponent(static_cast<InventoryItemComponent *>(m_Component));
	else if (m_Component->GetRTTI()->IsKindOf(ItemDescriptionComponent::TypeInfo))
		DrawComponent(static_cast<ItemDescriptionComponent *>(m_Component));

	ImGui::End();
}

bool ComponentViewWindow::Close()
{
	return !m_WindowOpen;
}

void ComponentViewWindow::DrawComponent(EntityComponent *Component)
{
	ImGui::Text("EntityComponent");
	ImGui::Separator();
}

void ComponentViewWindow::DrawComponent(ItemDescriptionComponent *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));

	if (!Component->m_Resource)
		return;

	auto resource = static_cast<ItemDescriptionComponentResource *>(Component->m_Resource.get());

	ImGui::Text("ItemDescriptionComponent");
	ImGui::TextWrapped("Item Name: %s", resource->m_LocalizedItemName ? resource->m_LocalizedItemName->GetTranslation().EncodeUTF8().c_str() : "<None>");
	ImGui::TextWrapped("Item Description: %s", resource->m_LocalizedItemDescription ? resource->m_LocalizedItemDescription->GetTranslation().EncodeUTF8().c_str() : "<None>");
	ImGui::Text("Price Info: %p", resource->m_PriceInfo.get());
	ImGui::Text("Item Weight: %d", resource->m_ItemWeight);
	ImGui::Text("Icon Texture: %p", resource->m_UIIconTexture.get());
	ImGui::Text("Icon Texture Inactive: %p", resource->m_UIIconInactiveTexture.get());
	ImGui::Text("Loot Description Resource: %p", resource->m_LootItemDescriptionResource.get());
	ImGui::Text("Stats Display Resource: %p", resource->m_StatsDisplayResource.get());
	ImGui::Text("Movie: %p", resource->m_Movie.get());
	ImGui::Separator();
}

void ComponentViewWindow::DrawComponent(InventoryItemComponent *Component)
{
	DrawComponent(static_cast<ItemDescriptionComponent *>(Component));

	if (!Component->m_Resource)
		return;

	auto resource = static_cast<InventoryItemComponentResource *>(Component->m_Resource.get());

	ImGui::Text("InventoryItemComponent");
	ImGui::Separator();
}

void ComponentViewWindow::DrawComponent(HumanoidInventory *Component)
{
	DrawComponent(static_cast<EntityComponent *>(Component));

	// Draw filter
	m_ScratchFilter.Draw();

	// Draw item list
	constexpr ImGuiTableFlags flags =
		ImGuiTableFlags_Resizable | ImGuiTableFlags_RowBg | ImGuiTableFlags_Borders | ImGuiTableFlags_NoBordersInBody
		| ImGuiTableFlags_ScrollX | ImGuiTableFlags_ScrollY | ImGuiTableFlags_SizingFixedFit;

	if (ImGui::BeginTable("inventory_item_list", 4, flags))
	{
		ImGui::TableSetupColumn("Item");
		ImGui::TableSetupColumn("Type");
		ImGui::TableSetupColumn("Name");
		ImGui::TableSetupColumn("Pointer");
		ImGui::TableSetupScrollFreeze(1, 1);
		ImGui::TableHeadersRow();

		for (auto& item : Component->m_Items)
		{
			auto showContextMenu = [&]()
			{
				ImGui::PushID((ImGui::TableGetRowIndex() * ImGui::TableGetColumnCount()) + ImGui::TableGetColumnIndex());

				if (ImGui::BeginPopupContextItem("InventoryListRowPopup"))
				{
					if (ImGui::Selectable("View Entity"))
					{
						AddWindow(std::make_unique<EntityWindow>(item));
						ImGui::CloseCurrentPopup();
					}

					ImGui::EndPopup();
				}

				ImGui::PopID();
			};

			auto& itemName = item->GetName();
			auto rttiSymName = item->GetRTTI()->GetSymbolName();

			if (!m_ScratchFilter.PassFilter(itemName.c_str()) && !m_ScratchFilter.PassFilter(rttiSymName.c_str()))
				continue;

			ImGui::PushID(item);
			ImGui::TableNextRow();

			ImGui::TableSetColumnIndex(0);
			ImGui::Text("%s", itemName.c_str());
			showContextMenu();

			ImGui::TableSetColumnIndex(1);
			ImGui::Text("%s", rttiSymName.c_str());
			showContextMenu();

			ImGui::TableSetColumnIndex(2);
			auto desc = item->m_Components.FindComponent<ItemDescriptionComponent>();
			auto descResource = static_cast<ItemDescriptionComponentResource *>(desc->m_Resource.get());
			if (descResource)
				ImGui::Text("%s", descResource->m_LocalizedItemName->GetTranslation().EncodeUTF8().c_str());
			else
				ImGui::Text("");
			showContextMenu();

			ImGui::TableSetColumnIndex(3);
			ImGui::Text("%p", item);
			showContextMenu();

			ImGui::PopID();
		}

		ImGui::EndTable();
	}
}

}
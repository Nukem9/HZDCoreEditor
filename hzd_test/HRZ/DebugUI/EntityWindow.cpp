#include <unordered_set>
#include <imgui.h>

#include "../Core/Mover.h"

#include "DebugUI.h"
#include "ComponentViewWindow.h"
#include "EntityWindow.h"

extern HRZ::SharedLock ResourceListLock;
extern std::unordered_set<HRZ::RTTIRefObject *> CachedAIFactions;

namespace HRZ
{
DECL_RTTI(AIFaction);
}

namespace HRZ::DebugUI
{

EntityWindow::EntityWindow(WeakPtr<Entity> TargetEntity) : m_Entity(TargetEntity)
{
}

void EntityWindow::Render()
{
	if (!m_Entity)
		return;

	ImGui::SetNextWindowSize(ImVec2(850.0f, 760.0f), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen, ImGuiWindowFlags_NoSavedSettings))
	{
		ImGui::End();
		return;
	}

	ImGui::Text("Entity name: %s", m_Entity->GetName().c_str());
	ImGui::Text("Resource type: %s (%p)", m_Entity->m_Resource ? m_Entity->m_Resource->GetName().c_str() : "", m_Entity->m_Resource.get());
	ImGui::Separator();

	ImGui::PushItemWidth(200);
	{
		// World position
		m_Entity->m_DataLock.lock();
		auto transform = m_Entity->m_Orientation;
		auto velocity = m_Entity->m_Mover ? m_Entity->m_Mover->GetVelocity() : Vec3{};
		m_Entity->m_DataLock.unlock();

		bool valueChanged = false;
		valueChanged |= ImGui::InputDouble("Position X", &transform.Position.X, 1.0, 20.0, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		valueChanged |= ImGui::InputDouble("Position Y", &transform.Position.Y, 1.0, 20.0, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		valueChanged |= ImGui::InputDouble("Position Z", &transform.Position.Z, 1.0, 20.0, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);

		// Save/load position
		ImGui::Spacing();

		if (ImGui::Button("Save Position"))
			m_SavedWorldPosition = transform.Position;

		ImGui::SameLine();

		if (ImGui::Button("Load Position"))
		{
			valueChanged = true;
			transform.Position = m_SavedWorldPosition;
		}

		ImGui::Spacing();

		if (valueChanged)
			m_Entity->PlaceOnWorldTransform(transform, false);

		// Velocity
		valueChanged = false;
		valueChanged |= ImGui::InputFloat("Velocity X", &velocity.X, 1.0f, 20.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		valueChanged |= ImGui::InputFloat("Velocity Y", &velocity.Y, 1.0f, 20.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		valueChanged |= ImGui::InputFloat("Velocity Z", &velocity.Z, 1.0f, 20.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);

		if (valueChanged && m_Entity->m_Mover)
			m_Entity->m_Mover->SetVelocity(velocity);

		ImGui::Spacing();

		// Faction
		std::scoped_lock lock(ResourceListLock);
		std::vector<Resource *> sortedFactions;

		for (auto refObject : CachedAIFactions)
			sortedFactions.push_back(RTTI::Cast<Resource>(refObject));

		std::sort(sortedFactions.begin(), sortedFactions.end(), [](Resource *A, Resource *B)
		{
			return A->GetName() < B->GetName();
		});

		auto itr = std::find(sortedFactions.begin(), sortedFactions.end(), (Resource *)m_Entity->m_Faction);
		int currentIndex = std::distance(sortedFactions.begin(), itr);

		auto fetchItem = +[](void *Data, int Index, const char **Text)
		{
			*Text = reinterpret_cast<std::vector<Resource *> *>(Data)->at(Index)->GetName().c_str();
			return true;
		};

		if (ImGui::Combo("Faction##entityfactioncombo", &currentIndex, fetchItem, &sortedFactions, sortedFactions.size()))
			m_Entity->SetFaction((AIFaction *)sortedFactions[currentIndex]);
	}
	ImGui::PopItemWidth();

	// Component list
	ImGui::Separator();

	m_ComponentListFilter.Draw();

	ImGui::SameLine(ImGui::GetWindowContentRegionMax().x - ImGui::CalcTextSize("Component List").x);
	ImGui::TextDisabled("Component List");

	constexpr ImGuiTableFlags flags =
		ImGuiTableFlags_Resizable | ImGuiTableFlags_RowBg | ImGuiTableFlags_Borders | ImGuiTableFlags_NoBordersInBody
		| ImGuiTableFlags_ScrollX | ImGuiTableFlags_ScrollY | ImGuiTableFlags_SizingFixedFit;

	if (ImGui::BeginTable("entity_component_list", 3, flags))
	{
		ImGui::TableSetupColumn("Component");
		ImGui::TableSetupColumn("Resource Name");
		ImGui::TableSetupColumn("Pointer");
		ImGui::TableSetupScrollFreeze(1, 1);
		ImGui::TableHeadersRow();

		for (auto& component : m_Entity->m_Components.m_Components)
		{
			auto showContextMenu = [&]()
			{
				ImGui::PushID((ImGui::TableGetRowIndex() * ImGui::TableGetColumnCount()) + ImGui::TableGetColumnIndex());

				if (ImGui::BeginPopupContextItem("ECListRowPopup"))
				{
					if (ImGui::Selectable("View Component"))
					{
						AddWindow(std::make_shared<ComponentViewWindow>(component));
						ImGui::CloseCurrentPopup();
					}

					if (ImGui::Selectable("Remove Component"))
					{
						m_Entity->RemoveComponent(component);
						ImGui::CloseCurrentPopup();
					}

					ImGui::EndPopup();
				}

				if (ImGui::IsItemHovered())
					ImGui::SetTooltip("Right click for options");

				ImGui::PopID();
			};

			auto rttiSymName = component->GetRTTI()->GetSymbolName();
			auto resourceName = component->m_Resource ? component->m_Resource->GetName().c_str() : "";

			if (!m_ComponentListFilter.PassFilter(rttiSymName.c_str()) && !m_ComponentListFilter.PassFilter(resourceName))
				continue;

			ImGui::PushID(component);
			ImGui::TableNextRow();

			ImGui::TableSetColumnIndex(0);
			ImGui::Text("%s", rttiSymName.c_str());
			showContextMenu();
			ImGui::TableSetColumnIndex(1);
			ImGui::Text("%s", resourceName);
			showContextMenu();
			ImGui::TableSetColumnIndex(2);
			ImGui::Text("%p", component);
			showContextMenu();

			ImGui::PopID();
		}

		ImGui::EndTable();
	}

	ImGui::End();
}

bool EntityWindow::Close()
{
	return !m_WindowOpen || !m_Entity;
}

std::string EntityWindow::GetId() const
{
	return std::format("Entity \"{0:}\" ({1:016X})", m_Entity->GetName().c_str(), reinterpret_cast<uintptr_t>(m_Entity.get()));
}

}
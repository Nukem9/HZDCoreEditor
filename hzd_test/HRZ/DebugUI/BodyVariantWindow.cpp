#include "../../ModConfig.h"
#include "../Core/Application.h"
#include "../Core/BodyVariantRuntimeComponent.h"
#include "../Core/Player.h"

#include "BodyVariantWindow.h"

namespace HRZ::DebugUI
{

void BodyVariantWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 600), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	// Draw list
	static std::string previouslySelectedUUID;

	m_VariantNameFilter.Draw();

	if (ImGui::BeginListBox("##BodyVariantSelector", ImVec2(-FLT_MIN, -FLT_MIN)))
	{
		for (auto& [corePath, uuid] : ModConfiguration.CachedBodyVariants)
		{
			char fullName[2048];
			sprintf_s(fullName, "%s, %s", uuid.c_str(), corePath.c_str());

			if (!m_VariantNameFilter.PassFilter(fullName))
				continue;

			if (ImGui::Selectable(fullName, previouslySelectedUUID == uuid))
			{
				previouslySelectedUUID = uuid;

				auto entity = Player::GetLocalPlayer()->m_Entity;
				auto component = entity->m_Components.FindComponent<BodyVariantRuntimeComponent>();

				component->ForceSetUnlistedVariantByPath(corePath.c_str(), GGUUID::TryParse(uuid).value());
			}
		}

		ImGui::EndListBox();
	}

	ImGui::End();
}

bool BodyVariantWindow::Close()
{
	return !m_WindowOpen || !Application::IsInGame();
}

std::string BodyVariantWindow::GetId() const
{
	return "Body Variants";
}

}
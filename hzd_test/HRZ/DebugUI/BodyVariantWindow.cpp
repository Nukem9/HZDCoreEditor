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
		for (auto& variant : ModConfiguration.CachedBodyVariants)
		{
			char fullName[2048];
			sprintf_s(fullName, "%s, %s", variant.Name.c_str(), variant.UUID.c_str());

			if (!m_VariantNameFilter.PassFilter(fullName))
				continue;

			if (ImGui::Selectable(fullName, previouslySelectedUUID == variant.UUID))
			{
				previouslySelectedUUID = variant.UUID;

				auto entity = Player::GetLocalPlayer()->m_Entity;
				auto component = entity->m_Components.FindComponent<BodyVariantRuntimeComponent>();

				component->ForceSetUnlistedVariantByPath(variant.CorePath.c_str(), GGUUID::Parse(variant.UUID));
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
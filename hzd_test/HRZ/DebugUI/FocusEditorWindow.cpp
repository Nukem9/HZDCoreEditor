#include <imgui.h>

#include "../Core/EntityComponent.h"
#include "../Core/Entity.h"
#include "../Core/Player.h"
#include "../Core/FocusComponent.h"

#include "DebugUI.h"
#include "FocusEditorWindow.h"

namespace HRZ::DebugUI
{

extern float MyColors[3];
extern float MyExponent;

void FocusEditorWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 500), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin("Focus Editor", &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	ImGui::ColorPicker4("Color Picker", MyColors, ImGuiColorEditFlags_AlphaBar);
	ImGui::InputFloat("Color Exponent", &MyExponent, 0.1f, 10.0f, "%.3f");

	ImGui::End();
}

bool FocusEditorWindow::Close()
{
	return !m_WindowOpen;
}

void UpdateFocusVertexColors()
{
	auto player = Player::GetLocalPlayer();

	if (!player || !player->m_Entity)
		return;

	auto focusComponent = player->m_Entity->m_Components.FindComponent<FocusComponent>();

	if (!focusComponent)
		return;

	auto resource = static_cast<FocusComponentResource *>(focusComponent->m_Resource.get());
}

}
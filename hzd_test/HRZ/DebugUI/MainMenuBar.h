#pragma once

#include <imgui.h>

#include "../PCore/Ref.h"
#include "../Core/GameModule.h"
#include "../Core/SlowMotionManager.h"
#include "../Core/Player.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"
#include "EntityWindow.h"
#include "WeatherWindow.h"
#include "DemoWindow.h"

namespace HRZ::DebugUI
{

class MainMenuBar : public Window
{
public:
	virtual void Render() override
	{
		if (!ImGui::BeginMainMenuBar())
			return;

		// Empty space for MSI afterburner display
		if (ImGui::BeginMenu("                        ", false))
			ImGui::EndMenu();

		if (ImGui::BeginMenu("Weather"))
		{
			if (ImGui::MenuItem("Weather Editor"))
			{
				AddWindow(std::make_unique<WeatherWindow>());
			}

			ImGui::EndMenu();
		}

		if (ImGui::BeginMenu("Entities"))
		{
			if (ImGui::MenuItem("Player"))
			{
				AddWindow(std::make_unique<EntityWindow>(Player::GetLocalPlayer()->m_Entity));
			}

			ImGui::EndMenu();
		}

		if (ImGui::BeginMenu("Timescale"))
		{
			auto modifyTimescale = [](float Scale)
			{
				static auto id = SlowMotionManager::TimescaleModifierId::Invalid;
				const bool reset = Scale == 1.0f;

				if (id != SlowMotionManager::TimescaleModifierId::Invalid || reset)
					GameModule::Instance->m_SlowMotionManager->RemoveTimescaleModifier(id, 0.0f);

				if (!reset)
					id = GameModule::Instance->m_SlowMotionManager->AddTimescaleModifier(Scale, 1.0f, 0.0f);
			};

			static float targetScale = 1.0f;

			if (ImGui::MenuItem("Set timescale to 0.25"))
				targetScale = 0.25f;
			if (ImGui::MenuItem("Set timescale to 0.5"))
				targetScale = 0.5f;
			if (ImGui::MenuItem("Set timescale to 1"))
				targetScale = 1.0f;
			if (ImGui::MenuItem("Set timescale to 2"))
				targetScale = 2.0f;
			if (ImGui::MenuItem("Set timescale to 5"))
				targetScale = 5.0f;

			ImGui::Separator();
			ImGui::SliderFloat("##TimescaleDragFloat", &targetScale, 0.01f, 5.0f);

			if (targetScale != 0.0f)
				modifyTimescale(targetScale);

			ImGui::EndMenu();
		}

		if (ImGui::BeginMenu("Misc"))
		{
			if (ImGui::MenuItem("Show Demo Window"))
				AddWindow(std::make_unique<DemoWindow>());
		}

		ImGui::EndMainMenuBar();
	}

	virtual bool Close() override
	{
		return false;
	}
};

}
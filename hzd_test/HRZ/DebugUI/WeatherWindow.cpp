#include <mutex>
#include <unordered_set>
#include <algorithm>
#include <imgui.h>

#include "../Core/Application.h"
#include "../Core/GameModule.h"
#include "../Core/WeatherSetup.h"
#include "../Core/WeatherSystem.h"

#include "DebugUI.h"
#include "WeatherWindow.h"

extern HRZ::SharedLock ResourceListLock;
extern std::unordered_set<HRZ::RTTIRefObject *> CachedWeatherSetups;

namespace HRZ::DebugUI
{

void WeatherWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 1000), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	// Gather & sort
	std::scoped_lock lock(ResourceListLock);
	std::vector<WeatherSetup *> sortedSetups;

	for (auto refObject : CachedWeatherSetups)
		sortedSetups.push_back(RTTI::Cast<WeatherSetup>(refObject));

	std::sort(sortedSetups.begin(), sortedSetups.end(), [](WeatherSetup *A, WeatherSetup *B)
	{
		return A->GetName() < B->GetName();
	});

	// Draw list
	static GGUUID selectedUUID;
	WeatherSetup *selectedObjectThisFrame = nullptr;
	bool forceSet = false;

	m_WeatherNameFilter.Draw();

	if (ImGui::BeginListBox("##WeatherSelector", ImVec2(-FLT_MIN, 250)))
	{
		for (auto setup : sortedSetups)
		{
			auto& name = setup->GetName();

			if (!m_WeatherNameFilter.PassFilter(name.c_str()))
				continue;

			char itemText[512];
			sprintf_s(itemText, "%s##%p", name.c_str(), setup);

			if (ImGui::Selectable(itemText, setup->m_ObjectUUID == selectedUUID))
			{
				selectedUUID = setup->m_ObjectUUID;
				forceSet = true;
			}

			if (setup->m_ObjectUUID == selectedUUID)
				selectedObjectThisFrame = setup;
		}

		ImGui::EndListBox();
	}

	ImGui::Separator();

	// Draw options
	if (selectedObjectThisFrame)
		DrawWeatherSetupEditor(selectedObjectThisFrame, forceSet);

	ImGui::End();
}

bool WeatherWindow::Close()
{
	return !m_WindowOpen;
}

std::string WeatherWindow::GetId() const
{
	return "Weather Editor";
}

void WeatherWindow::DrawWeatherSetupEditor(WeatherSetup *Setup, bool ForceSet)
{
	bool changed = false;

	static bool setOnUpdates = true;
	ImGui::Checkbox("Update weather on value change", &setOnUpdates);

	ImGui::PushItemWidth(200.0f);
	{
#define DO_FLOAT_INPUT(x) changed |= ImGui::InputFloat(#x, &Setup->m_Settings.m_##x, 0.5f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		DO_FLOAT_INPUT(CloudCoverage);
		DO_FLOAT_INPUT(CloudCoverageVariation);
		DO_FLOAT_INPUT(CloudCoverageVariationFrequency);
		DO_FLOAT_INPUT(CloudCoverageNoise1Amplitude);
		DO_FLOAT_INPUT(CloudCoverageNoise1Frequency);
		DO_FLOAT_INPUT(CloudCoverageNoise2Amplitude);
		DO_FLOAT_INPUT(CloudCoverageNoise2Frequency);
		DO_FLOAT_INPUT(CloudConnectivity);
		DO_FLOAT_INPUT(CloudDensityExponent);
		DO_FLOAT_INPUT(CloudDensityScale);
		DO_FLOAT_INPUT(CloudType);
		DO_FLOAT_INPUT(CloudTypeVariation);
		DO_FLOAT_INPUT(CloudTypeVariationFrequency);
		DO_FLOAT_INPUT(CloudScrollSpeed);
		DO_FLOAT_INPUT(CloudAnvilAmount);
		DO_FLOAT_INPUT(CloudAnvilSkew);
		DO_FLOAT_INPUT(CloudCustomWindDirectionBlendFactor);
		DO_FLOAT_INPUT(CloudCustomWindDirection);
		DO_FLOAT_INPUT(CloudCustomWindSpeed);
		DO_FLOAT_INPUT(CloudHeightOffset);
		DO_FLOAT_INPUT(CloudNoiseFrequency);
		DO_FLOAT_INPUT(Precipitation);
		DO_FLOAT_INPUT(PrecipitationVariation);
		DO_FLOAT_INPUT(PrecipitationVariationFrequency);
		DO_FLOAT_INPUT(RainbowIntensity);
		DO_FLOAT_INPUT(SundogIntensity);
		DO_FLOAT_INPUT(CirrusCloudDensity);
		DO_FLOAT_INPUT(Humidity);
		changed |= ImGui::InputFloat("WindSpeedMin", &Setup->m_Settings.m_WindSpeed.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		changed |= ImGui::InputFloat("WindSpeedMax", &Setup->m_Settings.m_WindSpeed.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		changed |= ImGui::InputFloat("WindDirectionAngleMin", &Setup->m_Settings.m_WindDirectionAngle.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		changed |= ImGui::InputFloat("WindDirectionAngleMax", &Setup->m_Settings.m_WindDirectionAngle.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		changed |= ImGui::InputFloat("TemperatureLimitsMin", &Setup->m_Settings.m_TemperatureLimits.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
		changed |= ImGui::InputFloat("TemperatureLimitsMax", &Setup->m_Settings.m_TemperatureLimits.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
#undef DO_FLOAT_INPUT
	}
	ImGui::PopItemWidth();

	if ((changed && setOnUpdates) || ForceSet)
	{
		auto& system = Application::Instance().m_GameModule->m_WeatherSystem;
		system->SetWeatherOverride(Setup, 1.0f, 0);
	}
}

}
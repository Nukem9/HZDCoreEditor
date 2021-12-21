#include <unordered_set>
#include <algorithm>
#include <imgui.h>

#include "../Core/Application.h"
#include "../Core/GameModule.h"
#include "../Core/WeatherSetup.h"
#include "../Core/WeatherSystem.h"

#include "DebugUI.h"
#include "WeatherWindow.h"

extern std::unordered_set<HRZ::RTTIRefObject *> AllResources;

namespace HRZ::DebugUI
{

void WeatherWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 1000), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin("Weather Editor", &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	std::vector<WeatherSetup *> sortedSetups;

	for (auto resource : AllResources)
	{
		if (auto asWeatherSetup = RTTI::Cast<WeatherSetup>(resource); asWeatherSetup)
			sortedSetups.push_back(asWeatherSetup);
	}

	std::sort(sortedSetups.begin(), sortedSetups.end(), [](WeatherSetup *A, WeatherSetup *B)
	{
		return A->GetName() < B->GetName();
	});

	static size_t selectedIndex = std::numeric_limits<size_t>::max();
	bool forceSet = false;

	if (ImGui::BeginListBox("##WeatherSelector", ImVec2(-FLT_MIN, 250)))
	{
		for (size_t i = 0; i < sortedSetups.size(); i++)
		{
			char itemText[512];
			sprintf_s(itemText, "%s##%p", sortedSetups[i]->GetName().c_str(), sortedSetups[i]);

			if (ImGui::Selectable(itemText, selectedIndex == i))
			{
				selectedIndex = i;
				forceSet = true;
			}
		}

		ImGui::EndListBox();
	}

	if (selectedIndex < sortedSetups.size())
		DrawWeatherSetupEditor(sortedSetups[selectedIndex], forceSet);

	ImGui::End();
}

bool WeatherWindow::Close()
{
	return !m_WindowOpen;
}

void WeatherWindow::DrawWeatherSetupEditor(WeatherSetup *Setup, bool ForceSet)
{
	bool changed = false;

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

	static bool setOnUpdates = true;
	ImGui::Checkbox("Set weather on value change", &setOnUpdates);

	if ((changed && setOnUpdates) || ForceSet)
	{
		auto& system = Application::Instance().m_GameModule->m_WeatherSystem;
		system->SetWeatherOverride(Setup, 1.0f, 0);
	}
}

}
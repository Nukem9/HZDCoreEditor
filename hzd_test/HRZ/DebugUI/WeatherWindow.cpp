#include <imgui.h>

#include "../Core/Application.h"
#include "../Core/GameModule.h"
#include "../Core/WeatherSetup.h"
#include "../Core/WeatherSystem.h"
#include "../Core/Climate.h"
#include "../Core/ClimateWeatherState.h"

#include "DebugUI.h"
#include "WeatherWindow.h"

namespace HRZ::DebugUI
{

void WeatherWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(512.0f, 700.0f), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin("Weather Editor", &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	auto system = Application::Instance().m_GameModule->m_WeatherSystem.get();
	auto setup = system->m_CurrentWeatherSetup.get();
	bool changed = false;

	ImGui::Text("Climate: %s", system->m_Climate ? system->m_Climate->GetName().c_str() : "<None>");
	ImGui::Text("Weather State: %s", system->m_ClimateWeatherState ? system->m_ClimateWeatherState->GetName().c_str() : "<None>");
	ImGui::Text("Default Setup: %s", system->m_DefaultWeatherSetup->GetName().c_str());
	ImGui::Text("Current Setup: %s", system->m_CurrentWeatherSetup->GetName().c_str());
	ImGui::Text("Previous Setup: %s", system->m_PreviousWeatherSetup->GetName().c_str());

#define DO_FLOAT_INPUT(x) changed |= ImGui::InputFloat(#x, &setup->m_Settings.m_##x, 0.5f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
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
	changed |= ImGui::InputFloat("WindSpeedMin", &setup->m_Settings.m_WindSpeed.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
	changed |= ImGui::InputFloat("WindSpeedMax", &setup->m_Settings.m_WindSpeed.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
	changed |= ImGui::InputFloat("WindDirectionAngleMin", &setup->m_Settings.m_WindDirectionAngle.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
	changed |= ImGui::InputFloat("WindDirectionAngleMax", &setup->m_Settings.m_WindDirectionAngle.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
	changed |= ImGui::InputFloat("TemperatureLimitsMin", &setup->m_Settings.m_TemperatureLimits.Min, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
	changed |= ImGui::InputFloat("TemperatureLimitsMax", &setup->m_Settings.m_TemperatureLimits.Max, 1.0f, 10.0f, "%.3f", ImGuiInputTextFlags_EnterReturnsTrue);
#undef DO_FLOAT_INPUT

	if (changed)
		system->SetWeatherOverride(setup, 1.0f, 0);

	ImGui::End();
}

bool WeatherWindow::Close()
{
	return !m_WindowOpen;
}

}
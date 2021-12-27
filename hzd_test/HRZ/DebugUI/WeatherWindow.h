#pragma once

#include <string>

#include "DebugUIWindow.h"

namespace HRZ
{
class WeatherSetup;
}

namespace HRZ::DebugUI
{

class WeatherWindow : public Window
{
private:
	bool m_WindowOpen = true;
	ImGuiTextFilter m_WeatherNameFilter;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

private:
	void DrawWeatherSetupEditor(WeatherSetup *Setup, bool ForceSet);
};

}
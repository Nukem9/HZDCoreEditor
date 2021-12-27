#pragma once

#include <string>

#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class FocusEditorWindow : public Window
{
private:
	bool m_WindowOpen = true;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

private:
	void UpdateFocusVertexColors();
};

}
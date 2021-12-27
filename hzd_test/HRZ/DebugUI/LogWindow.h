#pragma once

#include <string>
#include <vector>
#include <mutex>
#include <imgui.h>

#include "DebugUIWindow.h"

namespace HRZ::DebugUI
{

class LogWindow : public Window
{
private:
	bool m_WindowOpen = true;
	bool m_AutoScroll = true;

	size_t m_LineCountFilterCache = 0;
	std::vector<int> m_FilteredLines;
	ImGuiTextFilter m_Filter;

	// Shamelessly ripped from the dear imgui log window demo
	static inline std::vector<int> m_LineOffsets;
	static inline ImGuiTextBuffer m_Buf;
	static inline std::recursive_mutex m_Mutex;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

	static void Clear();
	static void AddLog(const char *Format, ...);
};

}
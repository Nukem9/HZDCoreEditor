#pragma once

#include <imgui.h>
#include <vector>
#include <mutex>

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
	virtual void Render() override
	{
		char windowName[512];
		sprintf_s(windowName, "Log Window##0x%p", this);

		if (ImGui::Begin(windowName, &m_WindowOpen))
		{
			std::lock_guard lock(m_Mutex);

			// Options menu
			if (ImGui::BeginPopup("Options"))
			{
				ImGui::Checkbox("Auto-scroll", &m_AutoScroll);
				ImGui::EndPopup();
			}

			if (ImGui::Button("Options"))
				ImGui::OpenPopup("Options");

			// Main window
			ImGui::SameLine();
			bool clear = ImGui::Button("Clear");

			ImGui::SameLine();
			bool copy = ImGui::Button("Copy");

			ImGui::SameLine();
			if (m_Filter.Draw("Filter", -100.0f))
				m_LineCountFilterCache = 0;

			ImGui::Separator();
			ImGui::BeginChild("scrolling", ImVec2(0, 0), false, ImGuiWindowFlags_HorizontalScrollbar);

			if (clear)
				Clear();

			if (copy)
				ImGui::LogToClipboard();

			ImGui::PushStyleVar(ImGuiStyleVar_ItemSpacing, ImVec2(0, 0));
			{
				const char *buf = m_Buf.begin();
				const char *bufEnd = m_Buf.end();

				bool filterActive = m_Filter.IsActive();
				size_t clipperItemCount = m_LineOffsets.size();

				// Put filtered lines into a separate vector
				if (filterActive)
				{
					if (m_LineCountFilterCache != m_LineOffsets.size())
					{
						m_LineCountFilterCache = m_LineOffsets.size();
						m_FilteredLines.clear();

						for (int lineNum = 0; lineNum < m_LineOffsets.size(); lineNum++)
						{
							const char *lineStart = buf + m_LineOffsets[lineNum];
							const char *lineEnd = (lineNum + 1 < m_LineOffsets.size()) ? (buf + m_LineOffsets[lineNum + 1] - 1) : bufEnd;

							if (m_Filter.PassFilter(lineStart, lineEnd))
								m_FilteredLines.push_back(lineNum);
						}
					}

					clipperItemCount = m_FilteredLines.size();
				}

				ImGuiListClipper clipper;
				clipper.Begin(static_cast<int>(clipperItemCount));

				while (clipper.Step())
				{
					for (int i = clipper.DisplayStart; i < clipper.DisplayEnd; i++)
					{
						int lineNum = filterActive ? m_FilteredLines[i] : i;

						const char *lineStart = buf + m_LineOffsets[lineNum];
						const char *lineEnd = (lineNum + 1 < m_LineOffsets.size()) ? (buf + m_LineOffsets[lineNum + 1] - 1) : bufEnd;

						ImGui::TextUnformatted(lineStart, lineEnd);
					}
				}

				clipper.End();
			}
			ImGui::PopStyleVar();

			if (m_AutoScroll && ImGui::GetScrollY() >= ImGui::GetScrollMaxY())
				ImGui::SetScrollHereY(1.0f);

			ImGui::EndChild();
		}

		ImGui::End();
	}

	virtual bool Close() override
	{
		return !m_WindowOpen;
	}

	static void Clear()
	{
		std::lock_guard lock(m_Mutex);

		m_Buf.clear();
		m_LineOffsets.clear();
		m_LineOffsets.push_back(0);
	}

	static void AddLog(const char* fmt, ...)
	{
		std::lock_guard lock(m_Mutex);

		int oldSize = m_Buf.size();
		va_list args;
		va_start(args, fmt);
		m_Buf.appendfv(fmt, args);
		va_end(args);

		for (int newSize = m_Buf.size(); oldSize < newSize; oldSize++)
		{
			if (m_Buf[oldSize] == '\n')
				m_LineOffsets.push_back(oldSize + 1);
		}
	}
};

}
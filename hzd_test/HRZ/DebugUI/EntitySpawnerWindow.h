#pragma once

#include <atomic>

#include "DebugUIWindow.h"

namespace HRZ
{
}

namespace HRZ::DebugUI
{

class EntitySpawnerWindow : public Window
{
private:
	bool m_WindowOpen = true;
	ImGuiTextFilter m_SpawnerNameFilter;

	static inline std::atomic_bool m_DoSpawnOnNextFrame;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

	static void ForceSpawnEntityClick();

private:
	static void DrawCacheStreamedAssets();
};

}
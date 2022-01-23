#pragma once

#include <atomic>
#include <string_view>
#include <imgui.h>

#include "../PCore/StreamingRef.h"
#include "../Core/IStreamingManager.h"
#include "../Core/WorldTransform.h"
#include "DebugUIWindow.h"

namespace HRZ
{
class Resource;
}

namespace HRZ::DebugUI
{

class EntitySpawnerLoaderCallback : public IStreamingRefCallback
{
public:
	std::atomic_uint32_t m_NextSpawnCount;
	WorldTransform m_NextSpawnTransform;
	StreamingRef<Resource> m_NextSpawnSetup;

	virtual ~EntitySpawnerLoaderCallback() override = default;
	virtual void OnStreamingRefLoad(RTTIRefObject *Object) override;
	virtual void OnStreamingRefUnload() override;

	void DoSpawn(const std::string_view CorePath, const std::string_view UUID);
};

class EntitySpawnerWindow : public Window
{
private:
	bool m_WindowOpen = true;
	ImGuiTextFilter m_SpawnerNameFilter;
	size_t m_LastSelectedSetupIndex = -1;
	EntitySpawnerLoaderCallback m_LoaderCallback;

	static inline std::atomic_bool m_DoSpawnOnNextFrame;

public:
	virtual void Render() override;
	virtual bool Close() override;
	virtual std::string GetId() const override;

	static void ForceSpawnEntityClick();
};

}
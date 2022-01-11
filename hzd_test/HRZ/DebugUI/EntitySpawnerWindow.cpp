#include <mutex>
#include <unordered_set>
#include <algorithm>
#include <imgui.h>

#include "../../ModConfig.h"
#include "../Core/Application.h"
#include "../Core/Resource.h"
#include "../Core/Player.h"
#include "../Core/StreamingManager.h"

#include "DebugUI.h"
#include "EntitySpawnerWindow.h"

extern HRZ::SharedLock ResourceListLock;
extern std::unordered_set<HRZ::RTTIRefObject *> CachedSpawnSetupBases;

namespace HRZ
{
DECL_RTTI(Spawnpoint);
DECL_RTTI(SpawnSetupBase);
}

namespace HRZ::DebugUI
{

void EntitySpawnerWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 600), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	// Gather & sort
	std::scoped_lock lock(ResourceListLock);
	std::vector<Resource *> sortedSetups;

	for (auto refObject : CachedSpawnSetupBases)
		sortedSetups.push_back(RTTI::Cast<Resource>(refObject));

	std::sort(sortedSetups.begin(), sortedSetups.end(), [](Resource *A, Resource *B)
	{
		return A->GetName() < B->GetName();
	});

	// Draw list
	static GGUUID selectedUUID;
	Resource *selectedObjectThisFrame = nullptr;

	m_SpawnerNameFilter.Draw();

	if (ImGui::BeginListBox("##SpawnSetupSelector", ImVec2(-FLT_MIN, -200)))
	{
		for (auto setup : sortedSetups)
		{
			auto& name = setup->GetName();

			if (!m_SpawnerNameFilter.PassFilter(name.c_str()))
				continue;

			char itemText[512];
			sprintf_s(itemText, "%s##%p", name.c_str(), setup);

			if (ImGui::Selectable(itemText, setup->m_ObjectUUID == selectedUUID))
				selectedUUID = setup->m_ObjectUUID;

			if (setup->m_ObjectUUID == selectedUUID)
				selectedObjectThisFrame = setup;
		}

		ImGui::EndListBox();
	}

	DrawCacheStreamedAssets();

	// Draw settings
	ImGui::Separator();

	if (!selectedObjectThisFrame)
		ImGui::BeginDisabled(true);

	static int spawnCount = 1;
	static int spawnLocationType = 0;
	static WorldPosition customSpawnPosition;

	ImGui::SetNextItemWidth(200); ImGui::InputInt("Entity count", &spawnCount);
	ImGui::Spacing();
	ImGui::RadioButton("Spawn at player position", &spawnLocationType, 0);
	ImGui::RadioButton("Spawn at crosshair position", &spawnLocationType, 1);
	ImGui::RadioButton("Spawn at custom position", &spawnLocationType, 2);
	ImGui::Spacing();

	if (spawnLocationType == 2)
	{
		ImGui::PushItemWidth(200);
		ImGui::InputDouble("X", &customSpawnPosition.X, 1.0, 20.0, "%.3f");
		ImGui::InputDouble("Y", &customSpawnPosition.Y, 1.0, 20.0, "%.3f");
		ImGui::InputDouble("Z", &customSpawnPosition.Z, 1.0, 20.0, "%.3f");
		ImGui::PopItemWidth();
		ImGui::Spacing();
	}

	auto getSpawnTransform = []()
	{
		auto player = Player::GetLocalPlayer();
		auto currentTransform = player->m_Entity->m_Orientation;

		if (spawnLocationType == 0)
		{
			// Player position
			currentTransform.Position = player->m_Entity->m_Orientation.Position;
		}
		else if (spawnLocationType == 1)
		{
			// Crosshair position
			auto camera = player->GetLastActivatedCamera();
			auto cameraMatrix = camera->m_Orientation.Orientation;

			float yaw;
			float pitch;
			cameraMatrix.Decompose(&yaw, &pitch, nullptr);

			// Project forwards
			Vec3 moveDirection(sin(yaw) * cos(pitch), cos(yaw) * cos(pitch), -sin(pitch));
			moveDirection = moveDirection * 200.0f;

			currentTransform.Position.X = camera->m_Orientation.Position.X + moveDirection.X;
			currentTransform.Position.Y = camera->m_Orientation.Position.Y + moveDirection.Y;
			currentTransform.Position.Z = camera->m_Orientation.Position.Z + moveDirection.Z;

			// Raycast
			WorldPosition rayHitPosition;
			float unknownFloat;
			uint16_t materialType;
			Entity *unknownEntity;
			Vec3 normal;

			Offsets::CallID<"NodeGraph::ExportedIntersectLine", void(*)(WorldPosition&, WorldPosition&, int, const Entity *, bool, WorldPosition *, Vec3 *, float *, Entity **, uint16_t *)>
				(camera->m_Orientation.Position, currentTransform.Position, 47, nullptr, false, &rayHitPosition, &normal, &unknownFloat, &unknownEntity, &materialType);

			currentTransform.Position = rayHitPosition;
		}
		else if (spawnLocationType == 2)
		{
			// Custom position
			currentTransform.Position = customSpawnPosition;
		}

		return currentTransform;
	};

	if (ImGui::Button("Spawn") || (selectedObjectThisFrame && m_DoSpawnOnNextFrame))
	{
		for (int i = 0; i < spawnCount; i++)
		{
			auto spawnpoint = Offsets::CallID<"RTTI::CreateObject", RTTIRefObject *(*)(const RTTI *)>(RTTI_Spawnpoint);
			auto rtti = spawnpoint->GetRTTI()->AsClass();

			spawnpoint->IncRef();
			rtti->SetMemberValue<GGUUID>(spawnpoint, "ObjectUUID", GGUUID::Generate());
			rtti->SetMemberValue<WorldTransform>(spawnpoint, "Orientation", getSpawnTransform());
			rtti->SetMemberValue<String>(spawnpoint, "Name", "UI_Manually_Spawned_Entity");
			rtti->SetMemberValue<Ref<Resource>>(spawnpoint, "SpawnSetup", selectedObjectThisFrame);
			rtti->SetMemberValue<float>(spawnpoint, "Radius", 1.0f);
			rtti->SetMemberValue<float>(spawnpoint, "DespawnRadius", 0.0f);
			// WeakPtr<Entity> @ Spawnpoint+0x188

			Offsets::CallID<"NodeGraph::ExportedSpawnpointSpawn", void(*)(RTTIRefObject *)>(spawnpoint);
			spawnpoint->DecRef();
		}
	}

	if (!selectedObjectThisFrame)
		ImGui::EndDisabled();

	ImGui::End();

	m_DoSpawnOnNextFrame = false;
}

bool EntitySpawnerWindow::Close()
{
	return !m_WindowOpen || !Application::IsInGame();
}

std::string DebugUI::EntitySpawnerWindow::GetId() const
{
	return "Entity Spawner";
}

void EntitySpawnerWindow::ForceSpawnEntityClick()
{
	m_DoSpawnOnNextFrame = true;
}

void EntitySpawnerWindow::DrawCacheStreamedAssets()
{
	static auto& cachedHandles = *new std::vector<StreamingRefHandle>();

	bool streamedAssetsLoaded = !cachedHandles.empty();
	auto text = streamedAssetsLoaded ? "Unload cached spawn setups" : "Force load cached spawn setups";

	if (ImGui::Button(text, ImVec2(-FLT_MIN, 0)))
	{
		if (streamedAssetsLoaded)
		{
			// Release references to the handles
			cachedHandles.clear();
		}
		else
		{
			cachedHandles.resize(ModConfiguration.CachedSpawnSetups.size());

			for (size_t i = 0; i < ModConfiguration.CachedSpawnSetups.size(); i++)
			{
				auto& [corePath, uuid] = ModConfiguration.CachedSpawnSetups[i];

				IStreamingManager::AssetLink link
				{
					.m_Handle = &cachedHandles[i],
					.m_Path = corePath.c_str(),
					.m_UUID = GGUUID::TryParse(uuid).value(),
				};

				StreamingManager::Instance()->CreateHandleFromLink(link);
				StreamingManager::Instance()->UpdateLoadState(*link.m_Handle, 7);
			}
		}
	}
}

}
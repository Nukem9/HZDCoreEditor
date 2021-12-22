#include <mutex>
#include <unordered_set>
#include <algorithm>
#include <imgui.h>

#include "../Core/Application.h"
#include "../Core/GameModule.h"
#include "../Core/Resource.h"
#include "../Core/Player.h"

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

	if (!ImGui::Begin("Entity Spawner", &m_WindowOpen))
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

	// Draw settings
	if (!selectedObjectThisFrame)
		ImGui::BeginDisabled(true);

	static int spawnCount = 1;
	static int spawnLocationType = 0;
	static WorldPosition spawnPosition;

	ImGui::Separator();
	ImGui::SetNextItemWidth(200); ImGui::InputInt("Entity count", &spawnCount);
	ImGui::Spacing();
	ImGui::RadioButton("Spawn at player position", &spawnLocationType, 0);
	ImGui::RadioButton("Spawn at crosshair position", &spawnLocationType, 1);
	ImGui::RadioButton("Spawn at custom position", &spawnLocationType, 2);
	ImGui::Spacing();

	auto player = Player::GetLocalPlayer();
	auto camera = player->GetLastActivatedCamera();

	auto cameraMatrix = camera->m_Orientation.Orientation;
	float yaw;
	float pitch;
	cameraMatrix.Decompose(&yaw, &pitch, nullptr);
	Vec3 moveDirection(sin(yaw) * cos(pitch), cos(yaw) * cos(pitch), -sin(pitch));
	moveDirection = moveDirection * 50.0f;

	auto testPosition = moveDirection;// camera->m_Orientation.Orientation * Vec3(10, 10, 10);
	spawnPosition.X = camera->m_Orientation.Position.X + testPosition.X;
	spawnPosition.Y = camera->m_Orientation.Position.Y + testPosition.Y;
	spawnPosition.Z = camera->m_Orientation.Position.Z + testPosition.Z;

	WorldTransform xform
	{
		.Position = spawnPosition,
		.Orientation = camera->m_Orientation.Orientation,
	};

	if (spawnLocationType == 2)
	{
		ImGui::PushItemWidth(200);
		ImGui::InputDouble("X", &spawnPosition.X, 1.0, 20.0, "%.3f");
		ImGui::InputDouble("Y", &spawnPosition.Y, 1.0, 20.0, "%.3f");
		ImGui::InputDouble("Z", &spawnPosition.Z, 1.0, 20.0, "%.3f");
		ImGui::PopItemWidth();
		ImGui::Spacing();
	}

	if (ImGui::Button("Spawn"))
	{

		for (int i = 0; i < spawnCount; i++)
		{
			auto spawnpoint = (RTTIRefObject *)Offsets::Call<0x02F5CD0, void *(*)(const RTTI *)>(RTTI_Spawnpoint);
			auto rtti = spawnpoint->GetRTTI()->AsClass();

			spawnpoint->IncRef();
			rtti->SetMemberValue<GGUUID>(spawnpoint, "ObjectUUID", GGUUID::Generate());
			rtti->SetMemberValue<WorldTransform>(spawnpoint, "Orientation", xform);
			rtti->SetMemberValue<String>(spawnpoint, "Name", "UI_Manually_Spawned_Entity");
			rtti->SetMemberValue<Ref<Resource>>(spawnpoint, "SpawnSetup", selectedObjectThisFrame);
			rtti->SetMemberValue<float>(spawnpoint, "Radius", 1.0f);
			rtti->SetMemberValue<float>(spawnpoint, "DespawnRadius", 1000000.0f);
			// WeakPtr<Entity> @ Spawnpoint+0x188

			Offsets::Call<0x0C49D00, void(*)(RTTIRefObject *)>(spawnpoint);
			spawnpoint->DecRef();
		}
	}

	if (!selectedObjectThisFrame)
		ImGui::EndDisabled();

	ImGui::End();
}

bool EntitySpawnerWindow::Close()
{
	return !m_WindowOpen;
}

}
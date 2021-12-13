#pragma once

#include <imgui.h>

#include "../../RTTIScanner.h"
#include "../../RTTIIDAExporter.h"

#include "../PCore/Ref.h"
#include "../Core/WorldPosition.h"
#include "../Core/Vec3.h"
#include "../Core/GameModule.h"
#include "../Core/SlowMotionManager.h"
#include "../Core/Player.h"
#include "../Core/CameraEntity.h"
#include "../Core/Application.h"
#include "../Core/DebugSettings.h"
#include "../Core/WorldState.h"
#include "../Core/Mover.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"
#include "EntityWindow.h"
#include "WeatherWindow.h"
#include "DemoWindow.h"
#include "LogWindow.h"

namespace HRZ::DebugUI
{

class MainMenuBar : public Window
{
public:
	enum class FreeCamMode
	{
		Off,
		Free,
		Noclip,
	};

	static inline FreeCamMode m_FreeCamMode;
	static inline WorldPosition m_FreeCamPosition;

	void UpdateFreecam()
	{
		if (m_FreeCamMode == FreeCamMode::Off)
			return;

		auto player = Player::GetLocalPlayer();

		if (!player)
			return;

		auto camera = player->GetLastActivatedCamera();

		if (!camera)
			return;

		auto& io = ImGui::GetIO();

		// Set up the camera's rotation matrix
		RotMatrix cameraMatrix;
		float yaw = 0.0f;
		float pitch = 0.0f;

		if (m_FreeCamMode == FreeCamMode::Free)
		{
			// Convert mouse X/Y to yaw/pitch angles in radians
			static float degreesX = 0.0f;
			static float degreesY = 0.0f;

			if (ImGui::IsMouseDragging(ImGuiMouseButton_Right, 0.0f))
			{
				degreesX = fmodf(degreesX + io.MouseDelta.x, 360.0f);
				degreesY = fmodf(degreesY + io.MouseDelta.y, 360.0f);
			}

			yaw = degreesX * (3.14159f / 180.0f);
			pitch = degreesY * (3.14159f / 180.0f);

			cameraMatrix = RotMatrix(yaw, pitch, 0.0f);
		}
		else if (m_FreeCamMode == FreeCamMode::Noclip)
		{
			std::scoped_lock lock(camera->m_DataLock);

			// Convert matrix components to angles
			cameraMatrix = camera->m_Orientation.Orientation;
			cameraMatrix.Decompose(&yaw, &pitch, nullptr);
		}

		// Scale camera velocity based on delta time
		float speed = io.DeltaTime * 5.0f;

		if (io.KeysDown[VK_SHIFT])
			speed *= 10.0f;
		else if (io.KeysDown[VK_CONTROL])
			speed /= 5.0f;

		// WSAD keys for movement
		Vec3 moveDirection(sin(yaw) * cos(pitch), cos(yaw) * cos(pitch), -sin(pitch));

		if (io.KeysDown['W'])
			m_FreeCamPosition += moveDirection * speed;

		if (io.KeysDown['S'])
			m_FreeCamPosition -= moveDirection * speed;

		if (io.KeysDown['A'])
			m_FreeCamPosition -= moveDirection.CrossProduct(Vec3(0, 0, 1)) * speed;

		if (io.KeysDown['D'])
			m_FreeCamPosition += moveDirection.CrossProduct(Vec3(0, 0, 1)) * speed;

		WorldTransform newTransform
		{
			.Position = m_FreeCamPosition,
			.Orientation = cameraMatrix,
		};

		if (m_FreeCamMode == FreeCamMode::Free)
		{
			std::scoped_lock lock(camera->m_DataLock);

			camera->m_PreviousOrientation = newTransform;
			camera->m_Orientation = newTransform;
			camera->m_Flags |= Entity::WorldTransformChanged;
			//CallOffset<0x0BB41A0, void(*)(CameraEntity *, WorldTransform&)>(camera, cameraTransform);
		}
		else if (m_FreeCamMode == FreeCamMode::Noclip)
		{
			player->m_Entity->m_Mover->MoveToWorldTransform(newTransform, 0.01f, false);
		}
	}

	virtual void Render() override
	{
		UpdateFreecam();

		if (!ShouldInterceptInput())
			return;

		if (!ImGui::BeginMainMenuBar())
			return;

		// Empty space for MSI afterburner display
		if (ImGui::BeginMenu("                        ", false))
			ImGui::EndMenu();

		//
		// "Weather" menu
		//
		if (ImGui::BeginMenu("Weather"))
		{
			if (ImGui::MenuItem("Weather Editor"))
			{
				AddWindow(std::make_unique<WeatherWindow>());
			}

			ImGui::EndMenu();
		}

		//
		// "Entities" menu
		//
		if (ImGui::BeginMenu("Entities"))
		{
			if (ImGui::MenuItem("Player"))
			{
				AddWindow(std::make_unique<EntityWindow>(Player::GetLocalPlayer()->m_Entity));
			}

			if (ImGui::MenuItem("Player Camera"))
			{
				AddWindow(std::make_unique<EntityWindow>(Player::GetLocalPlayer()->GetLastActivatedCamera()));
			}

			ImGui::EndMenu();
		}

		//
		// "Time" menu
		//
		if (ImGui::BeginMenu("Time"))
		{
			auto gameModule = Application::Instance().m_GameModule;
			auto worldState = gameModule->m_WorldState;

			int hour = static_cast<int>(worldState->m_TimeOfDay);
			int minute = static_cast<int>((worldState->m_TimeOfDay - hour) * 60.0f);

			static float targetTimescale = 1.0f;
			auto modifyTimescale = [&](float Scale)
			{
				static auto id = SlowMotionManager::TimescaleModifierId::Invalid;
				const bool reset = Scale == 1.0f;

				if (id != SlowMotionManager::TimescaleModifierId::Invalid || reset)
					gameModule->m_SlowMotionManager->RemoveTimescaleModifier(id, 0.0f);

				if (!reset)
					id = gameModule->m_SlowMotionManager->AddTimescaleModifier(Scale, 1.0f, 0.0f);
			};

			ImGui::Text("Days Passed: %u", worldState->m_DayNightCycleCount);
			ImGui::Text("Current Time: %02u:%02u", hour, minute);

			ImGui::Separator();

			static bool pauseGameLogic = false;
			if (ImGui::MenuItem("Pause Game Logic", nullptr, &pauseGameLogic))
			{
				if (pauseGameLogic)
					gameModule->Suspend();
				else
					gameModule->Resume();
			}

			ImGui::MenuItem("Pause Time of Day", "", &worldState->m_PauseTimeOfDay);

			if (ImGui::MenuItem("Pause Day/Night Cycle", "", !worldState->m_DayNightCycleEnabled))
				worldState->m_DayNightCycleEnabled = !worldState->m_DayNightCycleEnabled;

			if (ImGui::BeginMenu("Set Timescale"))
			{
				if (ImGui::MenuItem("0.01"))
					targetTimescale = 0.01f;
				if (ImGui::MenuItem("0.25"))
					targetTimescale = 0.25f;
				if (ImGui::MenuItem("0.5"))
					targetTimescale = 0.5f;
				if (ImGui::MenuItem("1"))
					targetTimescale = 1.0f;
				if (ImGui::MenuItem("2"))
					targetTimescale = 2.0f;
				if (ImGui::MenuItem("5"))
					targetTimescale = 5.0f;

				ImGui::EndMenu();
			}

			ImGui::Separator();

			int oldCount = worldState->m_DayNightCycleCount;
			float timeOfDay = worldState->m_TimeOfDay;

			if (ImGui::SliderFloat("Time of Day", &timeOfDay, 0.0f, 23.999f))
			{
				worldState->SetTimeOfDay(timeOfDay, 0.0f);
				worldState->m_DayNightCycleCount = oldCount;
			}

			ImGui::SliderFloat("Timescale##TimescaleDragFloat", &targetTimescale, 0.01f, 5.0f);

			if (targetTimescale != 0.0f)
				modifyTimescale(targetTimescale);

			ImGui::EndMenu();
		}

		//
		// "Cheats" menu
		//
		if (ImGui::BeginMenu("Cheats"))
		{
			if (ImGui::MenuItem("Enable Freefly Cam", nullptr, m_FreeCamMode == FreeCamMode::Free))
			{
				m_FreeCamMode = (m_FreeCamMode == FreeCamMode::Free) ? FreeCamMode::Off : FreeCamMode::Free;
				m_FreeCamPosition = Player::GetLocalPlayer()->GetLastActivatedCamera()->m_Orientation.Position;
			}

			if (ImGui::MenuItem("Enable Noclip", nullptr, m_FreeCamMode == FreeCamMode::Noclip))
			{
				m_FreeCamMode = (m_FreeCamMode == FreeCamMode::Noclip) ? FreeCamMode::Off : FreeCamMode::Noclip;
				m_FreeCamPosition = Player::GetLocalPlayer()->m_Entity->m_Orientation.Position;
			}

			if (auto debugSettings = Application::Instance().m_DebugSettings; debugSettings)
			{
				if (ImGui::MenuItem("Enable Demigod Mode", nullptr, debugSettings->m_GodModeState == EGodMode::On))
					debugSettings->m_GodModeState = (debugSettings->m_GodModeState == EGodMode::On) ? EGodMode::Off : EGodMode::On;

				if (ImGui::MenuItem("Enable God Mode", nullptr, debugSettings->m_GodModeState == EGodMode::Invulnerable))
					debugSettings->m_GodModeState = (debugSettings->m_GodModeState == EGodMode::Invulnerable) ? EGodMode::Off : EGodMode::Invulnerable;

				ImGui::MenuItem("Enable Infinite Ammo", "", &debugSettings->m_InfiniteAmmo);
				ImGui::MenuItem("Enable Infinite Ammo Reserves", "", &debugSettings->m_InfiniteAmmoReserves);
				ImGui::MenuItem("Enable All Unlocks", "", &debugSettings->m_UnlockAll);
				ImGui::MenuItem("Simulate Game Completed", "", &debugSettings->m_SimulateGameFinished);
			}

			ImGui::Separator();

			if (ImGui::BeginMenu("Teleport To"))
			{
				auto doTeleport = [](WorldPosition Position)
				{
					auto player = Player::GetLocalPlayer();

					if (!player || !player->m_Entity)
						return;

					WorldTransform transform
					{
						.Position = Position,
						.Orientation = player->m_Entity->m_Orientation.Orientation,
					};

					player->m_Entity->PlaceOnWorldTransform(transform, false);
				};

				if (ImGui::MenuItem("Freefly Cam Position"))
					doTeleport(m_FreeCamPosition);
				if (ImGui::MenuItem("Naming Cliff"))
					doTeleport(WorldPosition(2258.91, -1097.40, 359.18));
				if (ImGui::MenuItem("Elizabet Sobeck's Ranch"))
					doTeleport(WorldPosition(5349.0, -2322.0, 120.0));
				if (ImGui::MenuItem("Climbing Testing Area 1"))
					doTeleport(WorldPosition(-2278.0, -2222.0, 219.0));
				if (ImGui::MenuItem("Climbing Testing Area 2"))
					doTeleport(WorldPosition(-2265.0, -2307.0, 224.0));
				if (ImGui::MenuItem("Terrain Slope Testing Area"))
					doTeleport(WorldPosition(-2277.0, -2541.0, 324.0));
				if (ImGui::MenuItem("Script Testing Area"))
					doTeleport(WorldPosition(-2523.0, -2220.0, 221.0));
				if (ImGui::MenuItem("DLC Testing Area"))
					doTeleport(WorldPosition(-4953.22, -4907.42, 258.15));

				ImGui::EndMenu();
			}

			ImGui::EndMenu();
		}

		//
		// "Debug" menu
		//
		if (ImGui::BeginMenu("Debug"))
		{
			if (auto debugSettings = Application::Instance().m_DebugSettings; debugSettings)
			{
				if (ImGui::MenuItem("Enable Damage Logging"))
					Offsets::CallID<"ToggleDamageLogging", void(*)(void *, bool)>(nullptr, true);
				ImGui::MenuItem("Enable Player Cover", "", &debugSettings->m_PlayerCoverEnabled);
				ImGui::MenuItem("Show Debug Coordinates", "", &debugSettings->m_DrawDebugCoordinates);
				ImGui::MenuItem("Disable Inactivity Check", "", &debugSettings->m_DisableInactivityCheck);
			}

			ImGui::Separator();

			if (ImGui::MenuItem("Show Demo Window"))
				AddWindow(std::make_unique<DemoWindow>());

			if (ImGui::MenuItem("Show Log Window"))
				AddWindow(std::make_unique<LogWindow>());

			ImGui::Separator();

			if (ImGui::MenuItem("Dump RTTI Typeinfo"))
				RTTIScanner::ExportAll("C:\\hzd_rtti_export");

			if (ImGui::MenuItem("Dump Fullgame Typeinfo"))
			{
				RTTIIDAExporter idaExporter(RTTIScanner::GetAllTypes());
				idaExporter.ExportFullgameTypes("C:\\hzd_rtti_export");
			}

			ImGui::Separator();
			ImGui::MenuItem("", nullptr, nullptr, false);
			ImGui::MenuItem("", nullptr, nullptr, false);
			ImGui::MenuItem("", nullptr, nullptr, false);
			ImGui::Separator();

			if (ImGui::MenuItem("Terminate Process"))
				TerminateProcess(GetCurrentProcess(), 0);

			ImGui::EndMenu();
		}

		ImGui::EndMainMenuBar();
	}

	virtual bool Close() override
	{
		return false;
	}
};

}
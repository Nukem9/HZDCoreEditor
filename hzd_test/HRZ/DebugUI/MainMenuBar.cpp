#pragma once

#include <imgui.h>

#include "../../RTTI/RTTIScanner.h"
#include "../../RTTI/RTTIIDAExporter.h"

#include "../PCore/Ref.h"
#include "../Core/WorldPosition.h"
#include "../Core/Vec3.h"
#include "../Core/GameModule.h"
#include "../Core/Player.h"
#include "../Core/PlayerGame.h"
#include "../Core/CameraEntity.h"
#include "../Core/Application.h"
#include "../Core/DebugSettings.h"
#include "../Core/WorldState.h"
#include "../Core/Mover.h"
#include "../Core/StreamingManager.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"
#include "EntityWindow.h"
#include "WeatherWindow.h"
#include "DemoWindow.h"
#include "LogWindow.h"
#include "FocusEditorWindow.h"
#include "EntitySpawnerWindow.h"
#include "MainMenuBar.h"

namespace HRZ::DebugUI
{

void MainMenuBar::Render()
{
	if (!ShouldInterceptInput())
		return;

	if (!ImGui::BeginMainMenuBar())
		return;

	// Empty space for MSI afterburner display
	if (ImGui::BeginMenu("                        ", false))
		ImGui::EndMenu();

	// "World" menu
	if (ImGui::BeginMenu("World", Application::IsInGame()))
	{
		DrawWorldMenu();
		ImGui::EndMenu();
	}

	// "Time" menu
	if (ImGui::BeginMenu("Time", Application::IsInGame()))
	{
		DrawTimeMenu();
		ImGui::EndMenu();
	}

	// "Cheats" menu
	if (ImGui::BeginMenu("Cheats", Application::IsInGame()))
	{
		DrawCheatsMenu();
		ImGui::EndMenu();
	}

	// "Saves" menu
	if (ImGui::BeginMenu("Saves", Application::IsInGame()))
	{
		DrawSavesMenu();
		ImGui::EndMenu();
	}

	// "Debug" menu
	if (ImGui::BeginMenu("Debug"))
	{
		DrawDebugMenu();
		ImGui::EndMenu();
	}

	// "Miscellaneous" menu
	if (ImGui::BeginMenu("Miscellaneous"))
	{
		DrawMiscellaneousMenu();
		ImGui::EndMenu();
	}

	// Credits
	auto text = "Game input blocked | Mod created by Nukem (Nukem9)";
	auto len = ImGui::CalcTextSize(text).x;

	ImGui::SetCursorPosX(ImGui::GetWindowContentRegionMax().x - len);
	ImGui::BeginMenu(text, false);

	ImGui::EndMainMenuBar();
}

bool MainMenuBar::Close()
{
	return false;
}

std::string MainMenuBar::GetId() const
{
	return "Main Menu Bar";
}

void MainMenuBar::TogglePauseGameLogic()
{
	auto& gameModule = Application::Instance().m_GameModule;

	if (gameModule)
	{
		if (gameModule->IsSuspended())
			gameModule->Resume();
		else
			gameModule->Suspend();
	}
}

void MainMenuBar::ToggleFreeflyCamera()
{
	if (!Application::IsInGame())
		return;

	m_FreeCamMode = (m_FreeCamMode == FreeCamMode::Free) ? FreeCamMode::Off : FreeCamMode::Free;
	m_FreeCamPosition = Player::GetLocalPlayer()->GetLastActivatedCamera()->m_Orientation.Position;
}

void MainMenuBar::ToggleNoclip()
{
	if (!Application::IsInGame())
		return;

	m_FreeCamMode = (m_FreeCamMode == FreeCamMode::Noclip) ? FreeCamMode::Off : FreeCamMode::Noclip;
	m_FreeCamPosition = Player::GetLocalPlayer()->m_Entity->m_Orientation.Position;
}

void MainMenuBar::SavePlayerGame(SaveType Type)
{
	if (!Application::IsInGame())
		return;

	auto doSave = [](uint8_t SaveType)
	{
		Offsets::CallID<"NodeGraph::ExportedCreateSaveGame", void(*)(uint8_t SaveType, bool Unknown, const class AIMarker *)>(SaveType, false, nullptr);
	};

	switch (Type)
	{
	case SaveType::Quick:
	{
		// RestartOnSpawned determines if the player is moved to their last position or moved to a campfire
		auto playerGame = RTTI::Cast<PlayerGame>(Player::GetLocalPlayer());
		playerGame->m_RestartOnSpawned = true;

		doSave(2);
	}
	break;

	case SaveType::Auto:
		doSave(4);
		break;

	case SaveType::Manual:
		doSave(1);
		break;

	case SaveType::NewGamePlus:
		doSave(8);
		break;
	}
}

void MainMenuBar::LoadPreviousSave()
{
	if (!Application::IsInGame())
		return;

	Offsets::CallID<"NodeGraph::ExportedReloadLastSaveGame", void(*)(float)>(0.0f);
}

void MainMenuBar::DrawWorldMenu()
{
	if (ImGui::MenuItem("Weather Editor"))
		AddWindow(std::make_shared<WeatherWindow>());

	if (ImGui::MenuItem("Entity Spawner"))
		AddWindow(std::make_shared<EntitySpawnerWindow>());

	ImGui::Separator();

	if (ImGui::MenuItem("Player Entity"))
		AddWindow(std::make_shared<EntityWindow>(Player::GetLocalPlayer()->m_Entity));

	if (ImGui::MenuItem("Player Camera Entity"))
		AddWindow(std::make_shared<EntityWindow>(Player::GetLocalPlayer()->GetLastActivatedCamera()));

	//if (ImGui::MenuItem("Player Focus Entity"))
	//	AddWindow(std::make_shared<FocusEditorWindow>());
}

void MainMenuBar::DrawTimeMenu()
{
	auto& gameModule = Application::Instance().m_GameModule;
	auto& worldState = gameModule->m_WorldState;

	float timeOfDay = worldState->m_TimeOfDay;
	int cycleCount = worldState->m_DayNightCycleCount;

	int hour = static_cast<int>(timeOfDay);
	int minute = static_cast<int>((timeOfDay - hour) * 60.0f);

	ImGui::Text("Days Passed: %u", worldState->m_DayNightCycleCount);
	ImGui::Text("Current Time: %02u:%02u", hour, minute);
	ImGui::Separator();

	// Game loop
	if (ImGui::MenuItem("Pause Game Logic", nullptr, gameModule->IsSuspended()))
		TogglePauseGameLogic();

	ImGui::MenuItem("Pause AI Processing", nullptr, &m_PauseAIProcessing);
	ImGui::Separator();

	// Day/night cycle
	ImGui::MenuItem("Pause Time of Day", nullptr, &worldState->m_PauseTimeOfDay);

	if (ImGui::MenuItem("Pause Day/Night Cycle", nullptr, !worldState->m_DayNightCycleEnabled))
		worldState->m_DayNightCycleEnabled = !worldState->m_DayNightCycleEnabled;

	ImGui::MenuItem("Time of Day", nullptr, nullptr, false);

	if (ImGui::SliderFloat("##timeofdaybar", &timeOfDay, 0.0f, 23.9999f))
	{
		worldState->SetTimeOfDay(timeOfDay, 0.0f);
		worldState->m_DayNightCycleCount = cycleCount;
	}

	ImGui::Separator();

	// Timescale
	ImGui::MenuItem("Enable Timescale Override in Menus", nullptr, &m_TimescaleOverrideInMenus);
	ImGui::MenuItem("Enable Timescale Override", nullptr, &m_TimescaleOverride);
	ImGui::MenuItem("Timescale", nullptr, nullptr, false);

	auto modifyTimescale = [](float Scale, bool SameLine = true)
	{
		char temp[64];
		sprintf_s(temp, "%g##setTs%g", Scale, Scale);

		if (ImGui::Button(temp))
		{
			m_Timescale = Scale;
			m_TimescaleOverride = true;
		}

		if (SameLine)
			ImGui::SameLine();
	};

	if (ImGui::SliderFloat("##TimescaleDragFloat", &m_Timescale, 0.001f, 10.0f))
		m_TimescaleOverride = true;

	modifyTimescale(0.01f);
	modifyTimescale(0.25f);
	modifyTimescale(0.5f);
	modifyTimescale(1.0f);
	modifyTimescale(2.0f);
	modifyTimescale(5.0f);
	modifyTimescale(10.0f, false);
}

void MainMenuBar::DrawCheatsMenu()
{
	auto debugSettings = Application::Instance().m_DebugSettings;

	if (ImGui::MenuItem("Enable Freefly Camera", nullptr, m_FreeCamMode == FreeCamMode::Free))
		ToggleFreeflyCamera();

	if (ImGui::MenuItem("Enable Noclip", nullptr, m_FreeCamMode == FreeCamMode::Noclip))
		ToggleNoclip();

	if (ImGui::MenuItem("Enable Demigod Mode", nullptr, debugSettings->m_GodModeState == EGodMode::On))
		debugSettings->m_GodModeState = (debugSettings->m_GodModeState == EGodMode::On) ? EGodMode::Off : EGodMode::On;

	if (ImGui::MenuItem("Enable God Mode", nullptr, debugSettings->m_GodModeState == EGodMode::Invulnerable))
		debugSettings->m_GodModeState = (debugSettings->m_GodModeState == EGodMode::Invulnerable) ? EGodMode::Off : EGodMode::Invulnerable;

	ImGui::MenuItem("Enable Infinite Ammo", nullptr, &debugSettings->m_InfiniteAmmo);
	ImGui::MenuItem("Enable Infinite Ammo Reserves", nullptr, &debugSettings->m_InfiniteAmmoReserves);
	ImGui::MenuItem("Enable All Unlocks", nullptr, &debugSettings->m_UnlockAll);
	ImGui::MenuItem("Simulate Game Completed", nullptr, &debugSettings->m_SimulateGameFinished);

	// Teleport locations
	ImGui::Separator();

	auto doTeleport = [](const WorldPosition Position)
	{
		auto player = Player::GetLocalPlayer();

		if (!player || !player->m_Entity)
			return;

		WorldTransform transform
		{
			.Position = Position,
			.Orientation = player->m_Entity->m_Orientation.Orientation,
		};

		// Fixup so Aloy doesn't fall through the ground
		transform.Position.Z += 0.5;

		m_FreeCamPosition = transform.Position;
		player->m_Entity->PlaceOnWorldTransform(transform, false);
	};

	const static std::vector<std::pair<const char *, WorldPosition>> AreaLocations
	{
		{ "Naming Cliff", { 2258.91, -1097.40, 359.18 } },
		{ "Elizabet Sobeck's Ranch", { 5349.0, -2322.0, 120.0 } },
		{ "Climbing Testing Area 1", { -2278.0, -2222.0, 219.0 } },
		{ "Climbing Testing Area 2", { -2265.0, -2307.0, 224.0 } },
		{ "Terrain Slope Testing Area", { -2277.0, -2541.0, 324.0 } },
		{ "Script Testing Area", { -2523.0, -2220.0, 221.0 } },
		{ "DLC Testing Area", { 4765.22, 4832.68, 282.68 } },
	};

	if (ImGui::BeginMenu("Teleport To Area"))
	{
		if (ImGui::MenuItem("Freefly Camera Position"))
			doTeleport(m_FreeCamPosition);

		for (auto& [k, v] : AreaLocations)
		{
			if (ImGui::MenuItem(k))
				doTeleport(v);
		}

		ImGui::EndMenu();
	}

	const static std::vector<std::pair<const char *, WorldPosition>> UnlockableLocations
	{
		{ "GrazerDummy_01_RostsHovel", { 2166.24, -1521.54, 286.74 } },
		{ "GrazerDummy_02_RostsHovel", { 2163.07, -1525.95, 287.56 } },
		{ "GrazerDummy_03_RostsHovel", { 2158.27, -1526.80, 287.61 } },
		{ "GrazerDummy_04_RostsHovel", { 2148.92, -1509.10, 293.15 } },
		{ "GrazerDummy_05_RostsHovel", { 2141.70, -1496.57, 290.79 } },
		{ "GrazerDummy_06_RostsHovel", { 2166.80, -1496.77, 284.57 } },
		{ "GrazerDummy_07_RostsHovel", { 2163.33, -1496.83, 284.35 } },
		{ "GrazerDummy_08_KarstsShop", { 2829.59, -1956.60, 192.76 } },
		{ "GrazerDummy_09_MothersCradle", { 2731.09, -1914.96, 178.85 } },
		{ "GrazerDummy_10_MothersHeart", { 2518.01, -1359.98, 217.37 } },
		{ "GrazerDummy_11_MothersHeart", { 2489.16, -1361.32, 217.56 } },
		{ "GrazerDummy_12_MothersRise", { 2665.99, -1315.13, 209.17 } },
		{ "GrazerDummy_13_MothersRise", { 2671.79, -1372.27, 209.78 } },
		{ "GrazerDummy_14_MothersGate", { 2401.44, -1868.80, 188.97 } },
		{ "GrazerDummy_15_MothersGate", { 2364.84, -1868.52, 190.54 } },
		{ "GrazerDummy_16_MothersCrown", { 2684.90, -726.55, 180.38 } },
		{ "GrazerDummy_17_MothersCrown", { 2685.16, -719.75, 180.12 } },
		{ "GrazerDummy_18_BanditCamp", { 3076.84, -956.83, 170.23 } },
		{ "GrazerDummy_19_BanditCamp", { 1846.26, -540.75, 305.31 } },
		{ "GrazerDummy_20_HuntersGathering", { 2300.65, -457.92, 226.96 } },
		{ "GrazerDummy_21_HuntersGathering", { 2286.95, -462.21, 227.73 } },
		{ "GrazerDummy_22_HuntingGrounds1", { 2986.81, -1562.17, 194.28 } },
		{ "GrazerDummy_23_HuntingGrounds1", { 2965.68, -1574.96, 195.57 } },
		{ "Vantage_01_Airforce", { 2963.06, -1821.89, 209.79 } },
		{ "Vantage_02_ColoradoBuilding", { 3043.23, -1162.05, 216.93 } },
		{ "Vantage_03_PioneerMuseum", { 2998.87, -854.69, 170.04 } },
		{ "Vantage_04_DenverSkyline", { 3352.45, -573.77, 198.24 } },
		{ "Vantage_05_BridalVeilFalls", { 1775.53, -694.61, 254.63 } },
		{ "Vantage_06_MesaCity", { -176.51, -677.21, 230.51 } },
		{ "Vantage_07_Drone", { -572.33, -1969.96, 123.52 } },
		{ "Vantage_08_Citadel", { -929.59, 545.55, 301.81 } },
		{ "Vantage_09_FaroBuilding", { -424.89, 985.27, 290.85 } },
		{ "Vantage_10_LakePowell", { -610.22, -305.05, 255.51 } },
		{ "Vantage_11_ExplodedMountain", { 1328.36, 1333.82, 460.66 } },
		{ "Vantage_12_RedrockTheater", { 3045.02, -153.56, 185.03 } },
		{ "Vantage_13_DenverStadium", { 3877.23, -26.01, 164.08 } },
	};

	if (ImGui::BeginMenu("Teleport To Unlockable"))
	{
		for (auto& [k, v] : UnlockableLocations)
		{
			if (ImGui::MenuItem(k))
				doTeleport(v);
		}

		ImGui::EndMenu();
	}
}

void MainMenuBar::DrawSavesMenu()
{
	if (ImGui::MenuItem("Force Quicksave"))
		SavePlayerGame(SaveType::Quick);

	if (ImGui::MenuItem("Force Autosave"))
		SavePlayerGame(SaveType::Auto);

	if (ImGui::MenuItem("Force Manual Save"))
		SavePlayerGame(SaveType::Manual);

	if (ImGui::MenuItem("Force NG+ Save"))
		SavePlayerGame(SaveType::NewGamePlus);

	ImGui::Separator();

	if (ImGui::MenuItem("Load Previous Save"))
		LoadPreviousSave();
}

void MainMenuBar::DrawDebugMenu()
{
	if (auto debugSettings = Application::Instance().m_DebugSettings; debugSettings)
	{
		static bool toggleDamageLogging;
		if (ImGui::MenuItem("Enable Damage Logging", nullptr, &toggleDamageLogging))
			Offsets::CallID<"ToggleDamageLogging", void(*)(void *, bool)>(nullptr, toggleDamageLogging);
		ImGui::MenuItem("Enable Player Cover", nullptr, &debugSettings->m_PlayerCoverEnabled);
		if (ImGui::MenuItem("Enable Inactivity Check", nullptr, !debugSettings->m_DisableInactivityCheck))
			debugSettings->m_DisableInactivityCheck = !debugSettings->m_DisableInactivityCheck;
		ImGui::MenuItem("Show Debug Coordinates", nullptr, &debugSettings->m_DrawDebugCoordinates);
	}

	ImGui::Separator();

	if (ImGui::MenuItem("Show Log Window"))
		AddWindow(std::make_unique<LogWindow>());
}

void MainMenuBar::DrawMiscellaneousMenu()
{
	if (ImGui::MenuItem("Show ImGui Demo Window"))
		AddWindow(std::make_unique<DemoWindow>());

	if (ImGui::MenuItem("Dump RTTI Typeinfo"))
		RTTIScanner::ExportAll("C:\\hzd_rtti_export", "HZD");

	if (ImGui::MenuItem("Dump Fullgame Typeinfo"))
	{
		RTTIIDAExporter idaExporter(RTTIScanner::GetAllTypes(), "HZD");
		idaExporter.ExportFullgameTypes("C:\\hzd_rtti_export");
	}

	ImGui::Separator();
	ImGui::MenuItem("", nullptr, nullptr, false);
	ImGui::MenuItem("", nullptr, nullptr, false);
	ImGui::Separator();

	if (ImGui::MenuItem("Terminate Process"))
		TerminateProcess(GetCurrentProcess(), 0);
}

}
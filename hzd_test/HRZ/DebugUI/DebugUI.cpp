#include <d3d12.h>
#include <array>
#include <imgui.h>
#include <imgui_internal.h>
#include <imgui_impl_dx12.h>
#include <imgui_impl_win32.h>

#include "../../ModConfig.h"
#include "../Core/Application.h"
#include "../Core/CursorManager.h"
#include "../Core/PlayerGame.h"
#include "../Core/Mover.h"
#include "../PGraphics3D/RenderingDeviceDX12.h"
#include "../PGraphics3D/RenderingConfiguration.h"
#include "../PGraphics3D/SwapChainDX12.h"
#include "../PGraphics3D/HwRenderBuffer.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"
#include "MainMenuBar.h"
#include "EntitySpawnerWindow.h"

extern IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

namespace HRZ::DebugUI
{

std::unordered_map<std::string, std::shared_ptr<Window>> m_Windows;

std::array<ID3D12CommandAllocator *, SwapChainDX12::BackBufferCount> CommandAllocators;
ID3D12GraphicsCommandList *CommandList;
ID3D12DescriptorHeap *SrvDescHeap;

WNDPROC OriginalWndProc;
bool InterceptInput;

LRESULT WINAPI WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam);

void Initialize(const SwapChainDX12 *SwapChain)
{
	// D3D resources
	HRESULT hr = S_OK;
	auto device = RenderingDeviceDX12::Instance().m_Device;

	D3D12_DESCRIPTOR_HEAP_DESC desc
	{
		.Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
		.NumDescriptors = 10,
		.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
	};

	hr |= device->CreateDescriptorHeap(&desc, IID_PPV_ARGS(&SrvDescHeap));

	for (size_t i = 0; i < CommandAllocators.size(); i++)
		hr |= device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&CommandAllocators[i]));

	hr |= device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, CommandAllocators[0], nullptr, IID_PPV_ARGS(&CommandList));
	hr |= CommandList->Close();

	if (FAILED(hr))
		__debugbreak();

	// ImGui resources
	IMGUI_CHECKVERSION();
	ImGui::CreateContext();
	ImGui::StyleColorsDark();

	auto& style = ImGui::GetStyle();
	style.FrameBorderSize = 1;
	style.ScrollbarRounding = 0;

	auto& io = ImGui::GetIO();
	io.FontGlobalScale = ModConfiguration.DebugMenuFontScale;
	io.MouseDrawCursor = false;

	ImGui_ImplWin32_Init(SwapChain->m_NativeWindowHandle);
	ImGui_ImplDX12_Init(device, SwapChainDX12::BackBufferCount, SwapChain->m_BackBuffers[0].m_Resource->GetDesc().Format, SrvDescHeap, SrvDescHeap->GetCPUDescriptorHandleForHeapStart(), SrvDescHeap->GetGPUDescriptorHandleForHeapStart());
	OriginalWndProc = reinterpret_cast<WNDPROC>(SetWindowLongPtrW(SwapChain->m_NativeWindowHandle, GWLP_WNDPROC, reinterpret_cast<LONG_PTR>(&WndProc)));

	DebugUI::AddWindow(std::make_shared<MainMenuBar>());
}

void AddWindow(std::shared_ptr<Window> Handle)
{
	// Immediately discard duplicate window instances
	auto id = Handle->GetId();

	if (!m_Windows.contains(id))
		m_Windows.emplace(id, Handle);
}

void RenderUI()
{
	ImGui_ImplDX12_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

	UpdateFreecam();

	// Clear window focus for every frame that isn't intercepting input
	if (!InterceptInput)
		ImGui::FocusWindow(nullptr);

	// A copy is required because Render() might create new instances and invalidate iterators
	auto currentWindows = m_Windows;

	for (auto& [name, window] : currentWindows)
	{
		// Detach windows that are pending close first
		if (window->Close())
			m_Windows.erase(name);
		else
			window->Render();
	}

	ImGui::Render();
}

void RenderUID3D(const SwapChainDX12 *SwapChain)
{
	uint32_t bufferIndex = (SwapChain->m_CurrentFrameIndex - 1) % SwapChainDX12::BackBufferCount;
	ID3D12CommandAllocator *allocator = CommandAllocators[bufferIndex];

	allocator->Reset();

	D3D12_RESOURCE_BARRIER barrier {};
	barrier.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
	barrier.Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE;
	barrier.Transition.pResource = SwapChain->m_BackBuffers[bufferIndex].m_Resource;
	barrier.Transition.Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES;
	barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_PRESENT;
	barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET;

	CommandList->Reset(allocator, nullptr);
	CommandList->ResourceBarrier(1, &barrier);

	CommandList->OMSetRenderTargets(1, &SwapChain->m_BackBuffers[bufferIndex].m_RenderBuffer->m_CPUDescriptorHandle, false, nullptr);
	CommandList->SetDescriptorHeaps(1, &SrvDescHeap);
	ImGui_ImplDX12_RenderDrawData(ImGui::GetDrawData(), CommandList);

	barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
	barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;
	CommandList->ResourceBarrier(1, &barrier);
	CommandList->Close();

	SwapChain->m_RenderingConfig->GetCommandQueue()->ExecuteCommandLists(1, reinterpret_cast<ID3D12CommandList * const *>(&CommandList));
}

std::optional<LRESULT> HandleMessage(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	switch (Msg)
	{
	case WM_KEYDOWN:
	{
		bool keyHandled = true;

		if (wParam == ModConfiguration.Hotkeys.ToggleDebugUI)
		{
			// Toggle input blocking and the main menu bar
			ToggleInputInterception();
			MainMenuBar::ToggleVisibility();

			CallWindowProcW(OriginalWndProc, hWnd, WM_ACTIVATEAPP, ShouldInterceptInput() ? 0 : 1, 0);
		}
		else if (wParam == ModConfiguration.Hotkeys.TogglePauseGameLogic)
			MainMenuBar::TogglePauseGameLogic();
		else if (wParam == ModConfiguration.Hotkeys.TogglePauseTimeOfDay)
			MainMenuBar::TogglePauseTimeOfDay();
		else if (wParam == ModConfiguration.Hotkeys.ToggleFreeflyCamera)
			MainMenuBar::ToggleFreeflyCamera();
		else if (wParam == ModConfiguration.Hotkeys.ToggleNoclip)
			MainMenuBar::ToggleNoclip();
		else if (wParam == ModConfiguration.Hotkeys.SaveQuicksave)
			MainMenuBar::SavePlayerGame(MainMenuBar::SaveType::Quick);
		else if (wParam == ModConfiguration.Hotkeys.LoadPreviousSave)
			MainMenuBar::LoadPreviousSave();
		else if (wParam == ModConfiguration.Hotkeys.SpawnEntity)
			EntitySpawnerWindow::ForceSpawnEntityClick();
		else if (wParam == ModConfiguration.Hotkeys.IncreaseTimescale)
			MainMenuBar::AdjustTimescale(0.25f);
		else if (wParam == ModConfiguration.Hotkeys.DecreaseTimescale)
			MainMenuBar::AdjustTimescale(-0.25f);
		else if (wParam == ModConfiguration.Hotkeys.IncreaseTimeOfDay)
			MainMenuBar::AdjustTimeOfDay(1.0f);
		else if (wParam == ModConfiguration.Hotkeys.DecreaseTimeOfDay)
			MainMenuBar::AdjustTimeOfDay(-1.0f);
		else
			keyHandled = false;

		if (keyHandled)
			return 1;
	}
	break;

	case WM_ACTIVATEAPP:
	{
		// Prevent alt-tab from interfering with input blocking
		if (ShouldInterceptInput())
			return 1;
	}
	break;
	}

	return std::nullopt;
}

void ToggleInputInterception()
{
	InterceptInput = !InterceptInput;
	auto cursorManager = Application::Instance().m_CursorManager;

	if (InterceptInput)
	{
		ImGui::GetIO().MouseDrawCursor = true;
		cursorManager->m_UnlockCursorBounds = true;
		cursorManager->m_ForceHideCursor = true;
	}
	else
	{
		ImGui::GetIO().MouseDrawCursor = false;
		cursorManager->m_UnlockCursorBounds = false;
		cursorManager->m_ForceHideCursor = false;
	}

	// Apply freecam overrides (no visible cursor, but input still blocked)
	if (MainMenuBar::m_FreeCamMode == MainMenuBar::FreeCamMode::Free)
		cursorManager->m_UnlockCursorBounds = true;
}

bool ShouldInterceptInput()
{
	return InterceptInput || MainMenuBar::m_FreeCamMode == MainMenuBar::FreeCamMode::Free;
}

void UpdateFreecam()
{
	auto cameraMode = MainMenuBar::m_FreeCamMode;
	auto& cameraPosition = MainMenuBar::m_FreeCamPosition;

	if (cameraMode == MainMenuBar::FreeCamMode::Off)
		return;

	if (!Application::IsInGame())
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

	if (cameraMode == MainMenuBar::FreeCamMode::Free)
	{
		// Convert mouse X/Y to yaw/pitch angles
		static float currentCursorX = 0.0f;
		static float currentCursorY = 0.0f;
		static float targetCursorX = 0.0f;
		static float targetCursorY = 0.0f;

		if (ImGui::IsMouseDragging(ImGuiMouseButton_Right, 0.0f))
		{
			targetCursorX += io.MouseDelta.x * 0.5f;
			targetCursorY += io.MouseDelta.y * 0.5f;
		}

		// Exponential decay view angle smoothing (https://stackoverflow.com/a/10228863)
		const double springiness = 60.0;
		const float mult = static_cast<float>(1.0 - std::exp(std::log(0.5) * springiness * io.DeltaTime));

		currentCursorX += (targetCursorX - currentCursorX) * mult;
		currentCursorY += (targetCursorY - currentCursorY) * mult;

		float degreesX = std::fmodf(currentCursorX, 360.0f);
		if (degreesX < 0) degreesX += 360.0f;
		float degreesY = std::fmodf(currentCursorY, 360.0f);
		if (degreesY < 0) degreesY += 360.0f;

		// Degrees to radians
		yaw = degreesX * (3.14159f / 180.0f);
		pitch = degreesY * (3.14159f / 180.0f);

		cameraMatrix = RotMatrix(yaw, pitch, 0.0f);
	}
	else if (cameraMode == MainMenuBar::FreeCamMode::Noclip)
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
		cameraPosition += moveDirection * speed;

	if (io.KeysDown['S'])
		cameraPosition -= moveDirection * speed;

	if (io.KeysDown['A'])
		cameraPosition -= moveDirection.CrossProduct(Vec3(0, 0, 1)) * speed;

	if (io.KeysDown['D'])
		cameraPosition += moveDirection.CrossProduct(Vec3(0, 0, 1)) * speed;

	WorldTransform newTransform
	{
		.Position = cameraPosition,
		.Orientation = cameraMatrix,
	};

	if (cameraMode == MainMenuBar::FreeCamMode::Free)
	{
		std::scoped_lock lock(camera->m_DataLock);

		camera->m_PreviousOrientation = newTransform;
		camera->m_Orientation = newTransform;
		camera->m_Flags |= Entity::WorldTransformChanged;
	}
	else if (cameraMode == MainMenuBar::FreeCamMode::Noclip)
	{
		player->m_Entity->m_Mover->MoveToWorldTransform(newTransform, 0.01f, false);
	}
}

LRESULT WINAPI WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	// Intercepted messages will prevent them from being forwarded to imgui and the game
	auto handled = HandleMessage(hWnd, Msg, wParam, lParam);

	if (handled)
		return handled.value();

	ImGui_ImplWin32_WndProcHandler(hWnd, Msg, wParam, lParam);
	return CallWindowProcW(OriginalWndProc, hWnd, Msg, wParam, lParam);
}

}
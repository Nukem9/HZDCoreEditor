#include <d3d12.h>
#include <array>
#include <imgui.h>
#include <imgui_impl_dx12.h>
#include <imgui_impl_win32.h>

#include "../Core/Application.h"
#include "../Core/CursorManager.h"
#include "../Core/PlayerGame.h"
#include "../PGraphics3D/RenderingDeviceDX12.h"
#include "../PGraphics3D/RenderingConfiguration.h"
#include "../PGraphics3D/SwapChainDX12.h"
#include "../PGraphics3D/HwRenderBuffer.h"

#include "DebugUI.h"
#include "DebugUIWindow.h"
#include "MainMenuBar.h"

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

D3D12_CPU_DESCRIPTOR_HANDLE TextureCpuDescriptorIndex;
D3D12_GPU_DESCRIPTOR_HANDLE TextureGpuDescriptorIndex;

D3D12_CPU_DESCRIPTOR_HANDLE cpuDescriptorIndex;
D3D12_GPU_DESCRIPTOR_HANDLE gpuDescriptorIndex;
uint32_t descriptorIncrement;

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

	auto& io = ImGui::GetIO();
	io.MouseDrawCursor = false;

	cpuDescriptorIndex = SrvDescHeap->GetCPUDescriptorHandleForHeapStart();
	gpuDescriptorIndex = SrvDescHeap->GetGPUDescriptorHandleForHeapStart();
	descriptorIncrement = device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);

	ImGui_ImplWin32_Init(SwapChain->m_NativeWindowHandle);
	ImGui_ImplDX12_Init(device, SwapChainDX12::BackBufferCount, SwapChain->m_BackBuffers[0].m_Resource->GetDesc().Format, SrvDescHeap, cpuDescriptorIndex, gpuDescriptorIndex);

	cpuDescriptorIndex.ptr += descriptorIncrement;
	gpuDescriptorIndex.ptr += descriptorIncrement;
	TextureCpuDescriptorIndex = cpuDescriptorIndex;
	TextureGpuDescriptorIndex = gpuDescriptorIndex;

	OriginalWndProc = reinterpret_cast<WNDPROC>(SetWindowLongPtrW(SwapChain->m_NativeWindowHandle, GWLP_WNDPROC, reinterpret_cast<LONG_PTR>(&WndProc)));

	DebugUI::AddWindow(std::make_shared<MainMenuBar>());
}

void RenderUI()
{
	ImGui_ImplDX12_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

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
	/*
	if (TestTexture)
	{
		static bool once = []()
		{
			auto tex = static_cast<TextureDX12 *>(TestTexture.get())->m_Copies[0].m_State.m_D3DResource;

			auto desc = tex->GetDesc();

			D3D12_SHADER_RESOURCE_VIEW_DESC srvDesc{};
			srvDesc.Format = desc.Format;
			srvDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D;
			srvDesc.Texture2D.MipLevels = desc.MipLevels;
			srvDesc.Shader4ComponentMapping = D3D12_ENCODE_SHADER_4_COMPONENT_MAPPING(D3D12_SHADER_COMPONENT_MAPPING_FROM_MEMORY_COMPONENT_0, D3D12_SHADER_COMPONENT_MAPPING_FROM_MEMORY_COMPONENT_1, D3D12_SHADER_COMPONENT_MAPPING_FROM_MEMORY_COMPONENT_2, D3D12_SHADER_COMPONENT_MAPPING_FORCE_VALUE_1);

			RenderingDeviceDX12::Instance().m_Device->CreateShaderResourceView(tex.Get(), &srvDesc, TextureCpuDescriptorIndex);
			return true;
		}();
	}
	*/
	//	RenderingDeviceDX12::Instance().m_Device->CopyDescriptorsSimple(1, TextureCpuDescriptorIndex, static_cast<TextureDX12 *>(TestTexture.get())->m_Copies[0].m_CPUDescriptorHandle, D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);

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

void AddWindow(std::shared_ptr<Window> Handle)
{
	// Immediately discard duplicate window instances
	auto id = Handle->GetId();

	if (!m_Windows.contains(id))
		m_Windows.emplace(id, Handle);
}

LRESULT WINAPI WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	switch (Msg)
	{
	case WM_KEYDOWN:
		if (wParam == VK_OEM_3 || wParam == VK_F12)
		{
			// Toggle input blocking (~ or F12)
			InterceptInput = !InterceptInput;
			auto cursorManager = Application::Instance().m_CursorManager;

			if (InterceptInput)
			{
				ImGui::GetIO().MouseDrawCursor = true;

				cursorManager->m_UnlockCursorBounds = true;
				cursorManager->m_ForceHideCursor = true;

				CallWindowProcW(OriginalWndProc, hWnd, WM_ACTIVATEAPP, 0, 0);
			}
			else
			{
				ImGui::GetIO().MouseDrawCursor = false;

				cursorManager->m_UnlockCursorBounds = false;
				cursorManager->m_ForceHideCursor = false;

				CallWindowProcW(OriginalWndProc, hWnd, WM_ACTIVATEAPP, 1, 0);
			}

			return 0;
		}
		else if (wParam == VK_F5)
		{

		}
		else if (wParam == VK_F6)
		{
			// Load last save
		}
		break;

	case WM_ACTIVATEAPP:
		if (InterceptInput)
		{
			// Prevent alt-tab from interfering with input blocking
			return 0;
		}
		break;
	}

	ImGui_ImplWin32_WndProcHandler(hWnd, Msg, wParam, lParam);
	return CallWindowProcW(OriginalWndProc, hWnd, Msg, wParam, lParam);
}

bool ShouldInterceptInput()
{
	return InterceptInput;
}

}
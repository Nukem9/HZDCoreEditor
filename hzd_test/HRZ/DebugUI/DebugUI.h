#pragma once

#include <Windows.h>
#include <memory>
#include <optional>

#include "DebugUIWindow.h"

namespace HRZ
{
class SwapChainDX12;
}

namespace HRZ::DebugUI
{

void Initialize(const SwapChainDX12 *SwapChain);
void AddWindow(std::shared_ptr<Window> Handle);

void RenderUI();
void RenderUID3D(const SwapChainDX12 *SwapChain);

void ToggleInputInterception();
bool ShouldInterceptInput();
std::optional<LRESULT> HandleMessage(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam);
void UpdateFreecam();

}
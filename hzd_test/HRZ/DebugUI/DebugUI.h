#pragma once

#include <memory>

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

bool ShouldInterceptInput();
void UpdateFreecam();

}
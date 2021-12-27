#include <imgui.h>
#include <map>

#include "../Core/EntityComponent.h"
#include "../Core/Entity.h"
#include "../Core/Player.h"
#include "../Core/FocusComponent.h"
#include "../Core/VertexArrayResource.h"
#include "../PGraphics3D/DataBufferDX12.h"

#include "DebugUI.h"
#include "FocusEditorWindow.h"

namespace HRZ::DebugUI
{

float FocusColorOverride[4];
float FocusColorExponent = 2.2f;

void FocusEditorWindow::Render()
{
	ImGui::SetNextWindowSize(ImVec2(500, 500), ImGuiCond_FirstUseEver);

	if (!ImGui::Begin(GetId().c_str(), &m_WindowOpen))
	{
		ImGui::End();
		return;
	}

	UpdateFocusVertexColors();

	ImGui::ColorPicker4("Color Picker", FocusColorOverride, ImGuiColorEditFlags_AlphaBar);
	ImGui::InputFloat("Color Exponent", &FocusColorExponent, 0.1f, 10.0f, "%.3f");

	ImGui::End();
}

bool FocusEditorWindow::Close()
{
	return !m_WindowOpen;
}

std::string FocusEditorWindow::GetId() const
{
	return "Focus Editor";
}

void FocusEditorWindow::UpdateFocusVertexColors()
{
	auto player = Player::GetLocalPlayer();

	if (!player || !player->m_Entity)
		return;

	std::scoped_lock lock(player->m_Entity->m_DataLock);

	// Grab the StaticModel instance
	auto staticModelComponent = player->m_Entity->m_Components.FindComponentByResourceName("StaticModelResource_HeadMount");

	if (!staticModelComponent)
		return;

	// Iterate over each ModelPartResource (each component for the focus and its variants)
	static GGUUID selectedPartUUID;
	Ref<VertexArrayResource> selectedVertexResource;

	auto& staticModelResource = staticModelComponent->m_Resource;
	auto& staticModelParts = staticModelResource->GetMemberRefUnsafe<Array<Ref<Resource>>>("ModelPartResources");

	for (auto& part : staticModelParts)
	{
		auto& meshResource = part->GetMemberRefUnsafe<Ref<Resource>>("MeshResource");
		auto& primitives = meshResource->GetMemberRefUnsafe<Array<Ref<BaseResource>>>("Primitives");

		char radioLabel[64];
		sprintf_s(radioLabel, "%s, %s##%p", part->GetName().c_str(), meshResource->GetName().c_str(), meshResource.get());

		if (ImGui::RadioButton(radioLabel, selectedPartUUID == part->m_ObjectUUID))
			selectedPartUUID = part->m_ObjectUUID;

		if (selectedPartUUID == part->m_ObjectUUID)
			selectedVertexResource = primitives[0]->GetMemberRefUnsafe<Ref<VertexArrayResource>>("VertexArray");
	}

	if (!selectedVertexResource)
		return;

	// Grab the specific vertex color attribute stream from the focus mesh, then map it for CPU write access
	auto dataBuffer = static_cast<DataBufferDX12 *>(selectedVertexResource->m_VertexArray->m_VertexStreams[1].m_Resource->m_Buffer.get());

	std::map<uint32_t, uint32_t> focusColorRemap;

	if (dataBuffer->m_LoadState == 2)
	{
		uint32_t bufferSize = dataBuffer->Size();
		auto map = dataBuffer->Map(2 | 4 | 8, 0, bufferSize);

		auto floatToByte = [](float Value)
		{
			return static_cast<uint8_t>((Value * 255.0f) + 0.5f);
		};

		// TODO: Keep a copy of the on-disk data. This is equivalent to MAP_DISCARD and some other attributes are being trashed.
		for (uint32_t offset = 0; offset < bufferSize; offset += dataBuffer->Stride())
		{
			auto colors = reinterpret_cast<uint8_t *>(reinterpret_cast<uintptr_t>(map.m_Buffer) + offset + 0xC);

			//focusColorRemap.emplace(*color, *color);

			// An exponent of 2.2 isn't mathematically accurate according to shader code, but it's close enough. Oh well.
			colors[0] = floatToByte(std::powf(FocusColorOverride[0], FocusColorExponent));
			colors[1] = floatToByte(std::powf(FocusColorOverride[1], FocusColorExponent));
			colors[2] = floatToByte(std::powf(FocusColorOverride[2], FocusColorExponent));
			colors[3] = floatToByte(1.0f - FocusColorOverride[3]);
		}

		dataBuffer->Unmap(map);
	}

#if 0
	if (ImGui::BeginListBox("##FocusColorSelector", ImVec2(-FLT_MIN, 250)))
	{
#define UNPACK_COLORS(x) (x & 0xFF), (x >> 8) & 0xFF, (x >> 16) & 0xFF, (x >> 24) & 0xFF
		for (auto [key, value] : focusColorRemap)
		{
			char itemText[512];
			sprintf_s(itemText, "(%u, %u, %u, %u) -> (%u, %u, %u, %u)", UNPACK_COLORS(key), UNPACK_COLORS(value));

			ImGui::Selectable(itemText, false);
		}

		ImGui::EndListBox();
#undef UNPACK_COLORS
	}
#endif
}

}
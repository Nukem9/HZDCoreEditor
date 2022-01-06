#include "HRZ/Core/RTTI.h"
#include "HRZ/Core/RTTIBinaryReader.h"
#include "HRZ/Core/PrefetchList.h"
#include "HRZ/DebugUI/LogWindow.h"
#include "ModConfig.h"
#include "LogHooks.h"

using namespace HRZ;

namespace LogHooks
{

void PackfileDevice_MountArchive(PackFileDevice *Device, const String& BinPath, uint32_t Priority)
{
	Offsets::CallID<"PackfileDevice::MountArchive", decltype(&PackfileDevice_MountArchive)>(Device, BinPath, Priority);

	DebugUI::LogWindow::AddLog("[PackfileDevice] Mounted archive %s with priority %u\n", BinPath.c_str(), Priority);
}

void PreLoadObjectHook(HRZ::RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	Reader->OnObjectPreInit(object);

	if (ModConfiguration.EnableAssetLogging)
	{
		// Subtract 12 for the core entry header length (uint64, uint)
		uint64_t assetOffset = Reader->GetStreamPosition() - 12;

		DebugUI::LogWindow::AddLog("[Asset] Loading %s at [offset %lld, address 0x%p] (%s)\n", object->GetRTTI()->GetSymbolName().c_str(), assetOffset, object, Reader->m_FullFilePath.c_str());
	}
}

void PostLoadObjectHook(RTTIBinaryReader *Reader)
{
	auto object = Reader->m_CurrentObject;
	Reader->OnObjectPostInit(object);

	if (ModConfiguration.EnableAssetLogging)
	{
		DebugUI::LogWindow::AddLog("[Asset] Finished loading %s at [offset %lld, address 0x%p] (%s)", object->GetRTTI()->GetSymbolName().c_str(), Reader->GetStreamPosition(), object, Reader->m_FullFilePath.c_str());

		// Dump the name
		if (String assetName; object->GetMemberValue("Name", &assetName) && !assetName.empty())
			DebugUI::LogWindow::AddLog(" (%s)\n", assetName.c_str());
		else
			DebugUI::LogWindow::AddLog("\n");

		// Log prefetch values for debugging purposes
		if (auto prefetch = RTTI::Cast<PrefetchList>(object); prefetch)
		{
			DebugUI::LogWindow::AddLog("[Asset] Prefetch load done\n");
			DebugUI::LogWindow::AddLog("[Asset] Total files: %d\n", prefetch->m_Files.size());
			DebugUI::LogWindow::AddLog("[Asset] Total sizes: %d\n", prefetch->m_Sizes.size());
			DebugUI::LogWindow::AddLog("[Asset] Total links: %d\n", prefetch->m_Links.size());
		}
	}
}

void NodeGraphAlert(const char *Message, bool Unknown)
{
	DebugUI::LogWindow::AddLog("[Alert] %s\n", Message);
}

void NodeGraphAlertWithName(const char *Category, const char *Severity, const char *Message, bool Unknown)
{
	DebugUI::LogWindow::AddLog("[Alert] [%s] [%s] %s\n", Category, Severity, Message);
}

void NodeGraphTrace(const char *UUID, const char *Message)
{
	DebugUI::LogWindow::AddLog("[Trace] [%s] %s\n", UUID, Message);
}

FILE *hk___acrt_iob_func(uint32_t Ix)
{
	const static auto iobfuncPtr = reinterpret_cast<decltype(&hk___acrt_iob_func)>(GetProcAddress(GetModuleHandleA("api-ms-win-crt-stdio-l1-1-0.dll"), "__acrt_iob_func"));

	return iobfuncPtr(Ix);
}

size_t hk_fwrite(const void *Buffer, size_t ElementSize, size_t ElementCount, FILE *Stream)
{
	const static auto fwritePtr = reinterpret_cast<decltype(&hk_fwrite)>(GetProcAddress(GetModuleHandleA("api-ms-win-crt-stdio-l1-1-0.dll"), "fwrite"));

	if (ElementCount > 0 && Stream == hk___acrt_iob_func(1))
	{
		const size_t len = ElementCount * ElementSize;
		DebugUI::LogWindow::AddLog("[Engine] %.*s", len, Buffer);

		if (len > 1 && static_cast<const char *>(Buffer)[len - 1] != '\n')
			DebugUI::LogWindow::AddLog("\n");

		return ElementCount;
	}

	return fwritePtr(Buffer, ElementSize, ElementCount, Stream);
}

int hk___stdio_common_vfprintf(void *Options, FILE *Stream, const char *Format, _locale_t Locale, va_list ArgList)
{
	const static auto vfprintfPtr = reinterpret_cast<decltype(&hk___stdio_common_vfprintf)>(GetProcAddress(GetModuleHandleA("api-ms-win-crt-stdio-l1-1-0.dll"), "__stdio_common_vfprintf"));

	if (Stream == hk___acrt_iob_func(1))
	{
		char buffer[2048];
		const int len = _vsnprintf_s(buffer, _TRUNCATE, Format, ArgList);

		hk_fwrite(buffer, sizeof(char), len, Stream);
		return len;
	}

	return vfprintfPtr(Options, Stream, Format, Locale, ArgList);
}

}
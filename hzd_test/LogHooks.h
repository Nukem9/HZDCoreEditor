#pragma once

#include <stdio.h>

#include "HRZ/PCore/Common.h"

namespace HRZ
{
class PackFileDevice;
class RTTIBinaryReader;
}

namespace LogHooks
{

void PackfileDevice_MountArchive(HRZ::PackFileDevice *Device, const HRZ::String& BinPath, uint32_t Priority);
void PreLoadObjectHook(HRZ::RTTIBinaryReader *Reader);
void PostLoadObjectHook(HRZ::RTTIBinaryReader *Reader);
void NodeGraphAlert(const char *Message, bool Unknown);
void NodeGraphAlertWithName(const char *Category, const char *Severity, const char *Message, bool Unknown);
void NodeGraphTrace(const char *UUID, const char *Message);

FILE *hk___acrt_iob_func(uint32_t Ix);
size_t hk_fwrite(const void *Buffer, size_t ElementSize, size_t ElementCount, FILE *Stream);
int hk___stdio_common_vfprintf(void *Options, FILE *Stream, const char *Format, _locale_t Locale, va_list ArgList);

}
#pragma once

#define HORIZON_ZERO_DAWN 1
#define DEATH_STRANDING 0

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <stddef.h>
#include <vector>
#include <algorithm>
#include <string>
#include <unordered_set>
#include <unordered_map>

#include <detours/Detours.h>
#pragma comment(lib, "detours.lib")

#include "xutil.h"

extern uintptr_t g_ModuleBase;
extern uintptr_t g_ModuleSize;

extern uintptr_t g_CodeBase;	// .text or .textbss
extern uintptr_t g_CodeEnd;
extern uintptr_t g_RdataBase;	// .rdata
extern uintptr_t g_RdataEnd;
extern uintptr_t g_DataBase;	// .data
extern uintptr_t g_DataEnd;
#include "common.h"

uintptr_t g_ModuleBase;
uintptr_t g_ModuleSize;

uintptr_t g_CodeBase;	// .text or .textbss
uintptr_t g_CodeEnd;
uintptr_t g_RdataBase;	// .rdata
uintptr_t g_RdataEnd;
uintptr_t g_DataBase;	// .data
uintptr_t g_DataEnd;

GameType g_GameType;
char g_GamePreix[64];
std::unordered_map<std::string, uintptr_t> g_OffsetMap;
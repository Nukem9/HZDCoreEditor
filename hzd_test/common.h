#pragma once

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

enum class GameType
{
	DeathStranding = 0,
	HorizonZeroDawn = 1,
};

extern GameType g_GameType;
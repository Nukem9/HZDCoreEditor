#pragma once

#include <unordered_set>

#include "HRZ/Core/RTTI.h"

namespace RTTIScanner
{

const std::unordered_set<const HRZ::RTTI *>& GetAllTypes();
void ExportAll(std::string_view Directory);
void ScanForRTTIStructures();
void RegisterRTTIStructures();

}
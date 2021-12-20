#pragma once

#include <unordered_set>

#include "../HRZ/Core/RTTI.h"

namespace RTTIScanner
{

const std::unordered_set<const HRZ::RTTI *>& GetAllTypes();
const HRZ::RTTI *GetTypeByName(const std::string_view Name);
void ExportAll(const std::string_view Directory, const std::string_view GameTypePrefix);
void ScanForRTTIStructures();
void RegisterRTTIStructures();

}
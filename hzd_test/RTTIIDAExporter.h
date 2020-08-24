#pragma once

#include "common.h"
#include "RTTI.h"

namespace RTTIIDAExporter
{
	void ExportAll(const char *Directory);
	void ExportMSRTTI(FILE *F);
	void ExportGGRTTI(FILE *F);
	void ExportGameSymbolRTTI(FILE *F);
}
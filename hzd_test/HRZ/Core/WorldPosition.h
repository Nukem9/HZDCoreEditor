#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class WorldPosition final
{
public:
	union
	{
		struct
		{
			double X;
			double Y;
			double Z;
		};

		double Data[3]{};
	};
};
assert_size(WorldPosition, 0x18);

}
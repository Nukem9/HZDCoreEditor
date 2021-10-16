#pragma once

#include "../PCore/Util.h"

#include "Vec3.h"

namespace HRZ
{

class BoundingBox3 final
{
public:
	Vec3 Min;
	Vec3 Max;
};
assert_size(BoundingBox3, 0x20);

}
#pragma once

#include "../PCore/Util.h"

#include "Vec3.h"

namespace HRZ
{

class RotMatrix final
{
public:
	Vec3Pack Col0;
	Vec3Pack Col1;
	Vec3Pack Col2;
};
assert_size(RotMatrix, 0x24);

}
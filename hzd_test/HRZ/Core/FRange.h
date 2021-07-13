#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class FRange final
{
public:
	float Min;
	float Max;
};
assert_size(FRange, 0x8);

}
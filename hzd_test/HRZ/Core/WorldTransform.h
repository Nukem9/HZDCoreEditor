#pragma once

#include "../PCore/Util.h"

#include "WorldPosition.h"
#include "RotMatrix.h"

namespace HRZ
{

class WorldTransform final
{
public:
	WorldPosition Position;
	RotMatrix Orientation;
};
assert_size(WorldTransform, 0x40);

}
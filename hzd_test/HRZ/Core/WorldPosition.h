#pragma once

#include "../PCore/Util.h"

#include "Vec3.h"

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

	WorldPosition()
	{
	}

	WorldPosition(double aX, double aY, double aZ) : X(aX), Y(aY), Z(aZ)
	{
	}

	WorldPosition& operator +=(const Vec3& Other)
	{
		X += Other.X;
		Y += Other.Y;
		Z += Other.Z;

		return *this;
	}

	WorldPosition& operator -=(const Vec3& Other)
	{
		X -= Other.X;
		Y -= Other.Y;
		Z -= Other.Z;

		return *this;
	}
};
assert_size(WorldPosition, 0x18);

}
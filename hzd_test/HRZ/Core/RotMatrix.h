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

	RotMatrix()
	{
	}

	RotMatrix(float Yaw, float Pitch, float Roll)
	{
		float cosX = cosf(Pitch);	// X or Pitch
		float sinX = sinf(Pitch);

		float cosY = cosf(Yaw);		// Y or Yaw
		float sinY = sinf(Yaw);

		float cosZ = cosf(Roll);	// Z or Roll
		float sinZ = sinf(Roll);

		Col0.X = cosZ * cosY + sinZ * sinX * sinY;
		Col0.Y = cosY * sinZ * sinX - sinY * cosZ;
		Col0.Z = cosX * sinZ;

		Col1.X = cosX * sinY;
		Col1.Y = cosY * cosX;
		Col1.Z = -sinX;

		Col2.X = sinY * cosZ * sinX - sinZ * cosY;
		Col2.Y = sinZ * sinY + cosY * cosZ * sinX;
		Col2.Z = cosX * cosZ;
	}

	void Decompose(float *Yaw, float *Pitch, float *Roll)
	{
		float y;
		float p = std::asinf(-Col1.Z);
		float r;

		if (std::cosf(p) > 0.00001f)
		{
			y = std::atan2f(Col1.X, Col1.Y);
			r = std::atan2f(Col0.Z, Col2.Z);
		}
		else
		{
			y = std::atan2f(-Col0.Y, Col0.X);
			r = 0.0f;
		}

		if (Yaw)
			*Yaw = y;

		if (Pitch)
			*Pitch = p;

		if (Roll)
			*Roll = r;
	}
};
assert_size(RotMatrix, 0x24);

}
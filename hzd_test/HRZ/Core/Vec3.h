#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class Vec3 final
{
public:
	union
	{
		struct  
		{
			float X;
			float Y;
			float Z;
		};

		float Data[4]{};
	};
};
assert_size(Vec3, 0x10);

class Vec3Pack final
{
public:
	float X = 0.0f;
	float Y = 0.0f;
	float Z = 0.0f;
};
assert_size(Vec3Pack, 0xC);

}
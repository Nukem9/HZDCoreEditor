#pragma once

#include <xmmintrin.h>

#include "../PCore/Util.h"

namespace HRZ
{

class alignas(16) Vec3 final
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
		__m128 XMVec;
	};

	Vec3()
	{
	}

	Vec3(float aX, float aY, float aZ) : X(aX), Y(aY), Z(aZ)
	{
	}

	Vec3(__m128 Vector)
	{
		_mm_store_ps(Data, Vector);
	}

	Vec3 CrossProduct(const Vec3& Other)
	{
		auto a = XMVec;
		auto b = Other.XMVec;

		auto aShuffle = _mm_shuffle_ps(a, a, _MM_SHUFFLE(3, 0, 2, 1));
		auto bShuffle = _mm_shuffle_ps(b, b, _MM_SHUFFLE(3, 0, 2, 1));
		auto c = _mm_sub_ps(_mm_mul_ps(a, bShuffle), _mm_mul_ps(aShuffle, b));

		return _mm_shuffle_ps(c, c, _MM_SHUFFLE(3, 0, 2, 1));
	}

	Vec3 operator *(float Scale)
	{
		return _mm_mul_ps(XMVec, _mm_load1_ps(&Scale));
	}

};
assert_size(Vec3, 0x10);

class alignas(4) Vec3Pack final
{
public:
	float X = 0.0f;
	float Y = 0.0f;
	float Z = 0.0f;
};
assert_size(Vec3Pack, 0xC);

}
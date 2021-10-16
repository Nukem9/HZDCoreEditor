#pragma once

#include <cstddef>

namespace HRZ
{

struct GGUUID final
{
	std::byte Data[16]{};
};

}
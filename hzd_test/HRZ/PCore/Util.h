#pragma once

#include <type_traits>

namespace HRZ
{

template<size_t A, size_t B, typename = std::enable_if_t<A == B>>
struct is_offset_equal : std::true_type {};

template<size_t A, size_t B>
inline constexpr bool is_offset_equal_v = is_offset_equal<A, B>::value;

#define assert_offset(Structure, Member, Offset) static_assert(HRZ::is_offset_equal_v<offsetof(Structure, Member), (Offset)>, "Structure member offset is unexpected");
#define assert_size(Structure, Size) static_assert(HRZ::is_offset_equal_v<sizeof(Structure), (Size)>, "Structure size is unexpected");

}
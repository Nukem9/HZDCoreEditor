#pragma once

#include <objbase.h>
#include <cstddef>
#include <array>

namespace HRZ
{

struct GGUUID final
{
	union
	{
		struct
		{
			uint32_t Data1;
			uint16_t Data2;
			uint16_t Data3;
			uint8_t Data4[8];
		};

		struct
		{
			std::array<uint8_t, 16> All;
		};
	};

	constexpr bool operator==(const GGUUID& Other) const
	{
		return All == Other.All;
	}

	constexpr bool operator!=(const GGUUID& Other) const
	{
		return All != Other.All;
	}

	static GGUUID Generate()
	{
		static_assert(sizeof(GUID) == sizeof(GGUUID));

		GGUUID id {};
		CoCreateGuid(reinterpret_cast<GUID *>(&id));

		return id;
	}

	template<typename T, size_t Digits = sizeof(T) * 2>
	constexpr static uint32_t UUIDHexToBytes(const char *Hex)
	{
		auto charToByte = [](char C) constexpr -> uint32_t
		{
			if (C >= 'A' && C <= 'F')
				return C - 'A' + 10;
			else if (C >= 'a' && C <= 'f')
				return C - 'a' + 10;
			else if (C >= '0' && C <= '9')
				return C - '0';

			throw "Invalid hexadecimal digit";
		};

		T value {};

		for (size_t i = 0; i < Digits; i++)
			value |= charToByte(Hex[i]) << (4 * (Digits - i - 1));

		return value;
	}

	template<size_t N, size_t Len = N - 1>
	consteval static GGUUID Parse(const char(&UUID)[N])
	{
		//
		// Parse as:
		// 40e36691-5fd0-4a79-b3b3-87b2a3d13e9c
		// 40E36691-5FD0-4A79-B3B3-87B2A3D13E9C
		// {40E36691-5FD0-4A79-B3B3-87B2A3D13E9C}
		//
		static_assert(Len == 36 || Len == 38, "UUIDs are expected to be 36 or 38 characters long with dashes included. Brackets are optional.");
		const size_t add = (Len == 38) ? 1 : 0;

		if (add && (UUID[0] != '{' || UUID[Len - 1] != '}'))
			throw "Invalid bracket pair used";

		GGUUID id {};
		id.Data1 = UUIDHexToBytes<uint32_t>(UUID + 0 + add);
		id.Data2 = UUIDHexToBytes<uint16_t>(UUID + 9 + add);
		id.Data3 = UUIDHexToBytes<uint16_t>(UUID + 14 + add);
		id.Data4[0] = UUIDHexToBytes<uint8_t>(UUID + 19 + add);
		id.Data4[1] = UUIDHexToBytes<uint8_t>(UUID + 21 + add);

		for (int i = 0; i < 6; i++)
			id.Data4[i + 2] = UUIDHexToBytes<uint8_t>(UUID + 24 + (i * 2) + add);

		return id;
	}
};

}
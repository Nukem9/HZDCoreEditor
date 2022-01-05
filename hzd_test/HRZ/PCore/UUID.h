#pragma once

#include <objbase.h>
#include <cstddef>
#include <array>
#include <string_view>
#include <stdexcept>
#include <optional>

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

			throw std::invalid_argument("Invalid hexadecimal digit");
		};

		T value{};

		for (size_t i = 0; i < Digits; i++)
			value |= charToByte(Hex[i]) << (4 * (Digits - i - 1));

		return value;
	}

	static GGUUID Generate()
	{
		static_assert(sizeof(GUID) == sizeof(GGUUID));

		GGUUID id {};
		CoCreateGuid(reinterpret_cast<GUID *>(&id));

		return id;
	}

	constexpr static GGUUID Parse(const std::string_view UUID)
	{
		return Parse(UUID.data(), UUID.length());
	}

	template<size_t N, size_t Len = N - 1>
	consteval static GGUUID Parse(const char(&UUID)[N])
	{
		static_assert(Len == 36 || Len == 38, "UUIDs are expected to be 36 or 38 characters long with dashes included. Brackets are optional.");
		
		return Parse(UUID, Len);
	}

	constexpr static GGUUID Parse(const char *UUID, size_t Length)
	{
		//
		// Parse as:
		// 40e36691-5fd0-4a79-b3b3-87b2a3d13e9c
		// 40E36691-5FD0-4A79-B3B3-87B2A3D13E9C
		// {40E36691-5FD0-4A79-B3B3-87B2A3D13E9C}
		//
		const size_t add = (Length == 38) ? 1 : 0;

		if (Length != 36 && Length != 38)
			throw std::invalid_argument("Invalid GUID length specified");

		if (add && (UUID[0] != '{' || UUID[Length - 1] != '}'))
			throw std::invalid_argument("Invalid bracket pair used");

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

	constexpr static std::optional<GGUUID> TryParse(const std::string_view UUID)
	{
		try
		{
			return Parse(UUID);
		}
		catch (std::exception&)
		{
		}

		return std::nullopt;
	}
};

}

namespace std
{

template<>
struct hash<HRZ::GGUUID>
{
	constexpr size_t operator()(const HRZ::GGUUID& Key) const
	{
		size_t hash = 0x100000001B3ull;

		for (auto k : Key.All)
		{
			hash ^= k;
			hash *= 0xCBF29CE484222325ull;
		}

		return hash;
	}
};

}
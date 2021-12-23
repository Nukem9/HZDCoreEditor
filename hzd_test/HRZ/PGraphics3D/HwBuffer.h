#pragma once

#include "../../Offsets.h"
#include "../PCore/Util.h"

#include "HwResource.h"
#include "IRenderDataCallbackObject.h"

namespace HRZ
{

enum EDataBufferFormat : uint32_t
{
	Invalid = 0,
	R_FLOAT_16 = 1,
	R_FLOAT_32 = 2,
	RG_FLOAT_32 = 3,
	RGB_FLOAT_32 = 4,
	RGBA_FLOAT_32 = 5,
	R_UINT_8 = 6,
	R_UINT_16 = 7,
	R_UINT_32 = 8,
	RG_UINT_32 = 9,
	RGB_UINT_32 = 10,
	RGBA_UINT_32 = 11,
	R_INT_32 = 12,
	RG_INT_32 = 13,
	RGB_INT_32 = 14,
	RGBA_INT_32 = 15,
	R_UNORM_8 = 16,
	R_UNORM_16 = 17,
	RGBA_UNORM_8 = 18,
	RGBA_UINT_8 = 19,
	RG_UINT_16 = 20,
	RGBA_UINT_16 = 21,
	RGBA_INT_8 = 22,
	Structured = 23,
};

class HwBuffer : public HwResource, public IRenderDataCallbackObject
{
public:
	struct MapData
	{
		void *m_Buffer = nullptr;
		uint32_t m_StartOffset = 0;
		uint32_t m_Length = 0;
		uint32_t m_LengthInElements = 0;
		EDataBufferFormat m_Format = EDataBufferFormat::Invalid;
		uint32_t m_MapFlags = 0;
	};
	assert_size(MapData, 0x20);

	virtual ~HwBuffer() override;															// 0
	virtual bool HwBufferUnknown02(uint32_t LoadType);										// 2
	virtual void LoadBufferData();															// 3
	virtual void InitBufferData(void *BinaryReaderStream);									// 4
	virtual bool CreateGPUResources() = 0;													// 5
	virtual void DestroyGPUResources() = 0;													// 6
	virtual void *MapRange(uint8_t MapFlags, uint32_t StartOffset, uint32_t Length) = 0;	// 7
	virtual void UnmapRange(void *Unknown1, void *Unknown2) = 0;							// 8

	virtual void IRenderDataCallbackObjectUnknown01() override;	// 1
	virtual void IRenderDataCallbackObjectUnknown02() override;	// 2

	uint32_t Stride() const
	{
		if (m_Format == EDataBufferFormat::Structured)
			return m_StructuredByteStride;

		__debugbreak();// return GetFormatElementSize(m_Format);
		return 0;
	}

	uint32_t Size() const
	{
		return m_ElementCount * Stride();
	}

	MapData Map(uint32_t MapFlags, uint32_t StartOffset, uint32_t Length)
	{
		MapData data;
		Offsets::Call<0x1FE310, void(*)(HwBuffer *, uint8_t, MapData&, uint32_t, uint32_t)>(this, MapFlags, data, StartOffset, Length);

		return data;
	}

	void Unmap(MapData& Data)
	{
		Offsets::Call<0x21D900, void(*)(HwBuffer *, MapData&)>(this, Data);
	}

	uint32_t m_Flags;					// 0x28
	EDataBufferFormat m_Format;			// 0x2C
	uint32_t m_ElementCount;			// 0x30
	uint32_t m_StructuredByteStride;	// 0x34
	uint32_t m_Unknown38;				// 0x38 Initialized to -1
	bool m_IsDataStreamed;				// 0x3C
	char _pad40[0x40];					// 0x38
};
assert_offset(HwBuffer, m_Flags, 0x28);
assert_size(HwBuffer, 0x80);

}
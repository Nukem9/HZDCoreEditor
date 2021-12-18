#pragma once

#include <d3d12.h>

#include "../PCore/Common.h"

#include "HwBuffer.h"

namespace HRZ
{

class HwVertexStreamBufferResource : public HwReferencableBase<HwVertexStreamBufferResource>
{
public:
	Ref<HwBuffer> m_Buffer;
};
assert_size(HwVertexStreamBufferResource, 0x10);

class HwVertexArray : public HwReferencableBase<HwVertexArray>
{
public:
	struct VertexStreamResource
	{
		uint32_t m_TypeFlags;							// 0x0 Bit flags based on VertexElementDesc contents
		Ref<HwVertexStreamBufferResource> m_Resource;	// 0x8
		GGUUID m_VertexDataHash;						// 0x10 Actually MurMurHashValue
		void *m_VertexElementLayout;					// 0x20 Points to an array of VertexElementDesc (cached object pool)
	};
	assert_size(VertexStreamResource, 0x28);

	char _pad4[0x144];							// 0x4
	uint32_t m_VertexLayoutCRC;					// 0x148
	VertexStreamResource m_VertexStreams[27];	// 0x150 Corresponds to max enum value in EVertexElement
	char _pad588[0x8];							// 0x588
	uint32_t m_VertexCount;						// 0x590
	uint32_t m_StreamCount;						// 0x594
	bool m_IsDataStreamed;						// 0x598
};
assert_offset(HwVertexArray, m_VertexLayoutCRC, 0x148);
assert_offset(HwVertexArray, m_VertexCount, 0x590);
assert_offset(HwVertexArray, m_IsDataStreamed, 0x598);
assert_size(HwVertexArray, 0x5A0);

}
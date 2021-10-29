#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class RTTIObject;

class RTTIBinaryReader
{
public:
	char _pad8[0x8];				// 0x8
	uint64_t m_BytesPendingRead;	// 0x10
	uint64_t m_TotalBytesAvailable;	// 0x18
	uint8_t *m_DataBufferStart;		// 0x20
	uint8_t *m_DataBufferEnd;		// 0x28
	void *m_StreamHandle;			// 0x30
	RTTIObject *m_CurrentObject;	// 0x38
	char _pad40[0x8];				// 0x40
	String m_PartialFilePath;		// 0x48
	String m_FullFilePath;			// 0x50
	char _pad58[0x28];				// 0x58

	virtual ~RTTIBinaryReader();						// 0
	virtual void OnObjectPreInit(RTTIObject *Object);	// 1
	virtual void OnObjectPostInit(RTTIObject *Object);	// 2
	virtual void RTTIBinaryReaderUnknown03();			// 3
	virtual int RTTIBinaryReaderUnknown04();			// 4

	uint64_t GetStreamPosition() const
	{
		return m_TotalBytesAvailable - ((m_DataBufferEnd - m_DataBufferStart) + m_BytesPendingRead);
	}
};
assert_offset(RTTIBinaryReader, m_BytesPendingRead, 0x10);
assert_offset(RTTIBinaryReader, m_DataBufferEnd, 0x28);
assert_offset(RTTIBinaryReader, m_CurrentObject, 0x38);
assert_offset(RTTIBinaryReader, m_PartialFilePath, 0x48);
assert_offset(RTTIBinaryReader, m_FullFilePath, 0x50);
assert_size(RTTIBinaryReader, 0x80);

}
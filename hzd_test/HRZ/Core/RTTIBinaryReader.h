#pragma once

#include "../PCore/Common.h"

namespace HRZ
{

class RTTIObject;

class RTTIBinaryReader
{
public:
	char _pad8[0x18];				// 0x8
	uint64_t m_StreamOffset;		// 0x20
	uint64_t m_StreamEnd;			// 0x28
	char _pad30[0x8];				// 0x30
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
};
assert_offset(RTTIBinaryReader, m_StreamOffset, 0x20);
assert_offset(RTTIBinaryReader, m_StreamEnd, 0x28);
assert_offset(RTTIBinaryReader, m_CurrentObject, 0x38);
assert_offset(RTTIBinaryReader, m_PartialFilePath, 0x48);
assert_offset(RTTIBinaryReader, m_FullFilePath, 0x50);
assert_size(RTTIBinaryReader, 0x80);

}
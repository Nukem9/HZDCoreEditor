#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"

namespace HRZ
{

DECL_RTTI(AssetPath);
DECL_RTTI(PrefetchList);

class AssetPath
{
public:
	TYPE_RTTI(AssetPath);

	String m_Path;
};
assert_size(AssetPath, 0x8);

class PrefetchList : public CoreObject
{
public:
	TYPE_RTTI(PrefetchList);

	Array<AssetPath> m_Files;	// 0x20
	Array<int> m_Sizes;			// 0x30
	Array<int> m_Links;			// 0x40

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~PrefetchList() override;							// 1
};
assert_size(PrefetchList, 0x50);

}
#pragma once

#include "../PCore/Common.h"

#include "CoreObject.h"
#include "WorldTransform.h"

namespace HRZ
{

DECL_RTTI(WorldNode);

class WorldNode : public CoreObject
{
public:
	TYPE_RTTI(WorldNode);

	WorldTransform m_Transform;	// 0x20

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~WorldNode() override;					// 1
	virtual void WorldNodeUnknown16();				// 16
	virtual void WorldNodeUnknown17();				// 17
};
assert_offset(WorldNode, m_Transform, 0x20);
assert_size(WorldNode, 0x60);

}
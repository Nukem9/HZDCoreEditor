#pragma once

#include "RTTIObject.h"

namespace HRZ
{

DECL_RTTI(NetReplicatedObject);

class NetReplicatedObject : public RTTIObject
{
public:
	TYPE_RTTI(NetReplicatedObject);

	char _pad8[0x38];

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~NetReplicatedObject() override;		// 1
	virtual void NetReplicatedObjectUnknown02();	// 2
	virtual void NetReplicatedObjectUnknown03();	// 3
	virtual void NetReplicatedObjectUnknown04();	// 4
	virtual void NetReplicatedObjectUnknown05();	// 5
	virtual void NetReplicatedObjectUnknown06();	// 6
	virtual void NetReplicatedObjectUnknown07();	// 7
	virtual void NetReplicatedObjectUnknown08();	// 8
	virtual bool NetReplicatedObjectUnknown09();	// 9
	virtual void NetReplicatedObjectUnknown10();	// 10
};
assert_size(NetReplicatedObject, 0x40);

}
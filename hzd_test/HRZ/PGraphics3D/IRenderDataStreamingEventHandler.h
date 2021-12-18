#pragma once

#include "../PCore/Util.h"

namespace HRZ
{

class IRenderDataStreamingEventHandler
{
public:
	virtual void IRenderDataStreamingEventHandlerUnknown00() = 0;	// 0
	virtual void IRenderDataStreamingEventHandlerUnknown01() = 0;	// 1
	virtual void IRenderDataStreamingEventHandlerUnknown02() = 0;	// 2
	virtual void IRenderDataStreamingEventHandlerUnknown03() = 0;	// 3
	virtual void IRenderDataStreamingEventHandlerUnknown04() = 0;	// 4
};
assert_size(IRenderDataStreamingEventHandler, 0x8);

}
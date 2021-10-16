#pragma once

namespace HRZ::DebugUI
{

class Window
{
public:
	virtual void Render() = 0;
	virtual bool Close() = 0;
};

}
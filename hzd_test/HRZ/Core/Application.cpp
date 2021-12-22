#include "GameModule.h"
#include "Player.h"
#include "Application.h"

namespace HRZ
{

Application& Application::Instance()
{
	return *Offsets::ResolveID<"Application::Instance", Application *>();
}

bool Application::IsInGame()
{
	return Instance().m_GameModule && Player::GetLocalPlayer();
}

}
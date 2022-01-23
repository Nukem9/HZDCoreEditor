#include "GameModule.h"
#include "Player.h"
#include "Application.h"

namespace HRZ
{

RecursiveLock m_MainThreadCallbackLock;
std::vector<std::function<void()>> m_MainThreadCallbacks;

Application& Application::Instance()
{
	return *Offsets::ResolveID<"Application::Instance", Application *>();
}

bool Application::IsInGame()
{
	return Instance().m_GameModule && Player::GetLocalPlayer();
}

void Application::RunOnMainThread(std::function<void()> Callback)
{
	m_MainThreadCallbackLock.lock();
	m_MainThreadCallbacks.emplace_back(std::move(Callback));
	m_MainThreadCallbackLock.unlock();
}

void Application::RunMainThreadCallbacks()
{
	m_MainThreadCallbackLock.lock();
	for (auto callbacks = std::move(m_MainThreadCallbacks); auto& c : callbacks)
		c();
	m_MainThreadCallbackLock.unlock();
}

}
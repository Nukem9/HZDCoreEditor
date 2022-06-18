#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdlib.h>
#include <stdio.h>
#include "HRZ/DebugUI/LogWindow.h"

// winhttp dll exports for HZD
// only the functions that are used in the game are fully done manually with proper typedefs and other are generated automatically with IDA

#pragma comment(linker, "/export:DllCanUnloadNow=DllCanUnloadNow")
#pragma comment(linker, "/export:DllGetClassObject=DllGetClassObject")
#pragma comment(linker, "/export:Private1=Private1")
#pragma comment(linker, "/export:SvchostPushServiceGlobals=SvchostPushServiceGlobals")
#pragma comment(linker, "/export:WinHttpAddRequestHeaders=WinHttpAddRequestHeaders")
#pragma comment(linker, "/export:WinHttpAutoProxySvcMain=WinHttpAutoProxySvcMain")
#pragma comment(linker, "/export:WinHttpCheckPlatform=WinHttpCheckPlatform")
#pragma comment(linker, "/export:WinHttpCloseHandle=WinHttpCloseHandle")
#pragma comment(linker, "/export:WinHttpConnect=WinHttpConnect")
#pragma comment(linker, "/export:WinHttpConnectionDeletePolicyEntries=WinHttpConnectionDeletePolicyEntries")
#pragma comment(linker, "/export:WinHttpConnectionDeleteProxyInfo=WinHttpConnectionDeleteProxyInfo")
#pragma comment(linker, "/export:WinHttpConnectionFreeNameList=WinHttpConnectionFreeNameList")
#pragma comment(linker, "/export:WinHttpConnectionFreeProxyInfo=WinHttpConnectionFreeProxyInfo")
#pragma comment(linker, "/export:WinHttpConnectionFreeProxyList=WinHttpConnectionFreeProxyList")
#pragma comment(linker, "/export:WinHttpConnectionGetNameList=WinHttpConnectionGetNameList")
#pragma comment(linker, "/export:WinHttpConnectionGetProxyInfo=WinHttpConnectionGetProxyInfo")
#pragma comment(linker, "/export:WinHttpConnectionGetProxyList=WinHttpConnectionGetProxyList")
#pragma comment(linker, "/export:WinHttpConnectionSetPolicyEntries=WinHttpConnectionSetPolicyEntries")
#pragma comment(linker, "/export:WinHttpConnectionSetProxyInfo=WinHttpConnectionSetProxyInfo")
#pragma comment(linker, "/export:WinHttpConnectionUpdateIfIndexTable=WinHttpConnectionUpdateIfIndexTable")
#pragma comment(linker, "/export:WinHttpCrackUrl=WinHttpCrackUrl")
#pragma comment(linker, "/export:WinHttpCreateProxyResolver=WinHttpCreateProxyResolver")
#pragma comment(linker, "/export:WinHttpCreateUrl=WinHttpCreateUrl")
#pragma comment(linker, "/export:WinHttpDetectAutoProxyConfigUrl=WinHttpDetectAutoProxyConfigUrl")
#pragma comment(linker, "/export:WinHttpFreeProxyResult=WinHttpFreeProxyResult")
#pragma comment(linker, "/export:WinHttpFreeProxyResultEx=WinHttpFreeProxyResultEx")
#pragma comment(linker, "/export:WinHttpFreeProxySettings=WinHttpFreeProxySettings")
#pragma comment(linker, "/export:WinHttpGetDefaultProxyConfiguration=WinHttpGetDefaultProxyConfiguration")
#pragma comment(linker, "/export:WinHttpGetIEProxyConfigForCurrentUser=WinHttpGetIEProxyConfigForCurrentUser")
#pragma comment(linker, "/export:WinHttpGetProxyForUrl=WinHttpGetProxyForUrl")
#pragma comment(linker, "/export:WinHttpGetProxyForUrlEx=WinHttpGetProxyForUrlEx")
#pragma comment(linker, "/export:WinHttpGetProxyForUrlEx2=WinHttpGetProxyForUrlEx2")
#pragma comment(linker, "/export:WinHttpGetProxyForUrlHvsi=WinHttpGetProxyForUrlHvsi")
#pragma comment(linker, "/export:WinHttpGetProxyResult=WinHttpGetProxyResult")
#pragma comment(linker, "/export:WinHttpGetProxyResultEx=WinHttpGetProxyResultEx")
#pragma comment(linker, "/export:WinHttpGetProxySettingsVersion=WinHttpGetProxySettingsVersion")
#pragma comment(linker, "/export:WinHttpGetTunnelSocket=WinHttpGetTunnelSocket")
#pragma comment(linker, "/export:WinHttpOpen=WinHttpOpen")
#pragma comment(linker, "/export:WinHttpOpenRequest=WinHttpOpenRequest")
#pragma comment(linker, "/export:WinHttpProbeConnectivity=WinHttpProbeConnectivity")
#pragma comment(linker, "/export:WinHttpQueryAuthSchemes=WinHttpQueryAuthSchemes")
#pragma comment(linker, "/export:WinHttpQueryDataAvailable=WinHttpQueryDataAvailable")
#pragma comment(linker, "/export:WinHttpQueryHeaders=WinHttpQueryHeaders")
#pragma comment(linker, "/export:WinHttpQueryOption=WinHttpQueryOption")
#pragma comment(linker, "/export:WinHttpReadData=WinHttpReadData")
#pragma comment(linker, "/export:WinHttpReadProxySettings=WinHttpReadProxySettings")
#pragma comment(linker, "/export:WinHttpReadProxySettingsHvsi=WinHttpReadProxySettingsHvsi")
#pragma comment(linker, "/export:WinHttpReceiveResponse=WinHttpReceiveResponse")
#pragma comment(linker, "/export:WinHttpResetAutoProxy=WinHttpResetAutoProxy")
#pragma comment(linker, "/export:WinHttpSaveProxyCredentials=WinHttpSaveProxyCredentials")
#pragma comment(linker, "/export:WinHttpSendRequest=WinHttpSendRequest")
#pragma comment(linker, "/export:WinHttpSetCredentials=WinHttpSetCredentials")
#pragma comment(linker, "/export:WinHttpSetDefaultProxyConfiguration=WinHttpSetDefaultProxyConfiguration")
#pragma comment(linker, "/export:WinHttpSetOption=WinHttpSetOption")
#pragma comment(linker, "/export:WinHttpSetStatusCallback=WinHttpSetStatusCallback")
#pragma comment(linker, "/export:WinHttpSetTimeouts=WinHttpSetTimeouts")
#pragma comment(linker, "/export:WinHttpTimeFromSystemTime=WinHttpTimeFromSystemTime")
#pragma comment(linker, "/export:WinHttpTimeToSystemTime=WinHttpTimeToSystemTime")
#pragma comment(linker, "/export:WinHttpWebSocketClose=WinHttpWebSocketClose")
#pragma comment(linker, "/export:WinHttpWebSocketCompleteUpgrade=WinHttpWebSocketCompleteUpgrade")
#pragma comment(linker, "/export:WinHttpWebSocketQueryCloseStatus=WinHttpWebSocketQueryCloseStatus")
#pragma comment(linker, "/export:WinHttpWebSocketReceive=WinHttpWebSocketReceive")
#pragma comment(linker, "/export:WinHttpWebSocketSend=WinHttpWebSocketSend")
#pragma comment(linker, "/export:WinHttpWebSocketShutdown=WinHttpWebSocketShutdown")
#pragma comment(linker, "/export:WinHttpWriteData=WinHttpWriteData")
#pragma comment(linker, "/export:WinHttpWriteProxySettings=WinHttpWriteProxySettings")
#pragma comment(linker, "/export:WinHttpFreeQueryConnectionGroupResult=WinHttpFreeQueryConnectionGroupResult")
#pragma comment(linker, "/export:WinHttpGetProxySettingsEx=WinHttpGetProxySettingsEx")
#pragma comment(linker, "/export:WinHttpSetSecureLegacyServersAppCompat=WinHttpSetSecureLegacyServersAppCompat")
#pragma comment(linker, "/export:WinHttpAddRequestHeadersEx=WinHttpAddRequestHeadersEx")
#pragma comment(linker, "/export:WinHttpFreeProxySettingsEx=WinHttpFreeProxySettingsEx")
#pragma comment(linker, "/export:WinHttpGetProxySettingsResultEx=WinHttpGetProxySettingsResultEx")
#pragma comment(linker, "/export:WinHttpQueryHeadersEx=WinHttpQueryHeadersEx")
#pragma comment(linker, "/export:WinHttpReadDataEx=WinHttpReadDataEx")
#pragma comment(linker, "/export:WinHttpPacJsWorkerMain=WinHttpPacJsWorkerMain")
#pragma comment(linker, "/export:WinHttpQueryConnectionGroup=WinHttpQueryConnectionGroup")
#pragma comment(linker, "/export:WinHttpSetProxySettingsPerUser=WinHttpSetProxySettingsPerUser")
#pragma comment(linker, "/export:WinHttpRegisterProxyChangeNotification=WinHttpRegisterProxyChangeNotification")
#pragma comment(linker, "/export:WinHttpUnregisterProxyChangeNotification=WinHttpUnregisterProxyChangeNotification")
#pragma comment(linker, "/export:WinHttpConnectionOnlySend=WinHttpConnectionOnlySend")
#pragma comment(linker, "/export:WinHttpConnectionOnlyReceive=WinHttpConnectionOnlyReceive")
#pragma comment(linker, "/export:WinHttpConnectionOnlyConvert=WinHttpConnectionOnlyConvert")

typedef LPVOID HINTERNET;
typedef WORD INTERNET_PORT;
typedef VOID (CALLBACK* WINHTTP_STATUS_CALLBACK)(IN LPVOID hInternet, IN DWORD_PTR dwContext, IN DWORD dwInternetStatus,IN LPVOID lpvStatusInformation OPTIONAL, IN DWORD dwStatusInformationLength);
typedef WINHTTP_STATUS_CALLBACK(WINAPI* WinHttpSetStatusCallback_ptr)(HINTERNET hInternet, WINHTTP_STATUS_CALLBACK lpfnInternetCallback, DWORD dwNotificationFlags, DWORD_PTR dwReserved);
typedef BOOL(WINAPI* WinHttpAddRequestHeaders_ptr)(HINTERNET hRequest, LPCWSTR lpszHeaders, DWORD dwHeadersLength, DWORD dwModifiers);
typedef BOOL(WINAPI* WinHttpCheckPlatform_ptr)();
typedef BOOL(WINAPI* WinHttpCloseHandle_ptr)(HINTERNET hInternet);
typedef BOOL(WINAPI* WinHttpQueryDataAvailable_ptr)(HINTERNET hRequest, LPDWORD lpdwNumberOfBytesAvailable);
typedef BOOL(WINAPI* WinHttpQueryHeaders_ptr)(HINTERNET hRequest, DWORD dwInfoLevel, LPCWSTR pwszName, LPVOID lpBuffer, LPDWORD lpdwBufferLength, LPDWORD lpdwIndex);
typedef BOOL(WINAPI* WinHttpReadData_ptr)(HINTERNET hRequest, LPVOID lpBuffer, DWORD dwNumberOfBytesToRead, LPDWORD lpdwNumberOfBytesRead);
typedef BOOL(WINAPI* WinHttpReceiveResponse_ptr)(HINTERNET hRequest, LPVOID lpReserved);
typedef BOOL(WINAPI* WinHttpSendRequest_ptr)(HINTERNET hRequest, LPCWSTR lpszHeaders, DWORD dwHeadersLength, LPVOID lpOptional, DWORD dwOptionalLength, DWORD dwTotalLength, DWORD_PTR dwContext);
typedef BOOL(WINAPI* WinHttpSetTimeouts_ptr)(HINTERNET hInternet, int nResolveTimeout, int nConnectTimeout, int nSendTimeout, int nReceiveTimeout);
typedef BOOL(WINAPI* WinHttpWriteData_ptr) (HINTERNET hRequest, LPCVOID lpBuffer, DWORD dwNumberOfBytesToWrite, LPDWORD lpdwNumberOfBytesWritten);
typedef HINTERNET(WINAPI* WinHttpConnect_ptr)(HINTERNET hSession, LPCWSTR pswzServerName, INTERNET_PORT nServerPort, DWORD dwReserved);
typedef HINTERNET(WINAPI* WinHttpOpen_ptr)(LPCWSTR pszAgentW, DWORD dwAccessType, LPCWSTR pszProxyW, LPCWSTR pszProxyBypassW, DWORD dwFlags);
typedef HINTERNET(WINAPI* WinHttpOpenRequest_ptr)(HINTERNET hConnect, LPCWSTR pwszVerb, LPCWSTR pwszObjectName, LPCWSTR pwszVersion, LPCWSTR pwszReferrer, LPCWSTR* ppwszAcceptTypes, DWORD dwFlags);


WinHttpAddRequestHeaders_ptr WinHttpAddRequestHeaders_Orig;
WinHttpCheckPlatform_ptr WinHttpCheckPlatform_Orig;
WinHttpCloseHandle_ptr WinHttpCloseHandle_Orig;
WinHttpConnect_ptr WinHttpConnect_Orig;
WinHttpOpen_ptr WinHttpOpen_Orig = NULL;
WinHttpOpenRequest_ptr WinHttpOpenRequest_Orig;
WinHttpQueryDataAvailable_ptr WinHttpQueryDataAvailable_Orig;
WinHttpQueryHeaders_ptr WinHttpQueryHeaders_Orig;
WinHttpReadData_ptr WinHttpReadData_Orig;
WinHttpReceiveResponse_ptr WinHttpReceiveResponse_Orig;
WinHttpSendRequest_ptr WinHttpSendRequest_Orig;
WinHttpSetStatusCallback_ptr WinHttpSetStatusCallback_Orig;
WinHttpSetTimeouts_ptr WinHttpSetTimeouts_Orig;
WinHttpWriteData_ptr WinHttpWriteData_Orig;


FARPROC DllCanUnloadNow_Orig;
FARPROC DllGetClassObject_Orig;
FARPROC Private1_Orig;
FARPROC SvchostPushServiceGlobals_Orig;
FARPROC WinHttpAutoProxySvcMain_Orig;
FARPROC WinHttpConnectionDeletePolicyEntries_Orig;
FARPROC WinHttpConnectionDeleteProxyInfo_Orig;
FARPROC WinHttpConnectionFreeNameList_Orig;
FARPROC WinHttpConnectionFreeProxyInfo_Orig;
FARPROC WinHttpConnectionFreeProxyList_Orig;
FARPROC WinHttpConnectionGetNameList_Orig;
FARPROC WinHttpConnectionGetProxyInfo_Orig;
FARPROC WinHttpConnectionGetProxyList_Orig;
FARPROC WinHttpConnectionSetPolicyEntries_Orig;
FARPROC WinHttpConnectionSetProxyInfo_Orig;
FARPROC WinHttpConnectionUpdateIfIndexTable_Orig;
FARPROC WinHttpCrackUrl_Orig;
FARPROC WinHttpCreateProxyResolver_Orig;
FARPROC WinHttpCreateUrl_Orig;
FARPROC WinHttpDetectAutoProxyConfigUrl_Orig;
FARPROC WinHttpFreeProxyResult_Orig;
FARPROC WinHttpFreeProxyResultEx_Orig;
FARPROC WinHttpFreeProxySettings_Orig;
FARPROC WinHttpGetDefaultProxyConfiguration_Orig;
FARPROC WinHttpGetIEProxyConfigForCurrentUser_Orig;
FARPROC WinHttpGetProxyForUrl_Orig;
FARPROC WinHttpGetProxyForUrlEx_Orig;
FARPROC WinHttpGetProxyForUrlEx2_Orig;
FARPROC WinHttpGetProxyForUrlHvsi_Orig;
FARPROC WinHttpGetProxyResult_Orig;
FARPROC WinHttpGetProxyResultEx_Orig;
FARPROC WinHttpGetProxySettingsVersion_Orig;
FARPROC WinHttpGetTunnelSocket_Orig;
FARPROC WinHttpProbeConnectivity_Orig;
FARPROC WinHttpQueryAuthSchemes_Orig;
FARPROC WinHttpQueryOption_Orig;
FARPROC WinHttpReadProxySettings_Orig;
FARPROC WinHttpReadProxySettingsHvsi_Orig;
FARPROC WinHttpResetAutoProxy_Orig;
FARPROC WinHttpSaveProxyCredentials_Orig;
FARPROC WinHttpSetCredentials_Orig;
FARPROC WinHttpSetDefaultProxyConfiguration_Orig;
FARPROC WinHttpSetOption_Orig;
FARPROC WinHttpTimeFromSystemTime_Orig;
FARPROC WinHttpTimeToSystemTime_Orig;
FARPROC WinHttpWebSocketClose_Orig;
FARPROC WinHttpWebSocketCompleteUpgrade_Orig;
FARPROC WinHttpWebSocketQueryCloseStatus_Orig;
FARPROC WinHttpWebSocketReceive_Orig;
FARPROC WinHttpWebSocketSend_Orig;
FARPROC WinHttpWebSocketShutdown_Orig;
FARPROC WinHttpWriteProxySettings_Orig;
FARPROC WinHttpSetSecureLegacyServersAppCompat_Orig;
FARPROC WinHttpAddRequestHeadersEx_Orig;
FARPROC WinHttpFreeProxySettingsEx_Orig;
FARPROC WinHttpFreeQueryConnectionGroupResult_Orig;
FARPROC WinHttpGetProxySettingsEx_Orig;
FARPROC WinHttpGetProxySettingsResultEx_Orig;
FARPROC WinHttpQueryHeadersEx_Orig;
FARPROC WinHttpReadDataEx_Orig;
FARPROC WinHttpPacJsWorkerMain_Orig;
FARPROC WinHttpQueryConnectionGroup_Orig;
FARPROC WinHttpRegisterProxyChangeNotification_Orig;
FARPROC WinHttpUnregisterProxyChangeNotification_Orig;
FARPROC WinHttpSetProxySettingsPerUser_Orig;
FARPROC WinHttpConnectionOnlySend_Orig;
FARPROC WinHttpConnectionOnlyReceive_Orig;
FARPROC WinHttpConnectionOnlyConvert_Orig;

extern HMODULE g_thisModule;
bool origLoaded = false;
HMODULE origDll = NULL;



std::wstring GetSysDir()
{
	// get the filename of our DLL and try loading the DLL with the same name from system32
	WCHAR modulePath[MAX_PATH] = { 0 };
	if (!GetSystemDirectoryW(modulePath, _countof(modulePath)))
	{
		MessageBoxW(nullptr, L"GetSystemDirectoryW failed", L"Error", MB_ICONERROR);
	}

	// get filename of this DLL, which should be the original DLLs filename too
	WCHAR ourModulePath[MAX_PATH] = { 0 };
	GetModuleFileNameW(g_thisModule, ourModulePath, _countof(ourModulePath));

	WCHAR exeName[MAX_PATH] = { 0 };
	WCHAR extName[MAX_PATH] = { 0 };
	_wsplitpath_s(ourModulePath, NULL, NULL, NULL, NULL, exeName, MAX_PATH, extName, MAX_PATH);

	swprintf_s(modulePath, MAX_PATH, L"%ws\\%ws%ws", modulePath, exeName, extName);

	std::wstring path = std::wstring(modulePath);

	HRZ::DebugUI::LogWindow::AddLog("[Trace] modulepath: %ls\n", modulePath);

	return path;
};

bool LoadProxy()
{
	if (origLoaded)
		return true;

	origDll = LoadLibraryW(GetSysDir().c_str());
	if (!origDll)
	{
		MessageBoxW(nullptr, L"Could not load originial module", L"Error", MB_ICONERROR);
		return false;
	}

	DllCanUnloadNow_Orig = GetProcAddress(origDll, "DllCanUnloadNow");
	DllGetClassObject_Orig = GetProcAddress(origDll, "DllGetClassObject");
	Private1_Orig = GetProcAddress(origDll, "Private1");
	SvchostPushServiceGlobals_Orig = GetProcAddress(origDll, "SvchostPushServiceGlobals");
	WinHttpAutoProxySvcMain_Orig = GetProcAddress(origDll, "WinHttpAutoProxySvcMain");
	WinHttpConnectionDeletePolicyEntries_Orig = GetProcAddress(origDll, "WinHttpConnectionDeletePolicyEntries");
	WinHttpConnectionDeleteProxyInfo_Orig = GetProcAddress(origDll, "WinHttpConnectionDeleteProxyInfo");
	WinHttpConnectionFreeNameList_Orig = GetProcAddress(origDll, "WinHttpConnectionFreeNameList");
	WinHttpConnectionFreeProxyInfo_Orig = GetProcAddress(origDll, "WinHttpConnectionFreeProxyInfo");
	WinHttpConnectionFreeProxyList_Orig = GetProcAddress(origDll, "WinHttpConnectionFreeProxyList");
	WinHttpConnectionGetNameList_Orig = GetProcAddress(origDll, "WinHttpConnectionGetNameList");
	WinHttpConnectionGetProxyInfo_Orig = GetProcAddress(origDll, "WinHttpConnectionGetProxyInfo");
	WinHttpConnectionGetProxyList_Orig = GetProcAddress(origDll, "WinHttpConnectionGetProxyList");
	WinHttpConnectionSetPolicyEntries_Orig = GetProcAddress(origDll, "WinHttpConnectionSetPolicyEntries");
	WinHttpConnectionSetProxyInfo_Orig = GetProcAddress(origDll, "WinHttpConnectionSetProxyInfo");
	WinHttpConnectionUpdateIfIndexTable_Orig = GetProcAddress(origDll, "WinHttpConnectionUpdateIfIndexTable");
	WinHttpCrackUrl_Orig = GetProcAddress(origDll, "WinHttpCrackUrl");
	WinHttpCreateProxyResolver_Orig = GetProcAddress(origDll, "WinHttpCreateProxyResolver");
	WinHttpCreateUrl_Orig = GetProcAddress(origDll, "WinHttpCreateUrl");
	WinHttpDetectAutoProxyConfigUrl_Orig = GetProcAddress(origDll, "WinHttpDetectAutoProxyConfigUrl");
	WinHttpFreeProxyResult_Orig = GetProcAddress(origDll, "WinHttpFreeProxyResult");
	WinHttpFreeProxyResultEx_Orig = GetProcAddress(origDll, "WinHttpFreeProxyResultEx");
	WinHttpFreeProxySettings_Orig = GetProcAddress(origDll, "WinHttpFreeProxySettings");
	WinHttpGetDefaultProxyConfiguration_Orig = GetProcAddress(origDll, "WinHttpGetDefaultProxyConfiguration");
	WinHttpGetIEProxyConfigForCurrentUser_Orig = GetProcAddress(origDll, "WinHttpGetIEProxyConfigForCurrentUser");
	WinHttpGetProxyForUrl_Orig = GetProcAddress(origDll, "WinHttpGetProxyForUrl");
	WinHttpGetProxyForUrlEx_Orig = GetProcAddress(origDll, "WinHttpGetProxyForUrlEx");
	WinHttpGetProxyForUrlEx2_Orig = GetProcAddress(origDll, "WinHttpGetProxyForUrlEx2");
	WinHttpGetProxyForUrlHvsi_Orig = GetProcAddress(origDll, "WinHttpGetProxyForUrlHvsi");
	WinHttpGetProxyResult_Orig = GetProcAddress(origDll, "WinHttpGetProxyResult");
	WinHttpGetProxyResultEx_Orig = GetProcAddress(origDll, "WinHttpGetProxyResultEx");
	WinHttpGetProxySettingsVersion_Orig = GetProcAddress(origDll, "WinHttpGetProxySettingsVersion");
	WinHttpGetTunnelSocket_Orig = GetProcAddress(origDll, "WinHttpGetTunnelSocket");
	WinHttpProbeConnectivity_Orig = GetProcAddress(origDll, "WinHttpProbeConnectivity");
	WinHttpQueryAuthSchemes_Orig = GetProcAddress(origDll, "WinHttpQueryAuthSchemes");
	WinHttpQueryOption_Orig = GetProcAddress(origDll, "WinHttpQueryOption");
	WinHttpReadProxySettings_Orig = GetProcAddress(origDll, "WinHttpReadProxySettings");
	WinHttpReadProxySettingsHvsi_Orig = GetProcAddress(origDll, "WinHttpReadProxySettingsHvsi");
	WinHttpResetAutoProxy_Orig = GetProcAddress(origDll, "WinHttpResetAutoProxy");
	WinHttpSaveProxyCredentials_Orig = GetProcAddress(origDll, "WinHttpSaveProxyCredentials");
	WinHttpSetCredentials_Orig = GetProcAddress(origDll, "WinHttpSetCredentials");
	WinHttpSetDefaultProxyConfiguration_Orig = GetProcAddress(origDll, "WinHttpSetDefaultProxyConfiguration");
	WinHttpSetOption_Orig = GetProcAddress(origDll, "WinHttpSetOption");
	WinHttpTimeFromSystemTime_Orig = GetProcAddress(origDll, "WinHttpTimeFromSystemTime");
	WinHttpTimeToSystemTime_Orig = GetProcAddress(origDll, "WinHttpTimeToSystemTime");
	WinHttpWebSocketClose_Orig = GetProcAddress(origDll, "WinHttpWebSocketClose");
	WinHttpWebSocketCompleteUpgrade_Orig = GetProcAddress(origDll, "WinHttpWebSocketCompleteUpgrade");
	WinHttpWebSocketQueryCloseStatus_Orig = GetProcAddress(origDll, "WinHttpWebSocketQueryCloseStatus");
	WinHttpWebSocketReceive_Orig = GetProcAddress(origDll, "WinHttpWebSocketReceive");
	WinHttpWebSocketSend_Orig = GetProcAddress(origDll, "WinHttpWebSocketSend");
	WinHttpWebSocketShutdown_Orig = GetProcAddress(origDll, "WinHttpWebSocketShutdown");
	WinHttpWriteProxySettings_Orig = GetProcAddress(origDll, "WinHttpWriteProxySettings");
	WinHttpFreeQueryConnectionGroupResult_Orig = GetProcAddress(origDll, "WinHttpFreeQueryConnectionGroupResult");
	WinHttpGetProxySettingsEx_Orig = GetProcAddress(origDll, "WinHttpGetProxySettingsEx");
	WinHttpFreeProxySettingsEx_Orig = GetProcAddress(origDll, "WinHttpFreeProxySettingsEx");
	WinHttpSetSecureLegacyServersAppCompat_Orig = GetProcAddress(origDll, "WinHttpSetSecureLegacyServersAppCompat");
	WinHttpAddRequestHeadersEx_Orig = GetProcAddress(origDll, "WinHttpAddRequestHeadersEx");
	WinHttpGetProxySettingsResultEx_Orig = GetProcAddress(origDll, "WinHttpGetProxySettingsResultEx");
	WinHttpQueryHeadersEx_Orig = GetProcAddress(origDll, "WinHttpQueryHeadersEx");
	WinHttpReadDataEx_Orig = GetProcAddress(origDll, "WinHttpReadDataEx");
	WinHttpPacJsWorkerMain_Orig = GetProcAddress(origDll, "WinHttpPacJsWorkerMain");
	WinHttpQueryConnectionGroup_Orig = GetProcAddress(origDll, "WinHttpQueryConnectionGroup");
	WinHttpSetProxySettingsPerUser_Orig = GetProcAddress(origDll, "WinHttpSetProxySettingsPerUser");
	WinHttpRegisterProxyChangeNotification_Orig = GetProcAddress(origDll, "WinHttpRegisterProxyChangeNotification");
	WinHttpUnregisterProxyChangeNotification_Orig = GetProcAddress(origDll, "WinHttpUnregisterProxyChangeNotification");
	WinHttpConnectionOnlySend_Orig = GetProcAddress(origDll, "WinHttpConnectionOnlySend");
	WinHttpConnectionOnlyReceive_Orig = GetProcAddress(origDll, "WinHttpConnectionOnlyReceive");
	WinHttpConnectionOnlyConvert_Orig = GetProcAddress(origDll, "WinHttpConnectionOnlyConvert");

	WinHttpReadData_Orig = (WinHttpReadData_ptr)GetProcAddress(origDll, "WinHttpReadData");
	WinHttpQueryDataAvailable_Orig = (WinHttpQueryDataAvailable_ptr)GetProcAddress(origDll, "WinHttpQueryDataAvailable");
	WinHttpQueryHeaders_Orig = (WinHttpQueryHeaders_ptr)GetProcAddress(origDll, "WinHttpQueryHeaders");
	WinHttpReceiveResponse_Orig = (WinHttpReceiveResponse_ptr)GetProcAddress(origDll, "WinHttpReceiveResponse");
	WinHttpSendRequest_Orig = (WinHttpSendRequest_ptr)GetProcAddress(origDll, "WinHttpSendRequest");
	WinHttpSetStatusCallback_Orig = (WinHttpSetStatusCallback_ptr)GetProcAddress(origDll, "WinHttpSetStatusCallback");
	WinHttpSetTimeouts_Orig = (WinHttpSetTimeouts_ptr)GetProcAddress(origDll, "WinHttpSetTimeouts");
	WinHttpWriteData_Orig = (WinHttpWriteData_ptr)GetProcAddress(origDll, "WinHttpWriteData");
	WinHttpAddRequestHeaders_Orig = (WinHttpAddRequestHeaders_ptr)GetProcAddress(origDll, "WinHttpAddRequestHeaders");
	WinHttpCheckPlatform_Orig = (WinHttpCheckPlatform_ptr)GetProcAddress(origDll, "WinHttpCheckPlatform");
	WinHttpCloseHandle_Orig = (WinHttpCloseHandle_ptr)GetProcAddress(origDll, "WinHttpCloseHandle");
	WinHttpConnect_Orig = (WinHttpConnect_ptr)GetProcAddress(origDll, "WinHttpConnect");
	WinHttpOpen_Orig = (WinHttpOpen_ptr)GetProcAddress(origDll, "WinHttpOpen");
	WinHttpOpenRequest_Orig = (WinHttpOpenRequest_ptr)GetProcAddress(origDll, "WinHttpOpenRequest");

	origLoaded = true;
	return true;
}


extern "C" __declspec(dllexport) void __stdcall DllCanUnloadNow() { DllCanUnloadNow_Orig(); }
extern "C" __declspec(dllexport) void __stdcall DllGetClassObject() { DllGetClassObject_Orig(); }
extern "C" __declspec(dllexport) void __stdcall Private1() { Private1_Orig(); }
extern "C" __declspec(dllexport) void __stdcall SvchostPushServiceGlobals() { SvchostPushServiceGlobals_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpAutoProxySvcMain() { WinHttpAutoProxySvcMain_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionDeletePolicyEntries() { WinHttpConnectionDeletePolicyEntries_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionDeleteProxyInfo() { WinHttpConnectionDeleteProxyInfo_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionFreeNameList() { WinHttpConnectionFreeNameList_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionFreeProxyInfo() { WinHttpConnectionFreeProxyInfo_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionFreeProxyList() { WinHttpConnectionFreeProxyList_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionGetNameList() { WinHttpConnectionGetNameList_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionGetProxyInfo() { WinHttpConnectionGetProxyInfo_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionGetProxyList() { WinHttpConnectionGetProxyList_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionSetPolicyEntries() { WinHttpConnectionSetPolicyEntries_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionSetProxyInfo() { WinHttpConnectionSetProxyInfo_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionUpdateIfIndexTable() { WinHttpConnectionUpdateIfIndexTable_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpCrackUrl() { WinHttpCrackUrl_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpCreateProxyResolver() { WinHttpCreateProxyResolver_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpCreateUrl() { WinHttpCreateUrl_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpDetectAutoProxyConfigUrl() { WinHttpDetectAutoProxyConfigUrl_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpFreeProxyResult() { WinHttpFreeProxyResult_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpFreeProxyResultEx() { WinHttpFreeProxyResultEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpFreeProxySettings() { WinHttpFreeProxySettings_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetDefaultProxyConfiguration() { WinHttpGetDefaultProxyConfiguration_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetIEProxyConfigForCurrentUser() { WinHttpGetIEProxyConfigForCurrentUser_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyForUrl() { WinHttpGetProxyForUrl_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyForUrlEx() { WinHttpGetProxyForUrlEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyForUrlEx2() { WinHttpGetProxyForUrlEx2_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyForUrlHvsi() { WinHttpGetProxyForUrlHvsi_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyResult() { WinHttpGetProxyResult_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxyResultEx() { WinHttpGetProxyResultEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxySettingsVersion() { WinHttpGetProxySettingsVersion_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetTunnelSocket() { WinHttpGetTunnelSocket_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpProbeConnectivity() { WinHttpProbeConnectivity_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpQueryAuthSchemes() { WinHttpQueryAuthSchemes_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpQueryOption() { WinHttpQueryOption_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpReadProxySettings() { WinHttpReadProxySettings_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpReadProxySettingsHvsi() { WinHttpReadProxySettingsHvsi_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpResetAutoProxy() { WinHttpResetAutoProxy_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSaveProxyCredentials() { WinHttpSaveProxyCredentials_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSetCredentials() { WinHttpSetCredentials_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSetDefaultProxyConfiguration() { WinHttpSetDefaultProxyConfiguration_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSetOption() { WinHttpSetOption_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpTimeFromSystemTime() { WinHttpTimeFromSystemTime_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpTimeToSystemTime() { WinHttpTimeToSystemTime_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketClose() { WinHttpWebSocketClose_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketCompleteUpgrade() { WinHttpWebSocketCompleteUpgrade_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketQueryCloseStatus() { WinHttpWebSocketQueryCloseStatus_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketReceive() { WinHttpWebSocketReceive_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketSend() { WinHttpWebSocketSend_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWebSocketShutdown() { WinHttpWebSocketShutdown_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpWriteProxySettings() { WinHttpWriteProxySettings_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSetSecureLegacyServersAppCompat() { WinHttpSetSecureLegacyServersAppCompat_Orig(); }
extern "C" __declspec(dllexport) BOOL __stdcall WinHttpAddRequestHeadersEx() { return WinHttpAddRequestHeadersEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpFreeProxySettingsEx() { WinHttpFreeProxySettingsEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpFreeQueryConnectionGroupResult() { WinHttpFreeQueryConnectionGroupResult_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxySettingsEx() { WinHttpGetProxySettingsEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpGetProxySettingsResultEx() { WinHttpGetProxySettingsResultEx_Orig(); }
extern "C" __declspec(dllexport) DWORD __stdcall WinHttpQueryHeadersEx() { return WinHttpQueryHeadersEx_Orig(); }
extern "C" __declspec(dllexport) DWORD __stdcall WinHttpReadDataEx() { return WinHttpReadDataEx_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpPacJsWorkerMain() { WinHttpPacJsWorkerMain_Orig(); }
extern "C" __declspec(dllexport) DWORD __stdcall WinHttpQueryConnectionGroup() { return WinHttpQueryConnectionGroup_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpSetProxySettingsPerUser() { WinHttpSetProxySettingsPerUser_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpRegisterProxyChangeNotification() { WinHttpRegisterProxyChangeNotification_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpUnregisterProxyChangeNotification() { WinHttpUnregisterProxyChangeNotification_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionOnlySend() { WinHttpConnectionOnlySend_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionOnlyReceive() { WinHttpConnectionOnlyReceive_Orig(); }
extern "C" __declspec(dllexport) void __stdcall WinHttpConnectionOnlyConvert() { WinHttpConnectionOnlyConvert_Orig(); }


extern "C" BOOL WINAPI WinHttpAddRequestHeaders(HINTERNET hRequest, LPCWSTR lpszHeaders, DWORD dwHeadersLength, DWORD dwModifiers)
{
	return WinHttpAddRequestHeaders_Orig(hRequest, lpszHeaders, dwHeadersLength, dwModifiers);
}

extern "C" __declspec(dllexport) BOOL  WinHttpCheckPlatform()
{
	return WinHttpCheckPlatform_Orig();
}

extern "C" BOOL WINAPI WinHttpCloseHandle(HINTERNET hInternet)
{
	return WinHttpCloseHandle_Orig(hInternet);
}
extern "C" HINTERNET WINAPI WinHttpConnect(HINTERNET hSession, LPCWSTR pswzServerName, INTERNET_PORT nServerPort, DWORD dwReserved)
{
	return  WinHttpConnect_Orig(hSession, pswzServerName, nServerPort, dwReserved);
}

extern "C" HINTERNET WINAPI WinHttpOpen(LPCWSTR pszAgentW, DWORD dwAccessType, LPCWSTR pszProxyW, LPCWSTR pszProxyBypassW, DWORD dwFlags)
{
	HRZ::DebugUI::LogWindow::AddLog("[Module:] WinHttpOpen\n");
	if (!WinHttpOpen_Orig)
		LoadProxy();
	return WinHttpOpen_Orig(pszAgentW, dwAccessType, pszProxyW, pszProxyBypassW, dwFlags);
}
extern "C" HINTERNET WINAPI WinHttpOpenRequest(HINTERNET hConnect, LPCWSTR pwszVerb, LPCWSTR pwszObjectName, LPCWSTR pwszVersion, LPCWSTR pwszReferrer, LPCWSTR * ppwszAcceptTypes, DWORD dwFlags)
{
	return WinHttpOpenRequest_Orig(hConnect, pwszVerb, pwszObjectName, pwszVersion, pwszReferrer, ppwszAcceptTypes, dwFlags);
}
extern "C"  BOOL WINAPI WinHttpQueryDataAvailable(HINTERNET hRequest, LPDWORD lpdwNumberOfBytesAvailable)
{
	return WinHttpQueryDataAvailable_Orig(hRequest, lpdwNumberOfBytesAvailable);
}
extern "C" BOOL WINAPI WinHttpQueryHeaders(HINTERNET hRequest, DWORD dwInfoLevel, LPCWSTR pwszName, LPVOID lpBuffer, LPDWORD lpdwBufferLength, LPDWORD lpdwIndex)
{
	return WinHttpQueryHeaders_Orig(hRequest, dwInfoLevel, pwszName, lpBuffer, lpdwBufferLength, lpdwIndex);
}
extern "C"  BOOL WINAPI WinHttpReadData(HINTERNET hRequest, LPVOID lpBuffer, DWORD dwNumberOfBytesToRead, LPDWORD lpdwNumberOfBytesRead)
{
	return WinHttpReadData_Orig(hRequest, lpBuffer, dwNumberOfBytesToRead, lpdwNumberOfBytesRead);
}
extern "C" BOOL WINAPI WinHttpReceiveResponse(HINTERNET hRequest, LPVOID lpReserved)
{
	return WinHttpReceiveResponse_Orig(hRequest, lpReserved);
}
extern "C" BOOL WINAPI WinHttpSendRequest(HINTERNET hRequest, LPCWSTR lpszHeaders, DWORD dwHeadersLength, LPVOID lpOptional, DWORD dwOptionalLength, DWORD dwTotalLength, DWORD_PTR dwContex)
{
	return WinHttpSendRequest_Orig(hRequest, lpszHeaders, dwHeadersLength, lpOptional, dwOptionalLength, dwTotalLength, dwContex);
}
extern "C" WINHTTP_STATUS_CALLBACK WINAPI WinHttpSetStatusCallback(HINTERNET hInternet, WINHTTP_STATUS_CALLBACK lpfnInternetCallback, DWORD dwNotificationFlags, DWORD_PTR dwReserved)
{
	return WinHttpSetStatusCallback_Orig(hInternet, lpfnInternetCallback, dwNotificationFlags, dwReserved);
}
extern "C" BOOL WINAPI WinHttpSetTimeouts(HINTERNET hInternet, int nResolveTimeout, int nConnectTimeout, int nSendTimeout, int nReceiveTimeout)
{
	return WinHttpSetTimeouts_Orig(hInternet, nResolveTimeout, nConnectTimeout, nSendTimeout, nReceiveTimeout);
}
extern "C" BOOL WINAPI WinHttpWriteData(HINTERNET hRequest, LPCVOID lpBuffer, DWORD dwNumberOfBytesToWrite, LPDWORD lpdwNumberOfBytesWritten)
{
	return WinHttpWriteData_Orig(hRequest, lpBuffer, dwNumberOfBytesToWrite, lpdwNumberOfBytesWritten);
}

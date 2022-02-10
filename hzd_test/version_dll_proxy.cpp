#include"common.h"
#include <stdlib.h>
#include <stdio.h>


#pragma comment(linker, "/export:GetFileVersionInfoSizeA=GetFileVersionInfoSizeA")
#pragma comment(linker, "/export:GetFileVersionInfoA=GetFileVersionInfoA")
#pragma comment(linker, "/export:GetFileVersionInfoByHandle=GetFileVersionInfoByHandle")
#pragma comment(linker, "/export:GetFileVersionInfoExA=GetFileVersionInfoExA")
#pragma comment(linker, "/export:GetFileVersionInfoExW=GetFileVersionInfoExW")
#pragma comment(linker, "/export:GetFileVersionInfoSizeExA=GetFileVersionInfoSizeExA")
#pragma comment(linker, "/export:GetFileVersionInfoSizeExW=GetFileVersionInfoSizeExW")
#pragma comment(linker, "/export:GetFileVersionInfoSizeW=GetFileVersionInfoSizeW")
#pragma comment(linker, "/export:GetFileVersionInfoW=GetFileVersionInfoW")
#pragma comment(linker, "/export:VerFindFileA=VerFindFileA")
#pragma comment(linker, "/export:VerFindFileW=VerFindFileW")
#pragma comment(linker, "/export:VerInstallFileA=VerInstallFileA")
#pragma comment(linker, "/export:VerInstallFileW=VerInstallFileW")
#pragma comment(linker, "/export:VerLanguageNameA=VerLanguageNameA")
#pragma comment(linker, "/export:VerLanguageNameW=VerLanguageNameW")
#pragma comment(linker, "/export:VerQueryValueA=VerQueryValueA")
#pragma comment(linker, "/export:VerQueryValueW=VerQueryValueW")


typedef DWORD(APIENTRY* GetFileVersionInfoSizeA_ptr)(LPCSTR lptstrFilename, LPDWORD lpdwHandle);
typedef DWORD(WINAPI* GetFileVersionInfoA_ptr)(LPCSTR, DWORD, DWORD, LPVOID);
typedef DWORD(WINAPI* GetFileVersionInfoExA_ptr)(DWORD, LPCSTR, DWORD, DWORD, LPVOID);
typedef DWORD(WINAPI* GetFileVersionInfoSizeExA_ptr)(DWORD, LPCSTR, LPDWORD);
typedef DWORD(WINAPI* GetFileVersionInfoExW_ptr)(DWORD, LPCWSTR, DWORD, DWORD, LPVOID);
typedef DWORD(WINAPI* GetFileVersionInfoSizeW_ptr)(LPCWSTR, LPDWORD);
typedef DWORD(WINAPI* GetFileVersionInfoSizeExW_ptr)(DWORD, LPCWSTR, LPDWORD);
typedef DWORD(WINAPI* GetFileVersionInfoW_ptr)(LPCWSTR, DWORD, DWORD, LPVOID);
typedef DWORD(WINAPI* VerFindFileA_ptr)(DWORD, LPCSTR, LPCSTR, LPCSTR, LPSTR, PUINT, LPSTR, PUINT);
typedef DWORD(WINAPI* VerFindFileW_ptr)(DWORD, LPCWSTR, LPCWSTR, LPCWSTR, LPWSTR, PUINT, LPWSTR, PUINT);
typedef DWORD(WINAPI* VerInstallFileA_ptr)(DWORD, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPSTR, PUINT);
typedef DWORD(WINAPI* VerInstallFileW_ptr)(DWORD, LPCWSTR, LPCWSTR, LPCWSTR, LPCWSTR, LPCWSTR, LPWSTR, PUINT);
typedef DWORD(WINAPI* VerLanguageNameA_ptr)(DWORD, LPSTR, DWORD);
typedef DWORD(WINAPI* VerLanguageNameW_ptr)(DWORD, LPWSTR, DWORD);
typedef DWORD(WINAPI* VerQueryValueA_ptr)(LPCVOID, LPCSTR, LPVOID*, PUINT);
typedef DWORD(WINAPI* VerQueryValueW_ptr)(LPCVOID, LPCWSTR, LPVOID*, PUINT);

GetFileVersionInfoSizeA_ptr GetFileVersionInfoSizeA_Orig = NULL;
GetFileVersionInfoA_ptr GetFileVersionInfoA_Orig;
FARPROC GetFileVersionInfoByHandle_Orig;
GetFileVersionInfoExA_ptr GetFileVersionInfoExA_Orig;
GetFileVersionInfoExW_ptr GetFileVersionInfoExW_Orig;
GetFileVersionInfoSizeExA_ptr GetFileVersionInfoSizeExA_Orig;
GetFileVersionInfoSizeExW_ptr GetFileVersionInfoSizeExW_Orig;
GetFileVersionInfoSizeW_ptr GetFileVersionInfoSizeW_Orig;
GetFileVersionInfoW_ptr GetFileVersionInfoW_Orig;
VerFindFileA_ptr VerFindFileA_Orig;
VerFindFileW_ptr VerFindFileW_Orig;
VerInstallFileA_ptr VerInstallFileA_Orig;
VerInstallFileW_ptr VerInstallFileW_Orig;
VerLanguageNameA_ptr VerLanguageNameA_Orig;
VerLanguageNameW_ptr VerLanguageNameW_Orig;
VerQueryValueA_ptr VerQueryValueA_Orig;
VerQueryValueW_ptr VerQueryValueW_Orig;



extern HMODULE g_thisModule;
bool origLoaded = false;
HMODULE origDll = NULL;

bool LoadProxy()
{
	if (origLoaded)
		return true;

	// get the filename of our DLL and try loading the DLL with the same name from system32
	WCHAR modulePath[MAX_PATH] = { 0 };
	if (!GetSystemDirectoryW(modulePath, _countof(modulePath)))
	{
		MessageBoxW(nullptr, L"GetSystemDirectoryW failed", L"Error", MB_OK);
		return false;
	}

	// get filename of this DLL, which should be the original DLLs filename too
	WCHAR thisModulePath[MAX_PATH] = { 0 };
	GetModuleFileNameW(g_thisModule, thisModulePath, _countof(thisModulePath));

	WCHAR exeName[MAX_PATH] = { 0 };
	WCHAR extName[MAX_PATH] = { 0 };
	_wsplitpath_s(thisModulePath, NULL, NULL, NULL, NULL, exeName, MAX_PATH, extName, MAX_PATH);

	swprintf_s(modulePath, MAX_PATH, L"%ws\\%ws%ws", modulePath, exeName, extName);

	origDll = LoadLibraryW(modulePath);
	if (!origDll)
	{
		MessageBoxW(nullptr, L"Could not load originial module", L"Error", MB_OK);
		return false;
	}

	GetFileVersionInfoSizeA_Orig = (GetFileVersionInfoSizeA_ptr)GetProcAddress(origDll, "GetFileVersionInfoSizeA");
	GetFileVersionInfoA_Orig = (GetFileVersionInfoA_ptr)GetProcAddress(origDll, "GetFileVersionInfoA");

	GetFileVersionInfoByHandle_Orig = GetProcAddress(origDll, "GetFileVersionInfoByHandle");

	GetFileVersionInfoExA_Orig = (GetFileVersionInfoExA_ptr)GetProcAddress(origDll, "GetFileVersionInfoExA");
	GetFileVersionInfoExW_Orig = (GetFileVersionInfoExW_ptr)GetProcAddress(origDll, "GetFileVersionInfoExW");
	GetFileVersionInfoSizeExA_Orig = (GetFileVersionInfoSizeExA_ptr)GetProcAddress(origDll, "GetFileVersionInfoSizeExA");
	GetFileVersionInfoSizeExW_Orig = (GetFileVersionInfoSizeExW_ptr)GetProcAddress(origDll, "GetFileVersionInfoSizeExW");
	GetFileVersionInfoSizeW_Orig = (GetFileVersionInfoSizeW_ptr)GetProcAddress(origDll, "GetFileVersionInfoSizeW");
	GetFileVersionInfoW_Orig = (GetFileVersionInfoW_ptr)GetProcAddress(origDll, "GetFileVersionInfoW");
	VerFindFileA_Orig = (VerFindFileA_ptr)GetProcAddress(origDll, "VerFindFileA");
	VerFindFileW_Orig = (VerFindFileW_ptr)GetProcAddress(origDll, "VerFindFileW");
	VerInstallFileA_Orig = (VerInstallFileA_ptr)GetProcAddress(origDll, "VerInstallFileA");
	VerInstallFileW_Orig = (VerInstallFileW_ptr)GetProcAddress(origDll, "VerInstallFileW");
	VerLanguageNameA_Orig = (VerLanguageNameA_ptr)GetProcAddress(origDll, "VerLanguageNameA");
	VerLanguageNameW_Orig = (VerLanguageNameW_ptr)GetProcAddress(origDll, "VerLanguageNameW");
	VerQueryValueA_Orig = (VerQueryValueA_ptr)GetProcAddress(origDll, "VerQueryValueA");
	VerQueryValueW_Orig = (VerQueryValueW_ptr)GetProcAddress(origDll, "VerQueryValueW");

	origLoaded = true;
	return true;
}

extern "C" DWORD WINAPI GetFileVersionInfoSizeA(LPCSTR lptstrFilename, LPDWORD lpdwHandle)
{

	if (!GetFileVersionInfoSizeA_Orig)
		LoadProxy(); //putting it here since HZD also uses GetFileVersionInfoSizeA

	return GetFileVersionInfoSizeA_Orig(lptstrFilename, lpdwHandle);
}

extern "C" BOOL WINAPI GetFileVersionInfoA(LPCSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData)
{
	return (GetFileVersionInfoA_Orig)(lptstrFilename, dwHandle, dwLen, lpData);
}

extern "C" __declspec(dllexport) void __stdcall GetFileVersionInfoByHandle() { GetFileVersionInfoByHandle_Orig(); }

extern "C" BOOL WINAPI GetFileVersionInfoExA(DWORD dwFlags, LPCSTR lpwstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData)
{
	return (GetFileVersionInfoExA_Orig)(dwFlags, lpwstrFilename, dwHandle, dwLen, lpData);
}

extern "C" BOOL WINAPI GetFileVersionInfoExW(DWORD dwFlags, LPCWSTR lpwstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData)
{
	return (GetFileVersionInfoExW_Orig)(dwFlags, lpwstrFilename, dwHandle, dwLen, lpData);
}

extern "C" DWORD WINAPI GetFileVersionInfoSizeExA(DWORD dwFlags, LPCSTR lpwstrFilename, LPDWORD lpdwHandle)
{
	return (GetFileVersionInfoSizeExA_Orig)(dwFlags, lpwstrFilename, lpdwHandle);
}

extern "C" DWORD WINAPI GetFileVersionInfoSizeW(LPCWSTR lptstrFilename, LPDWORD lpdwHandle)
{
	return (GetFileVersionInfoSizeW_Orig)(lptstrFilename, lpdwHandle);
}

extern "C" DWORD WINAPI GetFileVersionInfoSizeExW(DWORD dwFlags, LPCWSTR lpwstrFilename, LPDWORD lpdwHandle)
{
	return (GetFileVersionInfoSizeExW_Orig)(dwFlags, lpwstrFilename, lpdwHandle);
}

extern "C" BOOL WINAPI GetFileVersionInfoW(LPCWSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData)
{
	return (GetFileVersionInfoW_Orig)(lptstrFilename, dwHandle, dwLen, lpData);
}
extern "C" DWORD WINAPI VerFindFileA(DWORD uFlags, LPCSTR szFileName, LPCSTR szWinDir, LPCSTR szAppDir, LPSTR szCurDir, PUINT puCurDirLen, LPSTR szDestDir, PUINT puDestDirLen)
{
	return (VerFindFileA_Orig)(uFlags, szFileName, szWinDir, szAppDir, szCurDir, puCurDirLen, szDestDir, puDestDirLen);
}
extern "C" DWORD WINAPI VerFindFileW(DWORD uFlags, LPCWSTR szFileName, LPCWSTR szWinDir, LPCWSTR szAppDir, LPWSTR szCurDir, PUINT puCurDirLen, LPWSTR szDestDir, PUINT puDestDirLen)
{
	return (VerFindFileW_Orig)(uFlags, szFileName, szWinDir, szAppDir, szCurDir, puCurDirLen, szDestDir, puDestDirLen);
}

extern DWORD WINAPI VerInstallFileA(DWORD uFlags, LPCSTR szSrcFileName, LPCSTR szDestFileName, LPCSTR szSrcDir, LPCSTR szDestDir, LPCSTR szCurDir, LPSTR szTmpFile, PUINT puTmpFileLen)
{
	return (VerInstallFileA_Orig)(uFlags, szSrcFileName, szDestFileName, szSrcDir, szDestDir, szCurDir, szTmpFile, puTmpFileLen);
}
extern "C" DWORD WINAPI VerInstallFileW(DWORD uFlags, LPCWSTR szSrcFileName, LPCWSTR szDestFileName, LPCWSTR szSrcDir, LPCWSTR szDestDir, LPCWSTR szCurDir, LPWSTR szTmpFile, PUINT puTmpFileLen)
{
	return (VerInstallFileW_Orig)(uFlags, szSrcFileName, szDestFileName, szSrcDir, szDestDir, szCurDir, szTmpFile, puTmpFileLen);
}
extern "C" DWORD WINAPI VerLanguageNameA(DWORD wLang, LPSTR szLang, DWORD cchLang)
{
	return (VerLanguageNameA_Orig)(wLang, szLang, cchLang);
}

extern "C" DWORD WINAPI VerLanguageNameW(DWORD wLang, LPWSTR szLang, DWORD cchLang)
{
	return (VerLanguageNameW_Orig)(wLang, szLang, cchLang);
}
extern "C" BOOL WINAPI VerQueryValueA(LPCVOID pBlock, LPCSTR lpSubBlock, LPVOID * lplpBuffer, PUINT puLen)
{
	return (VerQueryValueA_Orig)(pBlock, lpSubBlock, lplpBuffer, puLen);
}

extern "C" BOOL WINAPI VerQueryValueW(LPCVOID pBlock, LPCWSTR lpSubBlock, LPVOID * lplpBuffer, PUINT puLen)
{
	return (VerQueryValueW_Orig)(pBlock, lpSubBlock, lplpBuffer, puLen);
}

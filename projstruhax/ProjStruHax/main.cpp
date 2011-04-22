#include <stdio.h>
#include <windows.h>
#include <strsafe.h>

#include <string>
#include <iostream>

int main(int argc, char** argv)
{
    HMODULE hLocKernel32 = GetModuleHandle("Kernel32");
    FARPROC hLocLoadLibrary = GetProcAddress(hLocKernel32, "LoadLibraryA");

    STARTUPINFO si;
    PROCESS_INFORMATION pi;

    memset(&si, 0, sizeof(si));
    memset(&pi, 0, sizeof(pi));
    si.cb = sizeof(si);

	LPTSTR dir = new TCHAR[128];
	if(GetCurrentDirectory(128, dir) == 0)
	{
		printf("GetCurrentDirectory failed\n");
		return -1;
	}

    if( !CreateProcess( 
		NULL, //app name
		"ProjStru.exe", //command line
		NULL, //process attributes
		NULL, //thread attributes
		FALSE, //inherit handles
		CREATE_SUSPENDED, //creation flags
		NULL, //enviroment
		NULL, //current directory
		&si, &pi)
    )
    {
        printf("CreateProcess failed. %d\n", GetLastError());
		return -1;
    }

	LPTSTR dllpath = new TCHAR[164];
	StringCbCopy(dllpath, 164, dir);
	StringCbCopy(dllpath + strlen(dir), 164 - strlen(dir), TEXT("\\hax.dll"));

	printf("Will inject %s\n", dllpath);

	printf("VirtualAllocEx...\n");
    LPVOID hRemoteMem = VirtualAllocEx(pi.hProcess, NULL, strlen(dllpath), MEM_COMMIT, PAGE_READWRITE);

	if(hRemoteMem == 0)
	{
		printf("VirtualAllocEx failed %d\n", GetLastError());
		return 0;
	}

    DWORD numBytesWritten;
    WriteProcessMemory(pi.hProcess, hRemoteMem, dllpath, strlen(dllpath), &numBytesWritten);

	printf("CreateRemoteThread...\n");
    HANDLE hRemoteThread = CreateRemoteThread(pi.hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)hLocLoadLibrary, hRemoteMem, 0, NULL);

    std::cout << hRemoteThread << std::endl;

	if(hRemoteThread == 0)
	{
		printf("CreateRemoteThread failed %d\n", GetLastError());
	}

    bool res = (bool)WaitForSingleObject(hRemoteThread, INFINITE) != WAIT_TIMEOUT;

    VirtualFreeEx(pi.hProcess, hRemoteMem, strlen(dllpath), MEM_RELEASE);

	printf("ResumeThread...\n");
	ResumeThread(pi.hThread);

	printf("Resumed, running...\n");
    WaitForSingleObject( pi.hProcess, INFINITE );

    CloseHandle( pi.hProcess );
    CloseHandle( pi.hThread );
}

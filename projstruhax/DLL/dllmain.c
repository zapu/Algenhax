#include <windows.h>

#include <stdio.h>
#include <string.h>

const DWORD callAddr = 0x00404802;
const DWORD storageAddr = 0x004C80F9;

//Hijack sprintf call to save result variable

int sprintf_patched(char* dest, char* fmt, double val)
{
	*((double*)(storageAddr+4)) = val;
	return sprintf(dest, "SUP BROHAN %f", val);
}

void patchCall()
{
	DWORD dummy;

	VirtualProtect((LPVOID)(callAddr), 5, PAGE_EXECUTE_READWRITE, &dummy);
	*(unsigned int*)(callAddr+1) = (unsigned int)&sprintf_patched - (unsigned int)(callAddr+5);

	VirtualProtect((LPVOID)(storageAddr), 13, PAGE_READWRITE, &dummy);
	*((char*)storageAddr) = 0x00; //Terminate string
	*((char*)storageAddr+1) = 0xBE; //magic
	*((char*)storageAddr+2) = 0xEF;
}

BOOL APIENTRY DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    switch (fdwReason)
    {
        case DLL_PROCESS_ATTACH:
			patchCall();
            break;

        case DLL_PROCESS_DETACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
            break;
    }
    return TRUE;
}

#include "pch.h"
#include <windows.h>
#include <tchar.h>
#include <iostream>
#include "SharedFileOut.h"
#include "TelemetryDll.h"
#pragma optimize("",off)

template <typename T, unsigned S>
inline unsigned arraysize(const T(&v)[S]) {
    return S;
}


struct SMElement {
    HANDLE hMapFile;
    unsigned char* mapFileBuffer;
};

SMElement m_graphics;
SMElement m_physics;
SMElement m_static;

int m_test_number = 1;

void init_physics() {
    TCHAR szName[] = TEXT("Local\\acpmf_physics");
    m_physics.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFilePhysics), szName);
    if (!m_physics.hMapFile) {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_physics.mapFileBuffer = (unsigned char*)MapViewOfFile(m_physics.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFilePhysics));
    if (!m_physics.mapFileBuffer) {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void init_graphics() {
    TCHAR szName[] = TEXT("Local\\acpmf_graphics");
    m_graphics.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFileGraphics), szName);
    if (!m_graphics.hMapFile) {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_graphics.mapFileBuffer = (unsigned char*)MapViewOfFile(m_graphics.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFileGraphics));
    if (!m_graphics.mapFileBuffer) {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void init_static() {
    TCHAR szName[] = TEXT("Local\\acpmf_static");
    m_static.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFileStatic), szName);
    if (!m_static.hMapFile) {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_static.mapFileBuffer = (unsigned char*)MapViewOfFile(m_static.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFileStatic));
    if (!m_static.mapFileBuffer) {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void dismiss(SMElement element) {
    UnmapViewOfFile(element.mapFileBuffer);
    CloseHandle(element.hMapFile);
}

CPPWIN32DLL_EXPORTS void update_graphics_struct(OUT SPageFileGraphics* ptr_phys_struct, size_t struct_size) {
    if (!m_graphics.mapFileBuffer) {
        init_graphics();
    }

    memcpy(ptr_phys_struct, m_graphics.mapFileBuffer, struct_size);
}

CPPWIN32DLL_EXPORTS void update_physics_struct(OUT SPageFilePhysics* ptr_grph_struct, size_t struct_size) {
    if (!m_physics.mapFileBuffer) {
        init_physics();
    }

    memcpy(ptr_grph_struct, m_physics.mapFileBuffer, struct_size);
}

CPPWIN32DLL_EXPORTS void update_static_struct(OUT SPageFileStatic* ptr_stc_struct, size_t struct_size) {
    if (!m_static.mapFileBuffer) {
        init_static();
    }

    memcpy(ptr_stc_struct, m_static.mapFileBuffer, struct_size);
}


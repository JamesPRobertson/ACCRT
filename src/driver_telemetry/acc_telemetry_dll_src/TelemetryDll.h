#pragma once

#include "pch.h"
#include "SharedFileOut.h"

#ifndef CPPWIN32DLL_EXPORTS
#define CPPWIN32DLL_EXPORTS __declspec(dllexport)
#endif

extern "C" {
    CPPWIN32DLL_EXPORTS struct SPageFilePhysics;
    CPPWIN32DLL_EXPORTS struct SPageFileGraphics;
    CPPWIN32DLL_EXPORTS struct SPageFileStatic;

    CPPWIN32DLL_EXPORTS void update_physics_struct (OUT SPageFilePhysics*  ptr_phys_struct, size_t struct_size);
    CPPWIN32DLL_EXPORTS void update_graphics_struct(OUT SPageFileGraphics* ptr_grph_struct, size_t struct_size);
    CPPWIN32DLL_EXPORTS void update_static_struct  (OUT SPageFileStatic*   ptr_stc_struct,  size_t struct_size);
}


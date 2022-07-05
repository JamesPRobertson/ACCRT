#pragma once

#include "pch.h"
#include "SharedFileOut.h"
#include "stdbool.h"

#ifndef CPPWIN32DLL_EXPORTS
#define CPPWIN32DLL_EXPORTS __declspec(dllexport)
#endif

extern "C" {
    struct SPageFilePhysics;
    struct SPageFileGraphics;
    struct SPageFileStatic;

    CPPWIN32DLL_EXPORTS bool update_physics_data (OUT SPageFilePhysics*  ptr_phys_struct, size_t data_size);
    CPPWIN32DLL_EXPORTS bool update_graphics_data(OUT SPageFileGraphics* ptr_grph_struct, size_t data_size);
    CPPWIN32DLL_EXPORTS bool update_static_data  (OUT SPageFileStatic*   ptr_stc_struct,  size_t data_size);
}

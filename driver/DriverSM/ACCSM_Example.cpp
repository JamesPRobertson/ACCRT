#include "stdafx.h"
#include <windows.h>
#include <tchar.h>
#include <iostream>
#include "SharedFileOut.h"
#pragma optimize("",off)
using namespace std;


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

void initGraphics() {
    TCHAR szName[] = TEXT("Local\\acpmf_graphics");
    m_graphics.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFileGraphic), szName);
    if (!m_graphics.hMapFile) {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_graphics.mapFileBuffer = (unsigned char*)MapViewOfFile(m_graphics.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFileGraphic));
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

void print_data(string name, float value) {
    wcout << name.c_str() << " : " << value << endl;
}

template <typename T, unsigned S>
inline void print_data(const string name, const T(&v)[S]) {
    wcout << name.c_str() << " : ";
    
    for (int i = 0; i < S; i++)
    {
        wcout << v[i];
        if (i < S - 1)
        {
            wcout << " , ";
        }

    }
    wcout << endl;
}

template <typename T, unsigned S, unsigned S2>
inline void print_vector_data(const string name, const T(&v)[S][S2]) {
    wcout << name.c_str() << " : ";

    for (int i = 0; i < S; i++)
    {
        wcout << i << " : ";
        for (int j = 0; j < S2; j++) {
            wcout << v[i][j];
            if (j < S2 - 1)
            {
                wcout << " , ";
            }
        }

        wcout << ";" << endl;
       
    }

}

int _tmain(int argc, _TCHAR* argv[]) {
    init_physics();
    initGraphics();
    init_static();

    wcout << "Press 1 for physics, 2 for graphics, 3 for static" << endl;

    while (true) {
        // User presses 1
        if (GetAsyncKeyState(0x31) != 0) {
            wcout << "---------------PHYSICS INFO---------------" << endl;
            SPageFilePhysics* pf = (SPageFilePhysics*)m_physics.mapFileBuffer;
            print_data("acc G", pf->accG);
            print_data("brake", pf->brake);
            print_data("camber rad", pf->camberRAD);
            print_data("damage", pf->carDamage);
            print_data("car height", pf->cgHeight);
            print_data("drs", pf->drs);
            print_data("tc", pf->tc);
            print_data("fuel", pf->fuel);
            print_data("gas", pf->gas);
            print_data("gear", pf->gear);
            print_data("number of tyres out", pf->numberOfTyresOut);
            print_data("packet id", pf->packetId);
            print_data("heading", pf->heading);
            print_data("pitch", pf->pitch);
            print_data("roll", pf->roll);
            print_data("rpms", pf->rpms);
            print_data("speed kmh", pf->speedKmh);
            print_vector_data("contact point", pf->tyreContactPoint);
            print_vector_data("contact normal", pf->tyreContactNormal);
            print_vector_data("contact heading", pf->tyreContactHeading);
            print_data("steer ", pf->steerAngle);
            print_data("suspension travel", pf->suspensionTravel);
            print_data("tyre core temp", pf->tyreCoreTemperature);
            print_data("tyre dirty level", pf->tyreDirtyLevel);
            print_data("tyre wear", pf->tyreWear);
            print_data("velocity", pf->velocity);
            print_data("wheel angular speed", pf->wheelAngularSpeed);
            print_data("wheel load", pf->wheelLoad);
            print_data("wheel slip", pf->wheelSlip);
            print_data("wheel pressure", pf->wheelsPressure);
        }

        // User presses 2
        if (GetAsyncKeyState(0x32) != 0) {
            wcout << "---------------GRAPHICS INFO---------------" << endl;
            SPageFileGraphic* pf = (SPageFileGraphic*)m_graphics.mapFileBuffer;
            print_data("packetID ", pf->packetId);
            print_data("STATUS ", pf->status);
            print_data("session", pf->session);
            print_data("completed laps", pf->completedLaps);
            print_data("position", pf->position);
            print_data("current time s", pf->currentTime);
            print_data("current time", pf->iCurrentTime);
            print_data("last time s", pf->lastTime);
            print_data("last time ", pf->iLastTime);
            print_data("best time s", pf->bestTime);
            print_data("best time", pf->iBestTime);
            print_data("sessionTimeLeft", pf->sessionTimeLeft);
            print_data("distanceTraveled", pf->distanceTraveled);
            print_data("isInPit", pf->isInPit);
            print_data("currentSectorIndex", pf->currentSectorIndex);
            print_data("lastSectorTime", pf->lastSectorTime);
            print_data("numberOfLaps", pf->numberOfLaps);
            wcout << "TYRE COMPOUND : " << pf->tyreCompound << endl;
            print_data("replayMult", pf->replayTimeMultiplier);
            print_data("normalizedCarPosition", pf->normalizedCarPosition);
            print_vector_data("carCoordinates", pf->carCoordinates);
        }

        // User presses 3
        if (GetAsyncKeyState(0x33) != 0) {
            wcout << "---------------STATIC INFO---------------" << endl;
            SPageFileStatic* pf = (SPageFileStatic*)m_static.mapFileBuffer;
            wcout << "SM VERSION " << pf->smVersion << endl;
            wcout << "AC VERSION " << pf->acVersion << endl;

            print_data("number of sessions ", pf->numberOfSessions);
            print_data("numCars", pf->numCars);
            wcout << "Car model " << pf->carModel << endl;
            wcout << "Car track " << pf->track << endl;
            wcout << "Player Name " << pf->playerName << endl;
            print_data("sectorCount", pf->sectorCount);

            print_data("maxTorque", pf->maxTorque);
            print_data("maxPower", pf->maxPower);
            print_data("maxRpm", pf->maxRpm);
            print_data("maxFuel", pf->maxFuel);
            print_data("suspensionMaxTravel", pf->suspensionMaxTravel);
            print_data("tyreRadius", pf->tyreRadius);

        }
    }

    dismiss(m_graphics);
    dismiss(m_physics);
    dismiss(m_static);

    return 0;
}

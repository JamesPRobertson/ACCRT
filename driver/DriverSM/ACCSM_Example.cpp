#undef UNICODE

#define WIN32_LEAN_AND_MEAN

// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include <winsock2.h>
#include <windows.h>
#include "stdafx.h"
#include <tchar.h>
#include <iostream>
#include "SharedFileOut.h"
#pragma optimize("",off)
//using namespace std;


#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>

// Need to link with Ws2_32.lib
#pragma comment (lib, "Ws2_32.lib")
// #pragma comment (lib, "Mswsock.lib")

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27015"

int __cdecl main(void)
{
    WSADATA wsaData;
    int iResult;

    SOCKET ListenSocket = INVALID_SOCKET;
    SOCKET ClientSocket = INVALID_SOCKET;

    struct addrinfo* result = NULL;
    struct addrinfo hints;

    int iSendResult;
    char recvbuf[DEFAULT_BUFLEN];
    int recvbuflen = DEFAULT_BUFLEN;

    // Initialize Winsock
    iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (iResult != 0) {
        printf("WSAStartup failed with error: %d\n", iResult);
        return 1;
    }

    ZeroMemory(&hints, sizeof(hints));
    hints.ai_family = AF_INET;
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_protocol = IPPROTO_TCP;
    hints.ai_flags = AI_PASSIVE;

    // Resolve the server address and port
    iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result);
    if (iResult != 0) {
        printf("getaddrinfo failed with error: %d\n", iResult);
        WSACleanup();
        return 1;
    }

    // Create a SOCKET for connecting to server
    ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
    if (ListenSocket == INVALID_SOCKET) {
        printf("socket failed with error: %ld\n", WSAGetLastError());
        freeaddrinfo(result);
        WSACleanup();
        return 1;
    }

    // Setup the TCP listening socket
    iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
    if (iResult == SOCKET_ERROR) {
        printf("bind failed with error: %d\n", WSAGetLastError());
        freeaddrinfo(result);
        closesocket(ListenSocket);
        WSACleanup();
        return 1;
    }

    freeaddrinfo(result);

    iResult = listen(ListenSocket, SOMAXCONN);
    if (iResult == SOCKET_ERROR) {
        printf("listen failed with error: %d\n", WSAGetLastError());
        closesocket(ListenSocket);
        WSACleanup();
        return 1;
    }

    // Accept a client socket
    ClientSocket = accept(ListenSocket, NULL, NULL);
    if (ClientSocket == INVALID_SOCKET) {
        printf("accept failed with error: %d\n", WSAGetLastError());
        closesocket(ListenSocket);
        WSACleanup();
        return 1;
    }

    // No longer need server socket
    closesocket(ListenSocket);

    // Receive until the peer shuts down the connection
    do {

        iResult = recv(ClientSocket, recvbuf, recvbuflen, 0);
        if (iResult > 0) {
            printf("Bytes received: %d\n", iResult);

            // Echo the buffer back to the sender
            iSendResult = send(ClientSocket, recvbuf, iResult, 0);
            if (iSendResult == SOCKET_ERROR) {
                printf("send failed with error: %d\n", WSAGetLastError());
                closesocket(ClientSocket);
                WSACleanup();
                return 1;
            }
            printf("Bytes sent: %d\n", iSendResult);
        }
        else if (iResult == 0)
            printf("Connection closing...\n");
        else {
            printf("recv failed with error: %d\n", WSAGetLastError());
            closesocket(ClientSocket);
            WSACleanup();
            return 1;
        }

    } while (iResult > 0);

    // shutdown the connection since we're done
    iResult = shutdown(ClientSocket, SD_SEND);
    if (iResult == SOCKET_ERROR) {
        printf("shutdown failed with error: %d\n", WSAGetLastError());
        closesocket(ClientSocket);
        WSACleanup();
        return 1;
    }

    // cleanup
    closesocket(ClientSocket);
    WSACleanup();

    return 0;
}


template <typename T, unsigned S>
inline unsigned arraysize(const T(&v)[S])
{
    return S;
}


struct SMElement
{
    HANDLE hMapFile;
    unsigned char* mapFileBuffer;
};

SMElement m_graphics;
SMElement m_physics;
SMElement m_static;

void initPhysics()
{
    TCHAR szName[] = TEXT("Local\\acpmf_physics");
    m_physics.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFilePhysics), szName);
    if (!m_physics.hMapFile)
    {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_physics.mapFileBuffer = (unsigned char*)MapViewOfFile(m_physics.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFilePhysics));
    if (!m_physics.mapFileBuffer)
    {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void initGraphics()
{
    TCHAR szName[] = TEXT("Local\\acpmf_graphics");
    m_graphics.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFileGraphic), szName);
    if (!m_graphics.hMapFile)
    {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_graphics.mapFileBuffer = (unsigned char*)MapViewOfFile(m_graphics.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFileGraphic));
    if (!m_graphics.mapFileBuffer)
    {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void initStatic()
{
    TCHAR szName[] = TEXT("Local\\acpmf_static");
    m_static.hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SPageFileStatic), szName);
    if (!m_static.hMapFile)
    {
        MessageBoxA(GetActiveWindow(), "CreateFileMapping failed", "ACCS", MB_OK);
    }
    m_static.mapFileBuffer = (unsigned char*)MapViewOfFile(m_static.hMapFile, FILE_MAP_READ, 0, 0, sizeof(SPageFileStatic));
    if (!m_static.mapFileBuffer)
    {
        MessageBoxA(GetActiveWindow(), "MapViewOfFile failed", "ACCS", MB_OK);
    }
}

void dismiss(SMElement element)
{
    UnmapViewOfFile(element.mapFileBuffer);
    CloseHandle(element.hMapFile);
}

void printData(std::string name, float value)
{
    std::wcout << name.c_str() << " : " << value << std::endl;
}

template <typename T, unsigned S>
inline void printData(const std::string name, const T(&v)[S])
{
    std::wcout << name.c_str() << " : ";

    for (int i = 0; i < S; i++)
    {
        std::wcout << v[i];
        if (i < S - 1)
        {
            std::wcout << " , ";
        }

    }
    std::wcout << std::endl;
}

template <typename T, unsigned S, unsigned S2>
inline void printData2(const std::string name, const T(&v)[S][S2])
{
    std::wcout << name.c_str() << " : ";

    for (int i = 0; i < S; i++)
    {
        std::wcout << i << " : ";
        for (int j = 0; j < S2; j++) {
            std::wcout << v[i][j];
            if (j < S2 - 1)
            {
                std::wcout << " , ";
            }
        }

        std::wcout << ";" << std::endl;

    }

}

int _tmain(int argc, _TCHAR* argv[])
{
    initPhysics();
    initGraphics();
    initStatic();

    std::wcout << "Press 1 for physics, 2 for graphics, 3 for static" << std::endl;

    while (true)
    {
        if (GetAsyncKeyState(0x31) != 0) // user pressed 1
        {
            std::wcout << "---------------PHYSICS INFO---------------" << std::endl;
            SPageFilePhysics* pf = (SPageFilePhysics*)m_physics.mapFileBuffer;
            printData("acc G", pf->accG);
            printData("brake", pf->brake);
            printData("camber rad", pf->camberRAD);
            printData("damage", pf->carDamage);
            printData("car height", pf->cgHeight);
            printData("drs", pf->drs);
            printData("tc", pf->tc);
            printData("fuel", pf->fuel);
            printData("gas", pf->gas);
            printData("gear", pf->gear);
            printData("number of tyres out", pf->numberOfTyresOut);
            printData("packet id", pf->packetId);
            printData("heading", pf->heading);
            printData("pitch", pf->pitch);
            printData("roll", pf->roll);
            printData("rpms", pf->rpms);
            printData("speed kmh", pf->speedKmh);
            printData2("contact point", pf->tyreContactPoint);
            printData2("contact normal", pf->tyreContactNormal);
            printData2("contact heading", pf->tyreContactHeading);
            printData("steer ", pf->steerAngle);
            printData("suspension travel", pf->suspensionTravel);
            printData("tyre core temp", pf->tyreCoreTemperature);
            printData("tyre dirty level", pf->tyreDirtyLevel);
            printData("tyre wear", pf->tyreWear);
            printData("velocity", pf->velocity);
            printData("wheel angular speed", pf->wheelAngularSpeed);
            printData("wheel load", pf->wheelLoad);
            printData("wheel slip", pf->wheelSlip);
            printData("wheel pressure", pf->wheelsPressure);
        }

        if (GetAsyncKeyState(0x32) != 0) // user pressed 2
        {
            std::wcout << "---------------GRAPHICS INFO---------------" << std::endl;
            SPageFileGraphic* pf = (SPageFileGraphic*)m_graphics.mapFileBuffer;
            printData("packetID ", pf->packetId);
            printData("STATUS ", pf->status);
            printData("session", pf->session);
            printData("completed laps", pf->completedLaps);
            printData("position", pf->position);
            printData("current time s", pf->currentTime);
            printData("current time", pf->iCurrentTime);
            printData("last time s", pf->lastTime);
            printData("last time ", pf->iLastTime);
            printData("best time s", pf->bestTime);
            printData("best time", pf->iBestTime);
            printData("sessionTimeLeft", pf->sessionTimeLeft);
            printData("distanceTraveled", pf->distanceTraveled);
            printData("isInPit", pf->isInPit);
            printData("currentSectorIndex", pf->currentSectorIndex);
            printData("lastSectorTime", pf->lastSectorTime);
            printData("numberOfLaps", pf->numberOfLaps);
            std::wcout << "TYRE COMPOUND : " << pf->tyreCompound << std::endl;
            printData("replayMult", pf->replayTimeMultiplier);
            printData("normalizedCarPosition", pf->normalizedCarPosition);
            printData2("carCoordinates", pf->carCoordinates);
        }


        if (GetAsyncKeyState(0x33) != 0) // user pressed 3
        {
            std::wcout << "---------------STATIC INFO---------------" << std::endl;
            SPageFileStatic* pf = (SPageFileStatic*)m_static.mapFileBuffer;
            std::wcout << "SM VERSION " << pf->smVersion << std::endl;
            std::wcout << "AC VERSION " << pf->acVersion << std::endl;

            printData("number of sessions ", pf->numberOfSessions);
            printData("numCars", pf->numCars);
            std::wcout << "Car model " << pf->carModel << std::endl;
            std::wcout << "Car track " << pf->track << std::endl;
            std::wcout << "Player Name " << pf->playerName << std::endl;
            printData("sectorCount", pf->sectorCount);

            printData("maxTorque", pf->maxTorque);
            printData("maxPower", pf->maxPower);
            printData("maxRpm", pf->maxRpm);
            printData("maxFuel", pf->maxFuel);
            printData("suspensionMaxTravel", pf->suspensionMaxTravel);
            printData("tyreRadius", pf->tyreRadius);

        }
    }

    dismiss(m_graphics);
    dismiss(m_physics);
    dismiss(m_static);

    return 0;
}



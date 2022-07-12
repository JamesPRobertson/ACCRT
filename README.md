# ACCRT Overview
ACCRT is a real time telemetry viewing tool created for Assetto Corsa Competizione.  


# Building and Dependencies

## Dependencies
- Visual Studio 2017
- Python 3.0 or greater
- Windows PowerShell
- Assetto Corsa Competizione -> Driver 

## Driver
The current output path is `ACCRT/src/bin/Debug/net6.0/driver_telemetry.exe`  
Please note this exe requires the dll in the same directory.  

### PowerShell Script Method
Currently, the most straightforward method to building and running the driver 
side code is to run `ACCRT/build_and_run.ps1`, this will build and run the driver 
code as long as all dependencies are satisfied.

### Visual Studio Method
Using the 'Developer PowerShell for VS 2022' terminal, one can navigate to 
`ACCRT/src/driver_telemetry` and run `msbuild` to initiate a build within 
this directory.

## Engineer
TBD


# Operation

## Driver
The driver application, once built, is located at 
`ACCRT/src/driver_telemetry/bin/Debug/net6.0/driver_telemetry.exe`  
When running this application, the user may either supply an IP and a port to run 
the 'server' on using command line arguments. If the user does not supply any 
arguments, the program will attempt to listen on all adapters that are available and 
the default port (9000).

Once the program is running, an engineer can connect to the driver using their 
public IP and the port the server is being run on.

## Engineer
TBD

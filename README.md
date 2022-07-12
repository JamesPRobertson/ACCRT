# ACCRT
Realtime Telemetry Tool for Assetto Corsa Competizione

## Building and Dependencies

### Dependencies
 - Visual Studio 2017
 - Python 3.0 or greater (temporary)
 - Windows PowerShell
 - Assetto Corsa Competizione


### Driver
The current output path is `ACCRT/src/bin/Debug/net6.0/driver_telemetry.exe`  
Please note this exe requires the dll in the same directory.  

#### PowerShell
Currently, the most straightforward method to building and running the driver 
side code is to run `ACCRT/build_and_run.ps1`, this will build and run the driver 
code as long as all dependencies are satisfied.

#### Visual Studio
Using the 'Developer PowerShell for VS 2022' terminal, one can navigate to 
`ACCRT/src/driver_telemetry` and run `msbuild` to initiate a build within 
this directory.

### Engineer
TBD

## Operation
TBD
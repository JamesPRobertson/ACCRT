################################################################################
# Build script
################################################################################
param(
    [Switch] $release,
    [switch] $detailed_logging
)

################################################################################
# Constants
################################################################################
if($release) {
    $CONFIG = "Release"
}
else {
    $CONFIG = "Debug"
}

if($detailed_logging) {
    $LOG_LEVEL = "detailed"
}
else {
    $LOG_LEVEL = "normal"
}

# driver_telemetry constants
$DT_SLN_DIR = "$PWD\src\driver_telemetry"
$DT_OUTPUT_DIR = "$DT_SLN_DIR\bin\$CONFIG\net6.0"
$DT_EXECUTABLE_NAME = "driver_telemetry.exe"

$DT_BUILD_LOG_NAME = "latest_build.log"
$DT_BUILD_LOG_LOCATION = "$DT_SLN_DIR\$DT_BUILD_LOG_NAME"
$DT_MSBUILD_COMMAND = "msbuild /p:Configuration=$CONFIG /m -flp:logfile=$DT_BUILD_LOG_LOCATION``;verbosity=$LOG_LEVEL"


# buildtools constants
$BUILD_TOOLS_DIR = "$PWD\src\.buildtools"
$NUGET_DOWNLOAD_PATH = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$NUGET_SOURCES_ADD_COMMAND_OFFICIAL = "nuget sources add -Name `"NuGet official package source`" -Source `"https://api.nuget.org/v3/index.json`""

$NUGET_COMMAND = "nuget restore"

################################################################################
# Build script
################################################################################
$main_build_script = {    
    if (($Null -eq $env:VSINSTALLDIR) || (Get-Command "nuget" -ErrorAction SilentlyContinue) -eq $null) {
        Initialize-DevPowershell
    }
    else {
        Write-Host "Already in Visual Studio Developer Powershell mode, continuing with build process."
    }

    Write-Host "Starting Build"
    # TODO: Add additional build functions for other modules here.
    Build-DriverTelemetry
}

################################################################################
# Build functions
################################################################################

function Build-DriverTelemetry {
    Push-Location $DT_SLN_DIR

    Invoke-Expression $NUGET_COMMAND
    Invoke-Expression $DT_MSBUILD_COMMAND
    $msbuild_exitcode = $LASTEXITCODE
    
    Pop-Location
    
    if ($msbuild_exitcode -eq 0) {
        Invoke-Expression  "$DT_OUTPUT_DIR\$DT_EXECUTABLE_NAME"
    }
    else {
        Write-Host "Build failed. Log located at $DT_BUILD_LOG_LOCATION"
    }
}

################################################################################
# Helper Functions
################################################################################

function Initialize-DevPowershell {
    Write-Host "Enabling Visual Studio Developer Powershell mode."
    if ((Test-Path $BUILD_TOOLS_DIR) -eq $False) {
        New-Item $BUILD_TOOLS_DIR -ItemType Directory
    }

    $vswhere_path = "$BUILD_TOOLS_DIR\vswhere.exe"

    if ((Test-Path $vswhere_path) -eq $false) {
        Write-Host "vswhere.exe not found, downloading latest release from github"
        $repo = "Microsoft/vswhere"
        $file = "vswhere.exe"
        $releases = "https://api.github.com/repos/$repo/releases"
        
        Write-Host "Determining latest release of vswhere.exe"
        $tag = (Invoke-WebRequest $releases | ConvertFrom-Json)[0].tag_name
        
        $download = "https://github.com/$repo/releases/download/$tag/$file"
        
        Invoke-WebRequest $download -Out $vswhere_path
    }

    $installationPath = Invoke-Expression "$vswhere_path -prerelease -latest -property installationPath"
    if ($Null -eq $installationPath) {
        Throw "Error: Visual Studio not installed or not found."
    }
    else {
      & "${env:COMSPEC}" /s /c "`"$installationPath\Common7\Tools\vsdevcmd.bat`" -no_logo && set" | foreach-object {
        $name, $value = $_ -split '=', 2
        Set-Content env:\"$name" $value
      }
    }

    if ((Get-Command "nuget" -ErrorAction SilentlyContinue) -eq $null) {
        Invoke-WebRequest $NUGET_DOWNLOAD_PATH -out "$BUILD_TOOLS_DIR\nuget.exe"
    }

    Set-Content env:path "$env:path;$BUILD_TOOLS_DIR"

    Invoke-Expression $NUGET_SOURCES_ADD_COMMAND_OFFICIAL
}

& $main_build_script

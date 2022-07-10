################################################################################
# Build script
################################################################################
param(
    [Switch] $release,
    [switch] $detailed_logging
)

$START_DIR = "$PWD"

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

$SLN_DIR = "$PWD\src\driver_telemetry"
$OUTPUT_DIR = "$SLN_DIR\bin\$CONFIG\net6.0"
$EXECUTABLE_NAME = "driver_telemetry.exe"

$BUILD_LOG_NAME = "latest_build.log"
$BUILD_LOG_LOCATION = "$SLN_DIR\$BUILD_LOG_NAME"

$BUILD_TOOLS_DIR = "$SLN_DIR\.buildtools"
$NUGET_DOWNLOAD_PATH = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

$NUGET_COMMAND = "nuget restore"
$MSBUILD_COMMAND = "msbuild /p:Configuration=$CONFIG -flp:logfile=$BUILD_LOG_LOCATION``;verbosity=$LOG_LEVEL"

if ((Test-Path $BUILD_TOOLS_DIR) -eq $False) {
    New-Item $BUILD_TOOLS_DIR -ItemType Directory
}

if ($Null -eq $env:VSINSTALLDIR) {
    Write-Host "Enabling Visual Studio Developer Powershell mode."
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
}
else {
    Write-Host "Already in Visual Studio Developer Powershell mode, continuing with build process."
}

if ($null -eq (Get-Command "nuget" -ErrorAction SilentlyContinue)) {
    Invoke-WebRequest $NUGET_DOWNLOAD_PATH -out "$BUILD_TOOLS_DIR\nuget.exe"
}

Write-Host "Starting Build"

Set-Content env:path "$env:path;$BUILD_TOOLS_DIR"

Set-Location $SLN_DIR

Invoke-Expression $NUGET_COMMAND
Invoke-Expression $MSBUILD_COMMAND
$msbuild_exitcode = $LASTEXITCODE

Set-Location $start_dir

if ($msbuild_exitcode -eq 0) {
    Invoke-Expression  "$OUTPUT_DIR\$EXECUTABLE_NAME"
}
else {
    Write-Host "Build failed. Log located at $BUILD_LOG_LOCATION"
}

################################################################################
# Build script
################################################################################
param(
    [Switch] $release
)

$START_DIR = "$PWD"

$CONFIG = ($release) ? "Release" : "Debug"
$SLN_DIR = "$pwd\src\driver_telemetry"
$OUTPUT_DIR = "$SLN_DIR\bin\$CONFIG\net6.0"
$EXECUTABLE_NAME = "driver_telemetry.exe"

$BUILD_LOG_NAME = "latest_build.log"
$BUILD_LOG_LOCATION = "$SLN_DIR\$BUILD_LOG_NAME"

$MSBUILD_COMMAND = "msbuild /p:Configuration=$CONFIG"

if ($Null -eq $env:VSINSTALLDIR) {
    Write-Host "Enabling Visual Studio Developer Powershell mode."
    $vswhere_path = "$PWD\.vscode\vswhere.exe"

    if ((Test-Path $vswhere_path) -eq $false) {
        Write-Host "vswhere.exe not found, downloading latest release from github"
        $repo = "Microsoft/vswhere"
        $file = "vswhere.exe"
        $releases = "https://api.github.com/repos/$repo/releases"
        
        Write-Host Determining latest release of vswhere.exe
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
        set-content env:\"$name" $value
      }
    }
}
else {
    Write-Host "Already in Visual Studio Developer Powershell mode, continuing with build process."
}

Write-Host "Starting Build"

Set-Location $SLN_DIR

Start-Transcript -Path $BUILD_LOG_LOCATION

Invoke-Expression $MSBUILD_COMMAND
$msbuild_exitcode = $LASTEXITCODE
Stop-Transcript | Out-Null

Set-Location $start_dir

if ($msbuild_exitcode -eq 0) {
    Invoke-Expression  "$OUTPUT_DIR\$EXECUTABLE_NAME"
}
else {
    Write-Host "Build failed. Log located at $BUILD_LOG_LOCATION"
}
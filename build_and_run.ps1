################################################################################
# Build script
################################################################################
param(
    [Switch] $release
)

$start_dir = "$PWD"

$config = ($release) ? "Release" : "Debug"
$sln_dir = "$pwd\src\driver_telemetry"
$output_dir = "$sln_dir\bin\$config\net6.0"
$executable_name = "driver_telemetry.exe"

$msbuild_command = "msbuild /p:Configuration=$config"

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

Set-Location $sln_dir

Invoke-Expression $msbuild_command

Set-Location $start_dir

if ($LASTEXITCODE -eq 0) {
    Invoke-Expression  "$output_dir\$executable_name"
}
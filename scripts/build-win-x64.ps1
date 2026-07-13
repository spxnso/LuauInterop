$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$Root      = Split-Path -Parent $ScriptDir
$BuildDir  = Join-Path $Root "build"
$OutDir    = Join-Path $Root "src\LuauInterop.Native\runtimes\win-x64\native"

# Clean and recreate build dir
if (Test-Path $BuildDir) {
    Remove-Item $BuildDir -Recurse -Force
}
New-Item $BuildDir -ItemType Directory | Out-Null

Push-Location $BuildDir
try {
    cmake $Root -G "Visual Studio 18 2026" -A x64
    cmake --build . --config Release

    if (-not (Test-Path $OutDir)) {
        New-Item $OutDir -ItemType Directory | Out-Null
    }

    Copy-Item -Path "Release\luau.dll" -Destination "$OutDir\luau.dll" -Force

    Write-Host "Done! luau.dll copied to $OutDir"
}
finally {
    Pop-Location
    Remove-Item $BuildDir -Recurse -Force
}
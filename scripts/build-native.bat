@echo off
setlocal

set "SCRIPT_DIR=%~dp0"

for %%I in ("%SCRIPT_DIR%..") do set "ROOT=%%~fI"

set "BUILD_DIR=%ROOT%\build"

if exist "%BUILD_DIR%" rmdir /s /q "%BUILD_DIR%"
mkdir "%BUILD_DIR%"

cd /d "%BUILD_DIR%"


cmake "%ROOT%" -G "Visual Studio 18 2026" -A x64
cmake --build . --config Release

set "OUT_DIR=%ROOT%\src\Luau.Native\runtimes\win-x64\native"

if not exist "%OUT_DIR%" (
    mkdir "%OUT_DIR%"
)

copy /y "Release\luau.dll" "%OUT_DIR%\luau.dll"

echo Done! luau.dll copied to %OUT_DIR%

:: Cleanup
cd /d "%ROOT%"
rmdir /s /q "%BUILD_DIR%"
@echo off
setlocal
set "ROOT=%~dp0"

:: Create an empty glue.c file because it is required by CMakeLists.txt
echo // hi luau > "%ROOT%native\luau_interop.c"

:: Build
if exist "%ROOT%build" rmdir /s /q "%ROOT%build"
mkdir "%ROOT%build"
cd /d "%ROOT%build"

cmake .. -G "Visual Studio 17 2022" -A x64
cmake --build . --config Release

:: Copy to native
if not exist "%ROOT%src\Luau.Native\runtimes\win-x64\native" (
    mkdir "%ROOT%src\Luau.Native\runtimes\win-x64\native"
)
copy /y "Release\luau.dll" "%ROOT%src\Luau.Native\runtimes\win-x64\native\luau.dll"

echo Done! luau.dll copied to src\Luau.Native\runtimes\win-x64\native\

:: Cleanup
cd /d "%ROOT%"
rmdir /s /q "%ROOT%build"
del "%ROOT%native\luau_interop.c"
cmake_minimum_required(VERSION 3.10)
project(LuauInterop)

set(CMAKE_POSITION_INDEPENDENT_CODE ON)

set(LUAU_BUILD_SHARED OFF CACHE BOOL "" FORCE)
set(LUAU_EXTERN_C ON CACHE BOOL "" FORCE)
set(LUAU_BUILD_CLI OFF CACHE BOOL "" FORCE)
set(LUAU_BUILD_TESTS OFF CACHE BOOL "" FORCE)
set(LUAU_BUILD_WEB OFF CACHE BOOL "" FORCE)

add_subdirectory(${CMAKE_SOURCE_DIR}/external/luau)

add_library(luau SHARED ${CMAKE_SOURCE_DIR}/glue.c)

target_link_libraries(luau PRIVATE
    Luau.VM
    Luau.Compiler
    Luau.CodeGen
    Luau.Common
)

if(MSVC)
    target_link_options(luau PRIVATE
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.VM>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Compiler>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.CodeGen>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Common>
    )
elseif(APPLE)
    target_link_options(luau PRIVATE
        -Wl,-force_load,$<TARGET_FILE:Luau.VM>
        -Wl,-force_load,$<TARGET_FILE:Luau.Compiler>
        -Wl,-force_load,$<TARGET_FILE:Luau.CodeGen>
        -Wl,-force_load,$<TARGET_FILE:Luau.Common>
    )
else()
    target_link_options(luau PRIVATE
        -Wl,--whole-archive
        $<TARGET_FILE:Luau.VM>
        $<TARGET_FILE:Luau.Compiler>
        $<TARGET_FILE:Luau.CodeGen>
        $<TARGET_FILE:Luau.Common>
        -Wl,--no-whole-archive
    )
endif()
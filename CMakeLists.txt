cmake_minimum_required(VERSION 3.10)
project(LuauInterop)

set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)

set(LUAU_BUILD_SHARED OFF CACHE BOOL "" FORCE)
set(LUAU_EXTERN_C ON CACHE BOOL "" FORCE)
set(LUAU_BUILD_CLI OFF CACHE BOOL "" FORCE)
set(LUAU_BUILD_TESTS OFF CACHE BOOL "" FORCE)
set(LUAU_BUILD_WEB OFF CACHE BOOL "" FORCE)

add_subdirectory(${CMAKE_SOURCE_DIR}/external/luau)

add_library(luau SHARED ${CMAKE_SOURCE_DIR}/glue.cpp)

target_include_directories(luau PRIVATE
    ${CMAKE_SOURCE_DIR}/external/luau/Common/include
    ${CMAKE_SOURCE_DIR}/external/luau/Ast/include
    ${CMAKE_SOURCE_DIR}/external/luau/Analysis/include
    ${CMAKE_SOURCE_DIR}/external/luau/VM/include
)

target_link_libraries(luau PRIVATE
    Luau.VM
    Luau.Compiler
    Luau.CodeGen
    Luau.Common
    Luau.Ast
    Luau.Analysis
)

if(MSVC)
    target_link_options(luau PRIVATE
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.VM>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Compiler>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.CodeGen>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Common>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Ast>
        /WHOLEARCHIVE:$<TARGET_FILE:Luau.Analysis>
        /DEF:${CMAKE_SOURCE_DIR}/luau.def
    )
elseif(APPLE)
    target_link_options(luau PRIVATE
        -Wl,-force_load,$<TARGET_FILE:Luau.VM>
        -Wl,-force_load,$<TARGET_FILE:Luau.Compiler>
        -Wl,-force_load,$<TARGET_FILE:Luau.CodeGen>
        -Wl,-force_load,$<TARGET_FILE:Luau.Common>
        -Wl,-force_load,$<TARGET_FILE:Luau.Ast>
        -Wl,-force_load,$<TARGET_FILE:Luau.Analysis>
    )
else()
    target_link_options(luau PRIVATE
        -Wl,--whole-archive
        $<TARGET_FILE:Luau.VM>
        $<TARGET_FILE:Luau.Compiler>
        $<TARGET_FILE:Luau.CodeGen>
        $<TARGET_FILE:Luau.Common>
        $<TARGET_FILE:Luau.Ast>
        $<TARGET_FILE:Luau.Analysis>
        -Wl,--no-whole-archive
    )
endif()
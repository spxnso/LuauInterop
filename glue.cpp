#include <string.h>
#include <stdlib.h>
#include "external/luau/Common/include/Luau/Common.h"
#include "external/luau/VM/include/lua.h"

static int csharp_trampoline(lua_State* L)
{
    void* ptr = lua_tolightuserdata(L, lua_upvalueindex(1));
    
    typedef int(*ManagedCallback)(lua_State*);
    ManagedCallback fn = (ManagedCallback)ptr;
    int result = fn(L);

    // Catch the error.
    if (result == -1)
        lua_error(L);

    return result;
}

extern "C"
{
    void luau_free(void* ptr)
    {
        free(ptr);
    }

    void luau_setfflag(const char* name, int value)
    {
        for (Luau::FValue<bool>* flag = Luau::FValue<bool>::list; flag; flag = flag->next)
        {
            if (strcmp(flag->name, name) == 0)
            {
                flag->value = (bool)value;
                return;
            }
        }
    }

    int luau_getfflag(const char* name)
    {
        for (Luau::FValue<bool>* flag = Luau::FValue<bool>::list; flag; flag = flag->next)
        {
            if (strcmp(flag->name, name) == 0)
                return flag->value;
        }

        return -1;
    }

    void luau_pushcsharpfunc(lua_State* L, void* fnPtr)
    {
        lua_pushlightuserdata(L, fnPtr);
        lua_pushcclosurek(L, csharp_trampoline, "luauinterop", 1, nullptr);
    }
}
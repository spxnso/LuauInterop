#include <string.h>
#include <stdlib.h>
#include "external/luau/Common/include/Luau/Common.h"


extern "C"
{
    void lua_free(void* ptr)
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
}
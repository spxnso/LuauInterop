#include <stdlib.h>

// By doing this, we can avoid having to link against the C runtime library, which can cause issues when used in different environments (e.g., .NET on Windows vs Linux).
void lua_free(void* ptr) {
    free(ptr);
}
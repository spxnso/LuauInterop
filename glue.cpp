#include "external/luau/VM/include/lua.h"

#include "Luau/AstJsonEncoder.h"
#include "Luau/ParseResult.h"
#include "Luau/Parser.h"
#include "Luau/ToString.h"

#include "Luau/Common.h"
#include "Luau/Allocator.h"
#include "Luau/Lexer.h"
#include "Luau/ParseOptions.h"

#include <stdlib.h>
#include <string.h>
#include <string>


static int csharp_trampoline(lua_State* L)
{
    void* ptr = lua_tolightuserdata(L, lua_upvalueindex(1));

    typedef int (*ManagedCallback)(lua_State*);
    ManagedCallback fn = (ManagedCallback)ptr;
    int result = fn(L);

    // Catch the error.
    if (result == -1)
        lua_error(L);

    return result;
}

extern "C"
{
    void free(void* ptr)
    {
        free(ptr);
    }

    void cpp_delete(char* ptr)
    {
        delete[] ptr;
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

    // This is pretty much a direct copy of the CLI.
    // Cf. https://github.com/luau-lang/luau/blob/51e08625ad7983901fd2bf8a1c7f88f26d144b8e/CLI/src/Ast.cpp
    char* luau_parse(const char* source, size_t sourceLength)
    {
        Luau::Allocator allocator;
        Luau::AstNameTable names(allocator);

        Luau::ParseOptions options;
        options.captureComments = true;
        options.allowDeclarationSyntax = true;

        Luau::ParseResult parseResult = Luau::Parser::parse(source, sourceLength, names, allocator, options);

        std::string json = Luau::toJson(parseResult.root, parseResult.commentLocations);

        // Append errors to json output
        json.pop_back();
        json += ",\"errors\":[";
        for (size_t i = 0; i < parseResult.errors.size(); ++i)
        {
            if (i > 0)
                json += ",";
            const auto& e = parseResult.errors[i];
            json += "{\"location\":\"";
            json += Luau::toString(e.getLocation());
            json += "\",\"message\":\"";
            json += e.what();
            json += "\"}";
        }
        json += "]}";

        char* result = new char[json.size() + 1];

        if (!result)
            return nullptr;

        memcpy(result, json.c_str(), json.size() + 1);

        return result;
    };
}
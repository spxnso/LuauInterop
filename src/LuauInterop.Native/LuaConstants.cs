namespace LuauInterop.Native;

/// <summary>
/// Static class containing constants used in the Luau C API.
/// </summary>
public static class LuaConstants
{
    public const int LUAI_MAXCSTACK = 8000;
    public const int LUA_MULTRET = -1;

    public const int LUA_REGISTRYINDEX = -10000; // -LUAI_MAXCSTACK - 2000
    public const int LUA_ENVIRONINDEX = -10001;  // -LUAI_MAXCSTACK - 2001
    public const int LUA_GLOBALSINDEX = -10002;  // -LUAI_MAXCSTACK - 2002
}
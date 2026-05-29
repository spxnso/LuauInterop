namespace LuauInterop;

/// <summary>
/// Represents the type of a Lua value.
/// </summary>
public enum LuauType
{
    Nil = 0,
    Boolean = 1,
    LightUserData = 2,
    Number = 3,
    Integer = 4,
    Vector = 5,
    String = 6,
    Table = 7,
    Function = 8,
    UserData = 9,
    Thread = 10,
    Buffer = 11,
}
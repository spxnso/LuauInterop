namespace LuauInterop.Runtime;

[Flags]
public enum LuauLibrary
{
    Base = 1,
    Coroutine = 1 << 1,
    Table = 1 << 2,
    OS = 1 << 3,
    String = 1 << 4,
    Bit32 = 1 << 5,
    Buffer = 1 << 6,
    Utf8 = 1 << 7,
    Math = 1 << 8,
    Debug = 1 << 9,
    Vector = 1 << 10,
    Integer = 1 << 11,
    All = Base | Coroutine | Table | OS | String | Bit32 | Buffer | Utf8 | Math | Debug | Vector | Integer
}
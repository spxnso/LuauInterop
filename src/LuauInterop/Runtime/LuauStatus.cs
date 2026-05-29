namespace LuauInterop.Runtime;

public enum LuauStatus
{
    OK = 0,
    Yield = 1,
    ErrRun = 2,
    ErrSyntax = 3, // legacy error code, preserved for compatibility
    ErrMem = 4,
    ErrErr = 5,
    Break = 6, // yielded for a debug breakpoint
}
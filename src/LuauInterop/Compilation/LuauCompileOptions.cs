namespace LuauInterop.Compilation;

/// <summary>
/// Represents the options for compiling a Luau chunk.
/// </summary>
public sealed class LuauCompileOptions
{
    /// <summary>
    /// Sets Luau compiler optimization level.
    /// Possible values are:
    /// 0 - no optimizations
    /// 1 - baseline optimization level that doesn’t prevent debuggability (default)
    /// 2 - aggressive optimizations that may make debugging difficult
    /// </summary>
    public int OptimizationLevel { get; init; } = 1;

    /// <summary>
    /// Sets Luau compiler debug level.
    /// Possible values are:
    /// 0 - no debug information emitted
    /// 1 - line info and function names only; sufficient for backtraces (default)
    /// 2 - full debug info with local and upvalue names; necessary for debuggers
    /// </summary>
    public int DebugLevel { get; init; } = 1;

    /// <summary>
    /// Sets Luau type information level used to guide native code generation decisions.
    /// Possible values are:
    /// 0 - generate for native modules (default)
    /// 1 - generate for all modules
    /// </summary>
    public int TypeInfoLevel { get; init; } = 0;

    /// <summary>
    /// Sets Luau compiler code coverage level.
    /// Possible values are:
    /// 0 - no code coverage support (default)
    /// 1 - statement coverage
    /// 2 - statement and expression coverage (verbose)
    /// </summary>
    public int CoverageLevel { get; init; } = 0;
}
using LuauInterop.Runtime;

namespace LuauInterop.Sandbox;

public record LuauSandboxOptions
{
    /// <summary>
    /// Libraries to open before sandboxing. Anything not listed here
    /// will be completely absent from the script's environment.
    /// </summary>
    public LuauLibrary AllowedLibraries { get; init; } = LuauLibrary.None;

    /// <summary>
    /// Allows libraries even if a preset denies them.
    /// </summary>
    public LuauSandboxOptions AllowLibrary(LuauLibrary library) => this with
    {
        AllowedLibraries = AllowedLibraries | library
    };

    /// <summary>
    /// Allows a global even if a preset denies it.
    /// </summary>
    public LuauSandboxOptions AllowGlobal(string global) => this with
    {
        ForbiddenGlobals = [.. ForbiddenGlobals.Where(g => g != global)],
    };

    /// <summary>
    /// Allows globals even if a preset denies them. 
    /// </summary>
    public LuauSandboxOptions AllowGlobals(IReadOnlyList<string> globals) => this with
    {
        ForbiddenGlobals = [.. ForbiddenGlobals.Where(g => !globals.Contains(g))],
    };

    /// <summary>
    /// Globals to remove from the environment even if their library is open.
    /// Useful for stripping individual functions like <c>rawset</c>, <c>setfenv</c>.
    /// </summary>
    public IReadOnlyList<string> ForbiddenGlobals { get; init; } = [];


    /// <summary>
    /// Denies a global even if a preset allows it.
    /// </summary>
    public LuauSandboxOptions DenyGlobal(string global) => this with
    {
        ForbiddenGlobals = [.. ForbiddenGlobals.Concat([global]).Distinct()],
    };

    /// <summary>
    /// Denies globals even if a preset allows them.
    /// </summary>
    public LuauSandboxOptions DenyGlobals(IReadOnlyList<string> globals) => this with
    {
        ForbiddenGlobals = [.. ForbiddenGlobals.Concat(globals).Distinct()],
    };

    /// <summary>
    /// Whether each <see cref="LuauSandbox.Execute"/> call gets its own
    /// proxied <c>_G</c> via <see cref="NativeMethods.luaL_sandboxthread"/>
    /// Disabling allows scripts to share globals across calls (not recommended).
    /// </summary>
    public bool IsolateScripts { get; init; } = true;

    /// <summary>
    /// Whether to call <see cref="NativeMethods.luaL_sandbox"/> to make all builtins read-only.
    /// Disabling this is not recommended.
    /// </summary>
    public bool LockGlobals { get; init; } = true;

    /// <summary>
    /// Default sandbox options.
    /// </summary>
    public static LuauSandboxOptions Default { get; } = new() {

    };

    /// <summary>
    /// Strict preset for fully untrusted code.
    /// </summary>
    public static LuauSandboxOptions Strict { get; } = new()
    {
        AllowedLibraries = LuauLibrary.Base | LuauLibrary.Coroutine | LuauLibrary.Math | LuauLibrary.String | LuauLibrary.Table,
        ForbiddenGlobals = ["setfenv", "getfenv", "rawset", "rawget", "rawequal", "rawlen", "newproxy", "collectgarbage"],
        LockGlobals = true,
        IsolateScripts = true,
    };
}
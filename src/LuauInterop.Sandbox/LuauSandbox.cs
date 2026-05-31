using LuauInterop.Native;
using LuauInterop.Runtime;

namespace LuauInterop.Sandbox;

public class LuauSandbox : IDisposable
{
    /// <summary>
    /// Whether this object has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// The options used to create this sandbox.
    /// </summary>
    public LuauSandboxOptions Options { get; }

    /// <summary>
    /// The Luau object associated with this sandbox.
    /// </summary>
    public Luau Luau { get; }

    public LuauSandbox(LuauSandboxOptions? options = null)
    {
        Options = options ?? LuauSandboxOptions.Default;
        Luau = new Luau();

        Luau.OpenLibrary(Options.AllowedLibraries);
        RemoveDeniedGlobals();

        if (Options.LockGlobals)
            NativeMethods.luaL_sandbox(Luau.State);
    }

    ~LuauSandbox()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        Luau.Dispose();
        IsDisposed = true;
    }


    public object?[] Execute(string source, string name = "script")
    {
        using LuauThread thread = Luau.CreateThread();

        if (Options.IsolateScripts)
            NativeMethods.luaL_sandboxthread(thread.CoroutineState);

        using LuauChunk chunk = Luau.Compile(source);
        return thread.Resume(chunk);
    }

    private void RemoveDeniedGlobals()
    {
        foreach (string name in Options.ForbiddenGlobals)
            Luau[name] = null;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, GetType().Name);
    }
}
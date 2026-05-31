using LuauInterop.Native;

namespace LuauInterop;

/// <summary>
/// Base class for all Luau objects that hold a reference to the Lua state.
/// </summary>
public abstract class LuauBase(Luau owner, LuaState state, int reference) : IDisposable
{
    /// <summary>
    /// The <see cref="Luau"/> instance that owns this object.
    /// </summary>
    public Luau Owner { get; } = owner;

    /// <summary>
    /// The <see cref="LuaState"/> that this object is associated with.
    /// </summary>
    public LuaState State { get; } = state;

    /// <summary>
    /// The reference to the Lua object in the registry.
    /// </summary>
    public int Reference { get; } = reference;

    /// <summary>
    /// Whether this object has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    ~LuauBase()
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

        if (!Owner.IsDisposed && !Owner.State.IsNull)
            Owner.State.Unref(Reference);

        IsDisposed = true;
    }

    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(
            IsDisposed || Owner.IsDisposed || Owner.State.IsNull,
            GetType().Name
        );
    }

    public void PushReference()
    {
        ThrowIfDisposed();
        Owner.State.RawGetI(LuaConstants.LUA_REGISTRYINDEX, Reference);
    }
}

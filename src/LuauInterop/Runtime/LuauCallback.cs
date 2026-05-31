using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using LuauInterop.Native;

namespace LuauInterop.Runtime;

/// <summary>
/// A C# function exposed to Luau.
/// </summary>
public class LuauCallback : IDisposable
{
    [ThreadStatic]
    internal static Exception? PendingException;

    /// <summary> 
    /// The function pointer to pass to Luau.
    /// </summary>
    public IntPtr FunctionPointer => Marshal.GetFunctionPointerForDelegate(Native);

    /// <summary>
    /// The managed function that this callback wraps.
    /// </summary>
    public Func<Luau, LuaState, int> Managed { get; }

    /// <summary>
    /// The <see cref="Luau"/> instance that owns this object.
    /// </summary>
    public Luau Owner { get; }

    /// <summary>
    /// The native delegate that wraps the managed function.
    /// </summary>
    public delegate int NativeDelegate(LuaState state);

    /// <summary>
    /// The native delegate instance, which is passed to Luau.
    /// </summary>
    public NativeDelegate Native { get; }

    /// <summary>
    /// The pinned handle for the native delegate, to prevent GC collection.
    /// </summary>
    public GCHandle Pin { get; }

    /// <summary>
    /// Whether this object has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    public LuauCallback(Luau owner, Func<Luau, LuaState, int> managed)
    {
        Owner = owner;
        Managed = managed;

        Native = state =>
        {
            try
            {
                // we set the pending exception to null before calling the managed function
                // to ensure that any exception thrown by the managed function is properly propagated
                // and not accidentally treated as a leftover exception from a previous callback.
                var result = Managed(Owner, state);
                PendingException = null;
                return result;
            }
            catch (Exception ex)
            {
                PendingException = ex;
                state.PushString(ex.Message);
                return -1;
            }
        };

        Pin = GCHandle.Alloc(Native);
    }

    ~LuauCallback()
    {
        Dispose();
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

        if (Pin.IsAllocated)
            Pin.Free();

        IsDisposed = true;
    }
}
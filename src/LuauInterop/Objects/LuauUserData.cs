using System.Runtime.InteropServices;

using LuauInterop.Native;

namespace LuauInterop.Objects;

public sealed class LuauUserData(Luau owner, LuaState state, int reference) : LuauBase(owner, state, reference)
{
    public nint Pointer
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = State.GetTop();
            try
            {
                PushReference();
                return State.ToUserdata(-1);
            }
            finally
            {
                State.SetTop(stackBase);
            }
        }
    }

    public int Tag
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = State.GetTop();
            try
            {
                PushReference();
                return State.UserdataTag(-1);
            }
            finally
            {
                State.SetTop(stackBase);
            }
        }
    }

    public T Read<T>() where T : unmanaged
    {
        ThrowIfDisposed();

        nint ptr = Pointer;
        if (ptr == nint.Zero)
            throw new InvalidOperationException("Userdata pointer is null.");

        if (Marshal.SizeOf<T>() > int.MaxValue)
            throw new InvalidOperationException("Type too large.");

        return Marshal.PtrToStructure<T>(ptr);
    }
}
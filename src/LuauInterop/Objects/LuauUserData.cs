using System.Runtime.InteropServices;

namespace LuauInterop.Objects;

public sealed class LuauUserData(Luau owner, int reference) : LuauBase(owner, reference)
{
    public IntPtr Pointer
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                return Owner.State.ToUserdata(-1);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
    }

    public int Tag
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                return Owner.State.UserdataTag(-1);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
    }

    public T Read<T>() where T : unmanaged
    {
        ThrowIfDisposed();

        IntPtr ptr = Pointer;
        if (ptr == IntPtr.Zero)
            throw new InvalidOperationException("Userdata pointer is null.");

        if (Marshal.SizeOf<T>() > int.MaxValue)
            throw new InvalidOperationException("Type too large.");

        return Marshal.PtrToStructure<T>(ptr);
    }
}
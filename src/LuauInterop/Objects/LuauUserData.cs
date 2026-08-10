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

    /// <summary>
    /// Gets the number of bytes actually allocated for this userdata's payload,
    /// as reported by the Lua runtime (<c>lua_objlen</c>).
    /// </summary>
    /// <remarks>
    /// This is the authoritative size of the native block backing this userdata.
    /// <see cref="Read{T}"/> validates against this value before reading.
    /// </remarks>
    public int Size
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = State.GetTop();
            try
            {
                PushReference();
                return State.ObjLen(-1);
            }
            finally
            {
                State.SetTop(stackBase);
            }
        }
    }

    /// <summary>
    /// Reads this userdata's payload as a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the userdata's pointer is null, or if the userdata's actual
    /// allocated size (<see cref="Size"/>) is smaller than <c>sizeof(T)</c>.
    /// This guards against out-of-bounds reads.
    /// </exception>
    public T Read<T>() where T : unmanaged
    {
        ThrowIfDisposed();
        int stackBase = State.GetTop();

        try
        {
            PushReference();

            int allocatedSize = State.ObjLen(-1);
            int requiredSize = Marshal.SizeOf<T>();

            if (requiredSize > allocatedSize)
                throw new InvalidOperationException("Cannot read out-of-bounds.");

            nint ptr = State.ToUserdata(-1);
            if (ptr == nint.Zero)
                throw new InvalidOperationException("Userdata pointer is null.");

            return Marshal.PtrToStructure<T>(ptr);
        }
        finally
        {
            State.SetTop(stackBase);
        }
    }

    /// <summary>
    /// Attempts to read this userdata's payload as a value of type
    /// <typeparamref name="T"/>, returning <see langword="false"/> instead of
    /// throwing if the userdata is too small or its pointer is null.
    /// </summary>
    public bool TryRead<T>(out T value) where T : unmanaged
    {
        ThrowIfDisposed();
        int stackBase = State.GetTop();

        try
        {
            PushReference();

            int allocatedSize = State.ObjLen(-1);
            int requiredSize = Marshal.SizeOf<T>();
            nint ptr = State.ToUserdata(-1);

            if (requiredSize > allocatedSize || ptr == nint.Zero)
            {
                value = default;
                return false;
            }

            value = Marshal.PtrToStructure<T>(ptr);
            return true;
        }
        finally
        {
            State.SetTop(stackBase);
        }
    }
}
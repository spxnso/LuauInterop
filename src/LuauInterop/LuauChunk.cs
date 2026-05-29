using System.Runtime.InteropServices;

using LuauInterop.Native;


namespace LuauInterop;

/// <summary>
/// Represents a compiled Luau chunk, which is a block of bytecode that can be loaded into a Lua State.
/// </summary>
public class LuauChunk(IntPtr pointer, UIntPtr size) : IDisposable
{
    /// <summary>
    /// The pointer to the compiled chunk in unmanaged memory.
    /// </summary>
    public IntPtr Pointer { get; private set; } = pointer;

    /// <summary>
    /// The size of the compiled chunk in bytes.
    /// </summary>
    public UIntPtr Size { get; } = size;

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LuauChunk()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        if (Pointer != IntPtr.Zero)
        {
            NativeMethods.lua_free(Pointer);
            Pointer = IntPtr.Zero;
        }

        IsDisposed = true;
    }

    /// <summary>
    /// Returns the compiled chunk as a read-only span of bytes.
    /// </summary>
    public unsafe Span<byte> AsSpan()
    {
        ThrowIfDisposed();

        return (ulong)Size > int.MaxValue
            ? throw new InvalidOperationException("Chunk too large.")
            : new Span<byte>((void*)Pointer, (int)Size);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, GetType().Name);
    }

    /// <summary>
    /// Returns the compiled chunk as a byte array.
    /// </summary>
    public byte[] ToByteArray()
    {
        ThrowIfDisposed();

        byte[] bytecode = new byte[(int)Size];
        Marshal.Copy(Pointer, bytecode, 0, (int)Size);

        return bytecode;
    }

}

using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

using LuauInterop.Compilation;
using LuauInterop.Native;
using LuauInterop.Objects;
using LuauInterop.Runtime;

namespace LuauInterop;

/// <summary>
/// Represents a Luau instance.
/// </summary>
public class Luau : IDisposable
{
    /// <summary>
    /// List of callbacks registered in this instance.
    /// </summary>
    public readonly List<LuauCallback> Callbacks = [];

    /// <summary>
    /// Gets the underlying native Luau state.
    /// </summary>
    public LuaState State { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Initializes a new Luau instance and allocates a fresh native state.
    /// </summary>
    /// <exception cref="OutOfMemoryException">Thrown if the native state could not be allocated.</exception>
    public Luau()
    {
        State = new(NativeMethods.luaL_newstate());
        if (State.IsNull)
            throw new OutOfMemoryException("Failed to allocate Luau state.");
    }

    /// <summary>
    /// Gets or sets a global variable by name.
    /// </summary>
    /// <param name="name">The name of the global variable.</param>
    /// <returns>The value of the global, or <see langword="null"/> if it is nil.</returns>
    public object? this[string name]
    {
        get
        {
            ThrowIfDisposed();
            try
            {
                LuauType type = (LuauType)State.GetGlobal(name);
                return GetObject(-1);
            }
            finally
            {
                Pop(1);
            }
        }
        set
        {
            ThrowIfDisposed();

            if (value is Delegate del)
            {
                var wrapper = new LuauDelegate(this, del);
                var cb = new LuauCallback(this, (vm, state) => wrapper.Invoke(state));
                Callbacks.Add(cb);
                NativeMethods.luau_pushcsharpfunc(State.Handle, cb.FunctionPointer);
                State.SetGlobal(name);
                return;
            }

            PushObject(value);
            State.SetGlobal(name);
        }
    }

    ~Luau() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed) return;
        IsDisposed = true;

        if (disposing)
            foreach (LuauCallback cb in Callbacks)
                cb.Dispose();

        if (!State.IsNull)
        {
            State.Close();
            State = default;
        }
    }

    /// <summary>
    /// Compiles a Luau source string into a <see cref="LuauChunk"/>.
    /// </summary>
    /// <param name="chunk">The Luau source code to compile.</param>
    /// <param name="options">Optional compiler options. Pass <see langword="null"/> to use defaults.</param>
    /// <returns>A compiled <see cref="LuauChunk"/>. Dispose it after use.</returns>
    public LuauChunk Compile(string chunk, LuauCompileOptions? options = null) => LuauCompiler.Compile(chunk, options);

    /// <summary>
    /// Executes a pre-compiled <see cref="LuauChunk"/>, returning any values it produces.
    /// </summary>
    /// <param name="chunk">The compiled chunk to execute.</param>
    /// <param name="chunkName">A name for the chunk used in error messages. Defaults to <c>"chunk"</c>.</param>
    /// <returns>An array of values returned by the chunk, or an empty array if it returns nothing.</returns>
    /// <exception cref="LuauException">Thrown if execution fails.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public object?[] DoChunk(LuauChunk chunk, string chunkName = "chunk")
    {
        ThrowIfDisposed();
        int stackBase = State.GetTop();

        if ((LuauStatus)State.Load(chunkName, chunk.Pointer, chunk.Size, 0) != LuauStatus.OK)
            ThrowLastError();

        return CallAndCollect(stackBase);
    }

    /// <summary>
    /// Compiles and executes a Luau source string, returning any values it produces.
    /// </summary>
    /// <param name="chunk">The Luau source code to execute.</param>
    /// <param name="chunkName">A name for the chunk used in error messages. Defaults to <c>"chunk"</c>.</param>
    /// <returns>An array of values returned by the chunk, or an empty array if it returns nothing.</returns>
    /// <exception cref="LuauException">Thrown if compilation or execution fails.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public object?[] DoString(string chunk, string chunkName = "chunk")
    {
        ThrowIfDisposed();
        int stackBase = State.GetTop();

        if (LoadString(chunk, chunkName) != LuauStatus.OK)
            ThrowLastError();

        return CallAndCollect(stackBase);
    }

    /// <summary>
    /// Gets the error message from the top of the stack, if any, and pops it off the stack. If there is no error message, returns a default message.
    /// </summary>
    /// <returns>The error message from the top of the stack, or a default message if there is no error message.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public string GetErrorMessage(LuaState state)
    {
        ThrowIfDisposed();
        string message = "Unknown error";

        try
        {
            IntPtr ptr = state.ToLString(state.AbsIndex(-1), out _);
            if (ptr != IntPtr.Zero)
                message = Marshal.PtrToStringUTF8(ptr) ?? message;
        }
        finally
        {
            Pop(1, state);
        }

        return message;
    }

    /// <inheritdoc cref="GetErrorMessage(LuaState)"/>
    public string GetErrorMessage() => GetErrorMessage(State);

    /// <summary>
    /// Gets the value of an FFlag in the Luau VM.
    /// </summary>
    /// <param name="name">The name of the FFlag to get.</param>
    /// <returns><see langword="true"/> if the FFlag is enabled, or <see langword="false"/> if it is disabled.</returns>
    public bool GetFFlag(string name)
    {
        ThrowIfDisposed();
        return NativeMethods.luau_getfflag(name) != 0;
    }


    /// <summary>
    /// Gets the value at the specified index on the stack, converting it to an appropriate C# type.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <param name="state">The Lua state to use for retrieving the value.</param>
    /// <returns>The value at the specified index, converted to an appropriate C# type.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public object? GetObject(int index, LuaState state)
    {
        LuauType type = (LuauType)state.Type(index);

        return type switch
        {
            LuauType.Nil => null,
            LuauType.Boolean => state.ToBoolean(index),
            LuauType.Number => state.ToNumber(index),
            LuauType.Integer => state.ToInteger64(index, out _),
            LuauType.String => Marshal.PtrToStringUTF8(state.ToLString(index, out _)),
            LuauType.Function => new LuauFunction(this, state.Ref(index)),
            LuauType.Table => new LuauTable(this, state.Ref(index)),
            LuauType.UserData => new LuauUserData(this, state.Ref(index)),
            LuauType.Vector => ReadVector(index, state),
            LuauType.Buffer => ReadBuffer(index, state),
            LuauType.Thread => new LuauThread(this, state.Ref(index)),
            _ => throw new NotSupportedException($"Unsupported Luau type: {type}")
        };
    }

    /// <inheritdoc cref="GetObject(int, LuaState)"/>
    public object? GetObject(int index) => GetObject(index, State);

    /// <summary>
    /// Gets the value at the specified index on the stack.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <returns>The value at the specified index.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public LuauValue GetValue(int index, LuaState state)
    {
        ThrowIfDisposed();

        LuauType type = (LuauType)state.Type(index);

        switch (type)
        {
            case LuauType.Nil: return LuauValue.Nil;
            case LuauType.Boolean: return state.ToBoolean(index);
            case LuauType.Number: return state.ToNumber(index);
            case LuauType.Integer: return state.ToInteger64(index, out _);
            case LuauType.String:
                IntPtr ptr = state.ToLString(index, out UIntPtr len);
                string? s = ptr == IntPtr.Zero ? null : Marshal.PtrToStringUTF8(ptr, (int)len);
                return new LuauValue(LuauType.String, number: 0, integer: 0, reference: s);
            case LuauType.Function:
                return new LuauValue(LuauType.Function, number: state.Ref(index), integer: 0, reference: this);
            default:
                throw new NotSupportedException($"Unsupported Lua type: {type}");
        }
    }
    
    /// <inheritdoc cref="GetValue(int, LuaState)"/>
    public LuauValue GetValue(int index) => GetValue(index, State);

    /// <summary>
    /// Compiles a Luau source string and loads it onto the stack as a callable function,
    /// without executing it.
    /// </summary>
    /// <param name="chunk">The Luau source code to compile and load.</param>
    /// <param name="chunkName">A name for the chunk used in error messages. Defaults to <c>"chunk"</c>.</param>
    /// <returns><see cref="LuauStatus.OK"/> on success, or an error status if compilation or loading fails.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public LuauStatus LoadString(string chunk, string chunkName = "chunk")
    {
        ThrowIfDisposed();
        using var compiled = Compile(chunk);
        return (LuauStatus)State.Load(chunkName, compiled.Pointer, compiled.Size, 0);
    }

    /// <summary>
    /// Compiles a Luau source string and loads it onto the stack as a callable function,
    /// without executing it.
    /// </summary>
    /// <param name="chunk">The Luau source code to compile and load.</param>
    /// <param name="targetState">The Lua state to load the compiled function onto. If the current state is different from the target state, the compiled function will be moved to the target state after loading.</param>
    /// <param name="chunkName">A name for the chunk used in error messages. Defaults to <c>"chunk"</c>.</param>
    /// <returns><see cref="LuauStatus.OK"/> on success, or an error status if compilation or loading fails.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public LuauStatus LoadString(string chunk, LuaState targetState, string chunkName = "chunk")
    {
        ThrowIfDisposed();
        using var compiled = Compile(chunk);
        LuauStatus status = (LuauStatus)State.Load(chunkName, compiled.Pointer, compiled.Size, 0);
        if (status == LuauStatus.OK)
            State.XMove(targetState, 1);
        return status;
    }

    /// <summary>
    /// Creates a new Luau coroutine thread.
    /// </summary>
    /// <returns>A <see cref="LuauThread"/> that can be resumed independently.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public LuauThread CreateThread()
    {
        ThrowIfDisposed();
        State.NewThread();
        int reference = State.Ref(-1);
        Pop(1);
        return new LuauThread(this, reference);
    }

    /// <summary>
    /// Opens all standard Luau libraries into this state.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void OpenLibraries()
    {
        ThrowIfDisposed();

        State.OpenLibraries();
    }

    /// <summary>
    /// Opens all the specified Luau libraries into this state.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void OpenLibraries(List<LuauLibrary> libraries)
    {
        ThrowIfDisposed();

        foreach (LuauLibrary library in libraries)
            OpenLibrary(library);
    }


    /// <summary>
    /// Opens the specified standard Luau libraries into this state.
    /// </summary>
    /// <param name="library">The libraries to open.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void OpenLibrary(LuauLibrary library)
    {
        ThrowIfDisposed();

        if (library.HasFlag(LuauLibrary.Base)) OpenBase();
        if (library.HasFlag(LuauLibrary.Bit32)) OpenBit32();
        if (library.HasFlag(LuauLibrary.Buffer)) OpenBuffer();
        if (library.HasFlag(LuauLibrary.Coroutine)) OpenCoroutine();
        if (library.HasFlag(LuauLibrary.Debug)) OpenDebug();
        if (library.HasFlag(LuauLibrary.Integer)) OpenInteger();
        if (library.HasFlag(LuauLibrary.Math)) OpenMath();
        if (library.HasFlag(LuauLibrary.OS)) OpenOS();
        if (library.HasFlag(LuauLibrary.String)) OpenString();
        if (library.HasFlag(LuauLibrary.Table)) OpenTable();
        if (library.HasFlag(LuauLibrary.Utf8)) OpenUtf8();
        if (library.HasFlag(LuauLibrary.Vector)) OpenVector();
    }

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenBase() => State.OpenBase();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenBit32() => State.OpenBit32();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenBuffer() => State.OpenBuffer();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenCoroutine() => State.OpenCoroutine();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenDebug() => State.OpenDebug();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenInteger() => State.OpenInteger();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenMath() => State.OpenMath();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenOS() => State.OpenOS();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenString() => State.OpenString();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenTable() => State.OpenTable();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenUtf8() => State.OpenUtf8();

    /// <inheritdoc cref="OpenLibraries"/>
    public int OpenVector() => State.OpenVector();


    /// <summary>
    /// Pops <paramref name="n"/> values off the top of the stack.
    /// </summary>
    /// <param name="n">The number of values to pop.</param>
    /// <param name="state">The Lua state to pop values from.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void Pop(int n, LuaState state)
    {
        ThrowIfDisposed();
        state.SetTop(-n - 1);
    }

    /// <inheritdoc cref="Pop(int, LuaState)"/>
    public void Pop(int n) => Pop(n, State);

    /// <summary>
    /// Pushes a <see cref="LuauValue"/> onto the stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    /// <param name="state">The Lua state to push the value onto.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void Push(LuauValue value, LuaState state)
    {
        ThrowIfDisposed();
        PushValue(value, state);
    }

    /// <inheritdoc cref="Push(LuauValue, LuaState)"/>
    public void Push(LuauValue value) => Push(value, State);

    /// <summary>
    /// Pushes a C# function onto the stack.
    /// </summary>
    public void PushCallback(LuauCallback callback, LuaState state)
    {
        ThrowIfDisposed();
        NativeMethods.luau_pushcsharpfunc(state.Handle, callback.FunctionPointer);
    }

    /// <inheritdoc cref="PushCallback(LuauCallback, LuaState)"/>
    public void PushCallback(LuauCallback callback) => PushCallback(callback, State);

    /// <summary>
    /// Pushes a C# object onto the stack, converting it to an appropriate Luau type.
    /// </summary>
    /// <param name="value">The object to push.</param>
    /// <param name="state">The Lua state to push the object onto.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    public void PushObject(object? value, LuaState state)
    {
        ThrowIfDisposed();
        switch (value)
        {
            case LuauBase luauBase:
                if (!ReferenceEquals(luauBase.Owner, this))
                    throw new InvalidOperationException("Cross-VM usage is not allowed.");
                luauBase.PushReference();
                break;

            case LuauVector vector:
                state.PushVector(vector.X, vector.Y, vector.Z);
                break;

            case LuauBuffer buffer:
                IntPtr ptr = state.NewBuffer((UIntPtr)buffer.Length);
                if (buffer.Length > 0)
                    Marshal.Copy(buffer.Data, 0, ptr, buffer.Length);
                break;

            case Func<Luau, int> fn:
                var cb = new LuauCallback(this, (vm, state) => fn(vm));
                Callbacks.Add(cb);
                NativeMethods.luau_pushcsharpfunc(state.Handle, cb.FunctionPointer);
                break;
            case Delegate del:
                var wrapper = new LuauDelegate(this, del);
                var callback = new LuauCallback(this, (vm, s) => wrapper.Invoke(s));
                Callbacks.Add(callback);
                NativeMethods.luau_pushcsharpfunc(state.Handle, callback.FunctionPointer);
                break;
            default:
                PushValue(FromObject(value), state);
                break;
        }
    }

    /// <inheritdoc cref="PushObject(object?, LuaState)"/>
    public void PushObject(object? value) => PushObject(value, State);

    /// <summary>
    /// Pushes a Luau value onto the stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    /// <param name="state">The Lua state to push the value onto.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed.</exception>
    internal void PushValue(LuauValue value, LuaState state)
    {
        ThrowIfDisposed();
        switch (value.Type)
        {
            case LuauType.Nil: state.PushNil(); break;
            case LuauType.Boolean: state.PushBoolean(value.Number != 0); break;
            case LuauType.Number: state.PushNumber(value.Number); break;
            case LuauType.Integer: state.PushInteger64(value.Integer); break;
            case LuauType.String:
                if (value.Reference is not string str)
                    throw new InvalidOperationException("Luau string value has no backing string.");
                state.PushString(str);
                break;
            default:
                throw new NotSupportedException($"Cannot push LuauValue of type {value.Type}.");
        }
    }

    /// <inheritdoc cref="PushValue(LuauValue, LuaState)"/>
    public void PushValue(LuauValue value) => PushValue(value, State);

    /// <summary>
    /// Registers a C# function as a global in the Luau state, making it callable from Luau code.
    /// </summary>
    public LuauCallback RegisterCallback(string name, Func<Luau, LuaState, int> fn)
    {
        ThrowIfDisposed();

        var callback = new LuauCallback(this, (vm, state) => fn(vm, state));
        Callbacks.Add(callback);

        NativeMethods.luau_pushcsharpfunc(State.Handle, callback.FunctionPointer);
        State.SetGlobal(name);

        return callback;
    }

    public LuauCallback RegisterCallback(string name, Func<Luau, int> fn) => RegisterCallback(name, (vm, _) => fn(vm));

    /// <summary>
    /// Sets an FFlag in the Luau VM. FFlags are used to enable or disable experimental features.
    /// </summary>
    /// <param name="name">The name of the FFlag to set.</param>
    /// <param name="enabled">A value indicating whether the FFlag should be enabled.</param>
    public void SetFFlag(string name, bool enabled)
    {
        ThrowIfDisposed();
        NativeMethods.luau_setfflag(name, enabled ? 1 : 0);
    }

    internal object?[] CollectResults(int stackBase, LuaState state)
    {
        int count = state.GetTop() - stackBase;
        var results = new object?[count];

        try
        {
            for (int i = 0; i < count; i++)
                results[i] = GetObject(stackBase + i + 1, state);
        }
        finally
        {
            state.SetTop(stackBase);
        }

        return results;
    }

    /// <inheritdoc cref="CollectResults(int, LuaState)"/>
    public object?[] CollectResults(int stackBase) => CollectResults(stackBase, State);

    internal void ThrowIfDisposed()
    {
        if (IsDisposed || State.IsNull)
            throw new ObjectDisposedException(nameof(Luau));
    }

    internal void ThrowLastError(LuaState state)
    {
        throw new LuauException(GetErrorMessage(state));
    }

    internal void ThrowLastError() => ThrowLastError(State);

    private object?[] CallAndCollect(int stackBase)
    {
        if ((LuauStatus)State.PCall(0, LuaConstants.LUA_MULTRET, 0) != LuauStatus.OK)
        {
            var pending = LuauCallback._pendingException;
            LuauCallback._pendingException = null;

            GetErrorMessage(); // Clear the error message from the stack

            if (pending is not null)
                throw pending;

            ThrowLastError();
        }

        return CollectResults(stackBase);
    }

    private static LuauValue FromObject(object? value)
    {
        if (value is null) return LuauValue.Nil;
        if (value is LuauValue v) return v;

        return value switch
        {
            bool b => b,
            string s => s,
            char c => c.ToString(),
            sbyte i => (double)i,
            byte i => (double)i,
            short i => (double)i,
            ushort i => (double)i,
            int i => (double)i,
            uint i => (double)i,
            long i => i,
            ulong i => i <= (ulong)long.MaxValue
                           ? (long)i
                           : throw new OverflowException("ulong too large for Luau integer"),
            float f => (double)f,
            double d => d,
            decimal d => (double)d,

            _ => throw new NotSupportedException($"Unsupported CLR type: {value.GetType()}")
        };
    }

    private LuauBuffer ReadBuffer(int index) => ReadBuffer(index, State);

    private LuauBuffer ReadBuffer(int index, LuaState state)
    {
        IntPtr ptr = state.ToBuffer(index, out UIntPtr len);
        if (ptr == IntPtr.Zero || len == UIntPtr.Zero)
            return new LuauBuffer([]);

        if (len.ToUInt64() > int.MaxValue)
            throw new OverflowException("Luau buffer too large for managed array.");

        int length = (int)len.ToUInt64();
        byte[] data = new byte[length];
        Marshal.Copy(ptr, data, 0, length);
        return new LuauBuffer(data);
    }

    private LuauVector ReadVector(int index) => ReadVector(index, State);

    private LuauVector ReadVector(int index, LuaState state)
    {
        IntPtr ptr = state.ToVector(index);
        if (ptr == IntPtr.Zero) return default;

        float[] values = new float[3];
        Marshal.Copy(ptr, values, 0, 3);
        return new LuauVector(values[0], values[1], values[2]);
    }
}

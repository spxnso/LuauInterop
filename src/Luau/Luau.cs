using System.Runtime.InteropServices;
using System.Text;
using Luau.Compilation;
using Luau.Native;
using Luau.Objects;
using Luau.Runtime;

namespace Luau
{
    public class Luau : IDisposable
    {
        public LuaState State { get; private set; }

        public bool IsDisposed { get; private set; }

        public Luau()
        {
            State = new(NativeMethods.luaL_newstate());
            if (State.IsNull)
                throw new OutOfMemoryException("Failed to allocate Luau state.");
        }

        public object? this[string name]
        {
            get
            {
                ThrowIfDisposed();

                try
                {
                    State.GetGlobal(name);
                    var value = GetObject(-1);
                    return value;
                }
                finally
                {
                    Pop(1);
                }
            }
            set
            {
                ThrowIfDisposed();

                PushObject(value);
                State.SetGlobal(name);
            }
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

            IsDisposed = true;

            if (!State.IsNull)
            {
                State.Close();
                State = default;
            }
        }
        
        public LuauChunk Compile(string chunk, LuauCompileOptions? options = null)
        {
            return LuauCompiler.Compile(chunk, options);
        }

        public object?[] DoString(string chunk, string chunkName = "chunk")
        {
            ThrowIfDisposed();

            int stackBase = State.GetTop();

            LuauStatus loadStatus = LoadString(chunk, chunkName);
            if (loadStatus != LuauStatus.OK)
                ThrowLastError();

            LuauStatus callStatus = (LuauStatus)State.PCall(0, LuaConstants.LUA_MULTRET, 0);
            if (callStatus != LuauStatus.OK)
                ThrowLastError();

            int resultCount = State.GetTop() - stackBase;

            var results = new object?[resultCount];

            for (int i = 0; i < resultCount; i++)
            {
                results[i] = GetObject(stackBase + i + 1);
            }

            Pop(resultCount);

            return results;
        }


        private static LuauValue FromObject(object? value)
        {
            if (value is null)
                return LuauValue.Nil;

            if (value is LuauValue luauValue)
                return luauValue;

            return value switch
            {
                bool b => b,
                string s => s,
                char c => c.ToString(),

                sbyte i => (long)i,
                byte i => (long)i,
                short i => (long)i,
                ushort i => (long)i,
                int i => (long)i,
                long i => i,

                uint i => (long)i,

                ulong i => i <= (ulong)long.MaxValue
                    ? (long)i
                    : throw new OverflowException("ulong too large for Luau integer"),

                float f => (double)f,
                double d => d,

                decimal d => (double)d,

                _ => throw new NotSupportedException(
                    $"Unsupported CLR type: {value.GetType()}")
            };
        }

        // TODO: Cover more types
        public LuauValue GetValue(int index)
        {
            ThrowIfDisposed();

            LuauType type = (LuauType)State.Type(index);

            switch (type)
            {
                case LuauType.Nil:
                    return LuauValue.Nil;
                case LuauType.Boolean:
                    return State.ToBoolean(index);
                case LuauType.Number:
                    return State.ToNumber(index);
                case LuauType.Integer:
                    return State.ToInteger64(index, out _);
                case LuauType.String:
                    IntPtr ptr = State.ToLString(index, out UIntPtr len);
                    string? s = ptr == IntPtr.Zero ? null : Marshal.PtrToStringUTF8(ptr, (int)len);
                    return new LuauValue(LuauType.String, number: 0, integer: 0, reference: s);
                case LuauType.Function:
                    int reference = State.Ref(index);
                    return new LuauValue(LuauType.Function, number: reference, integer: 0, reference: this);
                default:
                    throw new NotSupportedException($"Unsupported Lua type: {type}");
            }

        }

        public LuauStatus LoadString(string chunk, string chunkName = "chunk")
        {
            ThrowIfDisposed();

            using var compiledChunk = Compile(chunk);

            return (LuauStatus)State.Load(chunkName, compiledChunk.Pointer, compiledChunk.Size, 0);
        }

        public LuauThread CreateThread()
        {
            ThrowIfDisposed();

            State.NewThread();
            int reference = State.Ref(-1);
            Pop(1);

            return new LuauThread(this, reference);
        }


        public void OpenLibraries()
        {
            ThrowIfDisposed();
            State.OpenLibraries();
        }

        public void Pop(int n)
        {
            ThrowIfDisposed();
            State.SetTop(-n - 1);
        }

        public void Push(LuauValue value)
        {
            ThrowIfDisposed();

            switch (value.Type)
            {
                case LuauType.Nil:
                    State.PushNil();
                    break;
                case LuauType.Boolean:
                    State.PushBoolean(value.Number != 0);
                    break;
                case LuauType.Number:
                    State.PushNumber(value.Number);
                    break;
                case LuauType.Integer:
                    State.PushInteger64(value.Integer);
                    break;
                case LuauType.String:
                    if (value.Reference is not string str)
                        throw new InvalidOperationException("Luau string value has no backing string.");
                    State.PushString(str);
                    break;
            }
        }

        internal void PushObject(object? value)
        {
            if (value is LuauBase luauBase)
            {
                if (!ReferenceEquals(luauBase.Owner, this))
                    throw new InvalidOperationException("Cross-VM usage is not allowed.");

                luauBase.PushReference();
                return;
            }

            if (value is LuauVector vector)
            {
                State.PushVector(vector.X, vector.Y, vector.Z);
                return;
            }

            if (value is LuauBuffer buffer)
            {
                IntPtr ptr = State.NewBuffer((UIntPtr)buffer.Length);
                if (buffer.Length > 0)
                    Marshal.Copy(buffer.Data, 0, ptr, buffer.Length);
                return;
            }

            Push(FromObject(value));
        }

        internal object? GetObject(int index)
        {
            ThrowIfDisposed();

            LuauType type = (LuauType)State.Type(index);

            switch (type)
            {
                case LuauType.Nil:
                    return null;
                case LuauType.Boolean:
                    return State.ToBoolean(index);
                case LuauType.Number:
                    return State.ToNumber(index);
                case LuauType.Integer:
                    return State.ToInteger64(index, out _);
                case LuauType.String:
                    IntPtr ptr = State.ToLString(index, out _);
                    return ptr == IntPtr.Zero ? null : Marshal.PtrToStringUTF8(ptr);
                case LuauType.Function:
                    int reference = State.Ref(index);
                    return new LuauFunction(this, reference);
                case LuauType.Table:
                    int tableReference = State.Ref(index);
                    return new LuauTable(this, tableReference);
                case LuauType.UserData:
                    int userdataReference = State.Ref(index);
                    return new LuauUserData(this, userdataReference);
                case LuauType.Vector:
                    return ReadVector(index);
                case LuauType.Buffer:
                    return ReadBuffer(index);
                case LuauType.Thread:
                    int threadReference = State.Ref(index);
                    return new LuauThread(this, threadReference);
                default:
                    throw new NotSupportedException($"Unsupported Lua type: {type}");
            }
        }

        private LuauVector ReadVector(int index)
        {
            IntPtr ptr = State.ToVector(index);
            if (ptr == IntPtr.Zero)
                return default;

            float[] values = new float[3];
            Marshal.Copy(ptr, values, 0, values.Length);
            return new LuauVector(values[0], values[1], values[2]);
        }

        private LuauBuffer ReadBuffer(int index)
        {
            IntPtr ptr = State.ToBuffer(index, out UIntPtr len);
            if (ptr == IntPtr.Zero || len == UIntPtr.Zero)
                return new LuauBuffer(Array.Empty<byte>());

            if (len.ToUInt64() > int.MaxValue)
                throw new OverflowException("Luau buffer too large for managed array.");

            int length = (int)len.ToUInt64();
            byte[] data = new byte[length];
            Marshal.Copy(ptr, data, 0, length);
            return new LuauBuffer(data);
        }

        internal void ThrowLastError()
        {
            ThrowIfDisposed();

            string message = GetErrorMessage();

            throw new LuauException(message);
        }

        internal string GetErrorMessage()
        {
            int errorIndex = State.AbsIndex(-1);
            string message = "Unknown error";

            IntPtr errorPtr = State.ToLString(errorIndex, out _);
            if (errorPtr != IntPtr.Zero)
                message = Marshal.PtrToStringUTF8(errorPtr) ?? message;

            return message;
        }

        private void ThrowIfDisposed()
        {
            if (State.IsNull || IsDisposed)
                throw new ObjectDisposedException(nameof(Luau));
        }
    }
}
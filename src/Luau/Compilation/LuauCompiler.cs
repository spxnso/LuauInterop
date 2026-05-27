using System.Runtime.InteropServices;
using System.Text;
using Luau.Native;

namespace Luau.Compilation
{
    public static class LuauCompiler
    {
        public static LuauChunk Compile(string chunk, LuauCompileOptions? options = null)
        {
            if (chunk is null)
                throw new ArgumentNullException(nameof(chunk));

            options ??= new LuauCompileOptions();

            IntPtr sourcePtr = Marshal.StringToCoTaskMemUTF8(chunk);

            try
            {
                using var nativeOptions = new NativeCompileOptions(options);
                UIntPtr size = (UIntPtr)Encoding.UTF8.GetByteCount(chunk);


                IntPtr bytecode = NativeMethods.luau_compile(
                    sourcePtr,
                    size,
                    nativeOptions.Pointer,
                    out UIntPtr outsize);

                if (bytecode == IntPtr.Zero)
                    throw new LuauException("Compilation failed.");


                return new LuauChunk(bytecode, outsize);

            }
            catch (Exception ex)
            {
                throw new LuauException("An error occurred during compilation.", ex);
            }
            finally
            {
                Marshal.FreeCoTaskMem(sourcePtr);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeCompileOptionsStruct
        {
            public int OptimizationLevel;
            public int DebugLevel;
            public int TypeInfoLevel;
            public int CoverageLevel;

            // These are ignored for now, but we need to keep them here to maintain the correct struct layout.
            // TODO: Implement these options in the future.

            public IntPtr VectorLib;
            public IntPtr VectorCtor;
            public IntPtr VectorType;

            public IntPtr MutableGlobals;
            public IntPtr UserdataTypes;
            public IntPtr LibrariesWithKnownMembers;

            public IntPtr LibraryMemberTypeCb;
            public IntPtr LibraryMemberConstantCb;

            public IntPtr DisabledBuiltins;
        }

        private sealed class NativeCompileOptions : IDisposable
        {
            public IntPtr Pointer { get; }

            public NativeCompileOptions(LuauCompileOptions options)
            {
                if (options is null)
                    throw new ArgumentNullException(nameof(options));

                var native = new NativeCompileOptionsStruct
                {
                    OptimizationLevel = options.OptimizationLevel,
                    DebugLevel = options.DebugLevel,
                    TypeInfoLevel = options.TypeInfoLevel,
                    CoverageLevel = options.CoverageLevel
                };

                Pointer = Marshal.AllocHGlobal(Marshal.SizeOf<NativeCompileOptionsStruct>());
                Marshal.StructureToPtr(native, Pointer, false);
            }

            public void Dispose()
            {
                if (Pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(Pointer);
            }
        }
    }
}

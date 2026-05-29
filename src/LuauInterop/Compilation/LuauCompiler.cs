using System.Runtime.InteropServices;
using System.Text;
using LuauInterop.Native;

namespace LuauInterop.Compilation;

/// <summary>
/// Provides functionality to compile Luau source code into bytecode chunks.
/// </summary>
public static class LuauCompiler
{
    /// <summary>
    /// Compiles a Luau source code chunk into a <see cref="LuauChunk"/> containing the bytecode.
    /// </summary>
    /// <param name="chunk">The Luau source code to compile.</param>
    /// <param name="options">Optional compilation options.</param>
    /// <returns>A <see cref="LuauChunk"/> containing the compiled bytecode.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input chunk is null.</exception>
    /// <exception cref="LuauException">Thrown if compilation fails or an unexpected error occurs.</exception>
    public static LuauChunk Compile(string chunk, LuauCompileOptions? options = null)
    {
        if (chunk is null)
            throw new ArgumentNullException(nameof(chunk));

        options ??= new LuauCompileOptions();

        byte[] sourceBytes = Encoding.UTF8.GetBytes(chunk);

        IntPtr sourcePtr = Marshal.AllocHGlobal(sourceBytes.Length);
        try
        {
            Marshal.Copy(sourceBytes, 0, sourcePtr, sourceBytes.Length);

            using var nativeOptions = new NativeCompileOptions(options);

            IntPtr bytecode = NativeMethods.luau_compile(
                sourcePtr,
                (UIntPtr)sourceBytes.Length,
                nativeOptions.Pointer,
                out UIntPtr outSize);

            if (bytecode == IntPtr.Zero)
                throw new LuauException("Compilation failed: luau_compile returned null.");

            
            if (Marshal.ReadByte(bytecode) == 0)
            {
                string error = Marshal.PtrToStringUTF8(bytecode + 1) ?? "Unknown compilation error.";
                NativeMethods.lua_free(bytecode);
                throw new LuauException(error);
            }

            return new LuauChunk(bytecode, outSize);
        }
        catch (LuauException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new LuauException("An unexpected error occurred during compilation.", ex);
        }
        finally
        {
            Marshal.FreeHGlobal(sourcePtr);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeCompileOptionsStruct
    {
        public int OptimizationLevel;
        public int DebugLevel;
        public int TypeInfoLevel;
        public int CoverageLevel;
        // IntPtr fields below are reserved for future use.
        // IMPORTANT: if any of these become managed types, update StructureToPtr call accordingly.
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
        public IntPtr Pointer { get; private set; }

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
            Marshal.StructureToPtr(native, Pointer, fDeleteOld: false);
        }

        public void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Pointer);
                Pointer = IntPtr.Zero;
            }
        }
    }
}
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

        nint sourcePtr = Marshal.AllocHGlobal(sourceBytes.Length);
        try
        {
            Marshal.Copy(sourceBytes, 0, sourcePtr, sourceBytes.Length);

            using var nativeOptions = new NativeCompileOptions(options);

            nint bytecode = NativeMethods.luau_compile(
                sourcePtr,
                (nuint)sourceBytes.Length,
                nativeOptions.Pointer,
                out nuint outSize);

            if (bytecode == nint.Zero)
                throw new LuauException("Compilation failed: luau_compile returned null.");

            if (Marshal.ReadByte(bytecode) == 0)
            {
                string error = Marshal.PtrToStringUTF8(bytecode + 1, (int)outSize - 1) ?? "Unknown compilation error.";
                NativeMethods.luau_free(bytecode);
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
        // nint fields below are reserved for future use.
        // IMPORTANT: if any of these become managed types, update StructureToPtr call accordingly.
        public nint VectorLib;
        public nint VectorCtor;
        public nint VectorType;
        public nint MutableGlobals;
        public nint UserdataTypes;
        public nint LibrariesWithKnownMembers;
        public nint LibraryMemberTypeCb;
        public nint LibraryMemberConstantCb;
        public nint DisabledBuiltins;
    }

    private sealed class NativeCompileOptions : IDisposable
    {
        public nint Pointer { get; private set; }

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
            if (Pointer != nint.Zero)
            {
                Marshal.FreeHGlobal(Pointer);
                Pointer = nint.Zero;
            }
        }
    }
}
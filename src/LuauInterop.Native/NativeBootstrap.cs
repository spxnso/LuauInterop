using System.Reflection;
using System.Runtime.InteropServices;

namespace LuauInterop.Native;

static partial class NativeMethods
{
    internal const string LibraryName = "luau";

    static NativeMethods()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, DllImportResolver);
    }

    static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != LibraryName)
            return IntPtr.Zero;

        string platformName = GetPlatformFileName();

        string path = Path.Combine(AppContext.BaseDirectory, "runtimes", GetPlatormSubdirectory(), "native", platformName);

        if (!File.Exists(path))
            throw new DllNotFoundException($"The native library '{platformName}' could not be found at path: {path}");

        return NativeLibrary.Load(path);
    }

    static string GetPlatformFileName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "luau.dll";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "libluau.so";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "libluau.dylib";
        else
            throw new PlatformNotSupportedException("Unsupported platform.");
    }

    static string GetPlatormSubdirectory()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "win-x64";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "linux-x64";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "osx-x64";
        else
            throw new PlatformNotSupportedException("Unsupported platform.");
    }
}
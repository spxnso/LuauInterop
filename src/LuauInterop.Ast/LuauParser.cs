using System.Runtime.InteropServices;
using System.Text.Json;

using LuauInterop.Ast.Serialization;
using LuauInterop.Native;

namespace LuauInterop.Ast;

/// <summary>
/// Parses the JSON produced by <c>luau_parse</c> and constructs the AST.
/// </summary>
public static class LuauParser
{
    public static SyntaxTree Parse(string source)
    {
        nuint sourceLength = (nuint)System.Text.Encoding.UTF8.GetByteCount(source);
        nint ptr = NativeMethods.luau_parse(source, sourceLength);

        if (ptr == nint.Zero)
            throw new OutOfMemoryException("Failed to parse source code.");

        try
        {
            string json = Marshal.PtrToStringUTF8(ptr) ?? throw new InvalidOperationException("Failed to read JSON from native memory.");
            return LuauSerializer.Deserialize(json);
        }
        finally
        {
            NativeMethods.cpp_delete(ptr);
        }
    }
}
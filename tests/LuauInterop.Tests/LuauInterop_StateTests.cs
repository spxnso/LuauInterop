using LuauInterop;
using LuauInterop.Compilation;
using LuauInterop.Runtime;

namespace LuauInterop.Tests;

public class LuauInterop_StateTests
{
    [Fact]
    public void Luau_Create_DoesNotThrow()
    {
        using var luau = new Luau();
        Assert.False(luau.IsDisposed);
    }

    [Fact]
    public void Luau_Dispose_DoesNotCrash()
    {
        var luau = new Luau();
        luau.Dispose();
        luau.Dispose(); // double dispose should not crash
    }

    [Fact]
    public void Luau_OpenLibraries_DoesNotThrow()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.All);
    }

    [Fact]
    public void LuauLibrary_NoneIsZero_BaseIsNonZero()
    {
        Assert.Equal(0, (int)LuauLibrary.None);
        Assert.NotEqual(LuauLibrary.None, LuauLibrary.Base);
        Assert.True(LuauLibrary.All.HasFlag(LuauLibrary.Base));
    }

    [Fact]
    public void DoString_ReturnsValue()
    {
        using var luau = new Luau();

        var results = luau.DoString("return 42");

        Assert.Single(results);
        Assert.Equal(42d, results[0]);
    }

    [Fact]
    public void DoString_NonStringErrorMessage_HandledGracefully()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        var exception = Assert.Throws<LuauException>(() => luau.DoString("error()")); // error with no message
        Assert.Equal("Unknown Luau-side error.", exception.Message);
    }

    [Fact]
    public void DoString_StringErrorMessage_HandledGracefully()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        var exception = Assert.Throws<LuauException>(() => luau.DoString("error('my error message')"));
        Assert.EndsWith("my error message", exception.Message);
    }

    [Fact]
    public void DoString_ErrorMessage_PreservesNullBytes()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        var exception = Assert.Throws<LuauException>(() => luau.DoString("error('aaa\\0b')"));
        Assert.EndsWith("aaa\0b", exception.Message);
    }

    [Fact]
    public void DoString_MultipleReturnValues()
    {
        using var luau = new Luau();
        var results = luau.DoString("return 1, 2, 3");

        Assert.Equal(3, results.Length);
        Assert.Equal(1d, results[0]);
        Assert.Equal(2d, results[1]);
        Assert.Equal(3d, results[2]);
    }

    [Fact]
    public void DoString_NoReturn_ReturnsEmptyArray()
    {
        using var luau = new Luau();
        var results = luau.DoString("local x = 1");

        Assert.Empty(results);
    }

    [Fact]
    public void DoString_SyntaxError_Throws()
    {
        using var luau = new Luau();

        Assert.Throws<LuauException>(() => luau.DoString("this is not valid lua!!!"));
    }

    [Fact]
    public void DoString_RuntimeError_Throws()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        Assert.Throws<LuauException>(() => luau.DoString("error('boom')"));
    }

    [Fact]
    public void DoString_AfterDispose_Throws()
    {
        var luau = new Luau();
        luau.Dispose();

        Assert.Throws<ObjectDisposedException>(() => luau.DoString("return 1"));
    }

    [Fact]
    public void Indexer_SetAndGet_RoundTrips()
    {
        using var luau = new Luau();

        luau["myVar"] = 123d;
        var result = luau["myVar"];

        Assert.Equal(123d, result);
    }

    [Fact]
    public void Indexer_SetString_RoundTrips()
    {
        using var luau = new Luau();

        luau["greeting"] = "hello";
        Assert.Equal("hello", luau["greeting"]);
    }

    [Fact]
    public void Indexer_GetUndefined_ReturnsNull()
    {
        using var luau = new Luau();

        var result = luau["doesNotExist"];

        Assert.Null(result);
    }

    [Fact]
    public void DoChunk_ExecutesPrecompiledChunk()
    {
        using var luau = new Luau();
        using var chunk = luau.Compile("return 99");

        var results = luau.DoChunk(chunk);

        Assert.Single(results);
        Assert.Equal(99d, results[0]);
    }

    [Fact]
    public void LoadString_PushesCallableFunction()
    {
        using var luau = new Luau();

        var status = luau.LoadString("return 7");

        Assert.Equal(LuauStatus.OK, status);
    }
}

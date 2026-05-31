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
    public void DoString_ReturnsValue()
    {
        using var luau = new Luau();

        var results = luau.DoString("return 42");

        Assert.Single(results);
        Assert.Equal(42.0, results[0]);
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
    public void DoString_MultipleReturnValues()
    {
        using var luau = new Luau();
        var results = luau.DoString("return 1, 2, 3");

        Assert.Equal(3, results.Length);
        Assert.Equal(1.0, results[0]);
        Assert.Equal(2.0, results[1]);
        Assert.Equal(3.0, results[2]);
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

        luau["myVar"] = 123.0;
        var result = luau["myVar"];

        Assert.Equal(123.0, result);
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
        Assert.Equal(99.0, results[0]);
    }

    [Fact]
    public void LoadString_PushesCallableFunction()
    {
        using var luau = new Luau();

        var status = luau.LoadString("return 7");

        Assert.Equal(LuauStatus.OK, status);
    }
}

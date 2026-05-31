using LuauInterop;
using LuauInterop.Compilation;
using LuauInterop.Runtime;

namespace LuauInterop.Tests;

public class LuauInterop_ThreadTests
{
    [Fact]
    public void CreateThread_DoesNotThrow()
    {
        using var luau = new Luau();
        var thread = luau.CreateThread();

        Assert.NotNull(thread);
    }

    [Fact]
    public void Resume_SimpleChunk_Executes()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("return 1 + 1");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk);

        Assert.Single(results);
        Assert.Equal(2d, results[0]);
    }

    [Fact]
    public void Resume_WithArgs_ReceivesArgs()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("local a, b = ...; return a + b");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk, 10d, 20d);

        Assert.Single(results);
        Assert.Equal(30d, results[0]);
    }

    [Fact]
    public void Resume_MultipleReturnValues()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("return 1, 2, 3");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk);

        Assert.Equal(3, results.Length);
    }

    [Fact]
    public void Resume_RuntimeError_Throws()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        using var chunk = luau.Compile("error('thread error')");
        var thread = luau.CreateThread();

        var exception = Assert.Throws<LuauException>(() => thread.Resume(chunk));
        Assert.Contains("thread error", exception.Message);
    }

    [Fact]
    public void Resume_CoroutineYield_StatusIsYield()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Coroutine);

        using var chunk = luau.Compile("coroutine.yield(42)");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk);

        Assert.Single(results);
        Assert.Equal(42d, results[0]);
        Assert.Equal(LuauCoStatus.Suspended, thread.Status);
    }

    [Fact]
    public void Resume_AfterFinished_Throws()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("return 1");
        var thread = luau.CreateThread();

        thread.Resume(chunk);

        Assert.Throws<LuauException>(() => thread.Resume());
    }

    [Fact]
    public void Resume_CanAccessGlobals()
    {
        using var luau = new Luau();

        luau["myGlobal"] = 55d;
        using var chunk = luau.Compile("return myGlobal");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk);

        Assert.Single(results);
        Assert.Equal(55d, results[0]);
    }

    [Fact]
    public void MultipleThreads_AreIndependent()
    {
        using var luau = new Luau();

        using var chunk1 = luau.Compile("return 1");
        using var chunk2 = luau.Compile("return 2");

        var t1 = luau.CreateThread();
        var t2 = luau.CreateThread();

        var r1 = t1.Resume(chunk1);
        var r2 = t2.Resume(chunk2);

        Assert.Equal(1d, r1[0]);
        Assert.Equal(2d, r2[0]);
    }

    [Fact]
    public void Thread_Status_IsFinishedAfterCompletion()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("return 1");
        var thread = luau.CreateThread();
        thread.Resume(chunk);

        Assert.Equal(LuauCoStatus.Finished, thread.Status);
    }
}
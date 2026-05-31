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
        Assert.Equal(2.0, results[0]);
    }

    [Fact]
    public void Resume_WithArgs_ReceivesArgs()
    {
        using var luau = new Luau();

        using var chunk = luau.Compile("local a, b = ...; return a + b");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk, 10.0, 20.0);

        Assert.Single(results);
        Assert.Equal(30.0, results[0]);
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

        Assert.Throws<LuauException>(() => thread.Resume(chunk));
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
        Assert.Equal(42.0, results[0]);
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

        luau["myGlobal"] = 55.0;
        using var chunk = luau.Compile("return myGlobal");
        var thread = luau.CreateThread();

        var results = thread.Resume(chunk);

        Assert.Single(results);
        Assert.Equal(55.0, results[0]);
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

        Assert.Equal(1.0, r1[0]);
        Assert.Equal(2.0, r2[0]);
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
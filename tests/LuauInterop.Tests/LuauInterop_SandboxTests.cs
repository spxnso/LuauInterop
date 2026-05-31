using LuauInterop.Runtime;
using LuauInterop.Sandbox;

namespace LuauInterop.Tests;

public class LuauInterop_SandboxTests
{
    [Fact]
    public void Execute_IsolatedScripts_DoNotShareGlobals()
    {
        var options = new LuauSandboxOptions
        {
            AllowedLibraries = LuauLibrary.None,
            LockGlobals = false,
            IsolateScripts = true
        };

        using var sandbox = new LuauSandbox(options);

        sandbox.Execute("x = 123");
        var result = sandbox.Execute("return x");

        Assert.Single(result);
        Assert.Null(result[0]);
    }

    [Fact]
    public void Execute_NonIsolatedScripts_ShareGlobals()
    {
        var options = new LuauSandboxOptions
        {
            AllowedLibraries = LuauLibrary.None,
            LockGlobals = false,
            IsolateScripts = false
        };

        using var sandbox = new LuauSandbox(options);

        sandbox.Execute("x = 123");
        var result = sandbox.Execute("return x");

        Assert.Single(result);
        Assert.Equal(123d, result[0]);
    }

    [Fact]
    public void SandboxOptions_AllowLibrary_PreservesExistingFlags()
    {
        var options = LuauSandboxOptions.Strict;

        var updated = options.AllowLibrary(LuauLibrary.OS);

        Assert.True(updated.AllowedLibraries.HasFlag(LuauLibrary.OS));
        Assert.True(updated.AllowedLibraries.HasFlag(LuauLibrary.Base));
        Assert.True(updated.AllowedLibraries.HasFlag(LuauLibrary.Table));
    }

    [Fact]
    public void SandboxOptions_AllowGlobal_RemovesForbiddenEntry()
    {
        var options = new LuauSandboxOptions
        {
            ForbiddenGlobals = new[] { "foo", "bar" }
        };

        var updated = options.AllowGlobal("foo");

        Assert.DoesNotContain("foo", updated.ForbiddenGlobals);
        Assert.Contains("bar", updated.ForbiddenGlobals);
    }

    [Fact]
    public void Execute_RemovedGlobals_AreNil()
    {
        var options = new LuauSandboxOptions
        {
            AllowedLibraries = LuauLibrary.Base | LuauLibrary.Math,
            ForbiddenGlobals = new[] { "rawset", "math" },
            LockGlobals = false,
            IsolateScripts = true
        };

        using var sandbox = new LuauSandbox(options);

        var result = sandbox.Execute("return rawset == nil, math == nil");

        Assert.Equal(2, result.Length);
        Assert.Equal(true, result[0]);
        Assert.Equal(true, result[1]);
    }

    [Fact]
    public void Execute_CallbackGlobal_IsCallable()
    {
        var options = new LuauSandboxOptions
        {
            AllowedLibraries = LuauLibrary.Base,
            LockGlobals = false,
            IsolateScripts = true
        };

        using var sandbox = new LuauSandbox(options);

        sandbox.Luau.RegisterCallback("add", (vm, state) =>
        {
            double a = state.ToNumber(1);
            double b = state.ToNumber(2);
            state.PushNumber(a + b);
            return 1;
        });

        var result = sandbox.Execute("return add(2, 3)");

        Assert.Single(result);
        Assert.Equal(5d, result[0]);
    }
}

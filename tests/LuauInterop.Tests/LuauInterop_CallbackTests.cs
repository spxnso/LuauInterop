using LuauInterop.Runtime;
using Xunit;

namespace LuauInterop.Tests;

public class LuauInterop_CallbackTests
{
    [Fact]
    public void RegisterCallback_And_Invoke_SimpleFunction()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau.RegisterCallback("add", vm =>
        {
            double a = vm.State.ToNumber(1);
            double b = vm.State.ToNumber(2);

            vm.State.PushNumber(a + b);
            return 1;
        });

        var result = luau.DoString("return add(2, 3)");

        Assert.Single(result);
        Assert.Equal(5.0, result[0]);
    }

    [Fact]
    public void RegisterCallback_NoReturn_Works()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        bool called = false;

        luau.RegisterCallback("test", vm =>
        {
            called = true;
            return 0;
        });

        luau.DoString("test()");

        Assert.True(called);
    }

    [Fact]
    public void Delegate_Action_WithArguments_Works()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        string? captured = null;

        luau["print2"] = new Action<string, string>((a, b) =>
        {
            captured = a + b;
        });

        luau.DoString("print2('hello', 'world')");

        Assert.Equal("helloworld", captured);
    }

    [Fact]
    public void Delegate_Func_ReturnsValue_ToLuau()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau["add"] = new Func<int, int, int>((a, b) => a + b);

        var result = luau.DoString("return add(10, 5)");

        Assert.Equal(15.0, result[0]);
    }

    [Fact]
    public void Delegate_MixedTypes_Coercion_Works()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau["mix"] = new Action<int, string, double>((a, b, c) =>
        {
            Assert.Equal(1, a);
            Assert.Equal("x", b);
            Assert.Equal(2.5, c);
        });

        luau.DoString("mix(1, 'x', 2.5)");
    }

    [Fact]
    public void Callback_ReturnMultipleValues_Works()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau.RegisterCallback("multi", vm =>
        {
            vm.State.PushNumber(1);
            vm.State.PushNumber(2);
            vm.State.PushNumber(3);
            return 3;
        });

        var result = luau.DoString("return multi()");

        Assert.Equal(3, result.Length);
        Assert.Equal(1.0, result[0]);
        Assert.Equal(2.0, result[1]);
        Assert.Equal(3.0, result[2]);
    }

    [Fact]
    public void Delegate_Global_Assignment_Executes()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau["x"] = new Func<int>(() => 42);

        var result = luau.DoString("return x()");

        Assert.Equal(42.0, result[0]);
    }

    [Fact]
    public void Callback_Exception_DoesNotCrash_VM()
    {
        using var luau = new Luau();
        luau.OpenLibraries();

        luau.RegisterCallback("fail", vm =>
        {
            throw new Exception("boom");
        });

        Assert.Throws<Exception>(() =>
        {
            luau.DoString("fail()");
        });
    }
}
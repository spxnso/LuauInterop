using LuauInterop.Objects;
using LuauInterop.Runtime;
using Xunit;

namespace LuauInterop.Tests;

public class LuauInterop_CallbackTests
{
    [Fact]
    public void RegisterCallback_AndInvokeSimpleFunction_Works()
    {
        using var luau = new Luau();

        luau.RegisterCallback("add", (vm, state) =>
        {
            double a = state.ToNumber(1);
            double b = state.ToNumber(2);

            state.PushNumber(a + b);
            return 1;
        });

        var result = luau.DoString("return add(2, 3)");

        Assert.Single(result);
        Assert.Equal(5d, result[0]);
    }

    [Fact]
    public void RegisterCallback_NoReturn_Works()
    {
        using var luau = new Luau();
        var called = false;

        luau.RegisterCallback("test", (vm, state) =>
        {
            called = true;
            return 0;
        });

        luau.DoString("test()");

        Assert.True(called);
    }

    [Fact]
    public void Callback_CallFromCSharp_Works()
    {
        using var luau = new Luau();

        luau.RegisterCallback("concat", (vm, state) =>
        {
            var a = vm.GetValue(1, state).ToClr() is string s1
                ? s1 : throw new InvalidOperationException("Expected string for argument 1");
            var b = vm.GetValue(2, state).ToClr() is string s2
                ? s2 : throw new InvalidOperationException("Expected string for argument 2");

            vm.PushObject(a + b, state);
            return 1;
        });

        using var thread = luau.CreateThread();
        var func = luau.DoString("return concat")[0] as LuauFunction
            ?? throw new InvalidOperationException("Expected concat to be a function");

        var results = func.Call("foo", "bar");
        Assert.Equal("foobar", results[0]);

        // We expect exactly InvalidOperationException instead of LuauException (luau code)
        // since we should properly propagate the exception
        Assert.Throws<InvalidOperationException>(() => func.Call(1, 2));
    }

    [Fact]
    public void Delegate_ActionWithArguments_Works()
    {
        using var luau = new Luau();
        string? captured = null;

        luau["print2"] = new Action<string, string>((a, b) => captured = a + b);
        luau.DoString("print2('hello', 'world')");

        Assert.Equal("helloworld", captured);
    }

    [Fact]
    public void Delegate_FuncReturnsValue_ToLuau()
    {
        using var luau = new Luau();

        luau["add"] = new Func<int, int, int>((a, b) => a + b);

        var result = luau.DoString("return add(10, 5)");

        Assert.Equal(15d, result[0]);
    }

    [Fact]
    public void Delegate_MixedTypesCoercion_Works()
    {
        using var luau = new Luau();

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

        luau.RegisterCallback("multi", (vm, state) =>
        {
            state.PushNumber(1);
            state.PushNumber(2);
            state.PushNumber(3);
            return 3;
        });

        var result = luau.DoString("return multi()");

        Assert.Equal(3, result.Length);
        Assert.Equal(1d, result[0]);
        Assert.Equal(2d, result[1]);
        Assert.Equal(3d, result[2]);
    }

    [Fact]
    public void Delegate_GlobalAssignment_Executes()
    {
        using var luau = new Luau();

        luau["x"] = new Func<int>(() => 42);

        var result = luau.DoString("return x()");

        Assert.Equal(42d, result[0]);
    }

    [Fact]
    public void Callback_Exception_DoesNotCrash()
    {
        using var luau = new Luau();

        luau.RegisterCallback("fail", (vm, state) => throw new Exception("boom"));

        var exception = Assert.Throws<Exception>(() => luau.DoString("fail()"));
        Assert.Equal("boom", exception.Message);
    }

    [Fact]
    public void Callback_Exception_PcallReturnsMessage()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        luau.RegisterCallback("fail", (vm, state) => throw new Exception("boom"));

        var result = luau.DoString("local ok, err = pcall(fail); return ok, err");

        Assert.Equal(false, result[0]);
        Assert.Equal("boom", result[1]);
    }

    [Fact]
    public void Callback_HandledException_DoesNotHaltExecution()
    {
        using var luau = new Luau();
        luau.OpenLibrary(LuauLibrary.Base);

        luau.RegisterCallback("fail", (vm, state) => throw new Exception("boom"));

        // We expect this to not throw, since the exception
        // should be caught and converted to a Lua error, which we then pcall
        luau.DoString("pcall(fail)");
    }
}
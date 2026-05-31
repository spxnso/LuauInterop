using System.Reflection;
using System.Runtime.InteropServices;

using LuauInterop.Native;

namespace LuauInterop.Runtime;

/// <summary>
/// Wraps any C# delegate for use as a Luau function.
/// Handles argument marshalling and return value pushing automatically.
/// </summary>
public sealed class LuauDelegate
{
    /// <summary>
    /// The original delegate instance.
    /// </summary>
    public Delegate Delegate { get; private set; }

    /// <summary>
    /// Whether this delegate accepts a variable number of arguments (i.e. a single parameter of type object[]).
    /// </summary>
    public bool IsVariadic { get; private set; }

    /// <summary>
    /// The <see cref="Luau"/> instance that owns this object.
    /// </summary>
    public Luau Owner { get; private set; }

    /// <summary>
    /// The parameters of the delegate method.
    /// </summary>
    public ParameterInfo[] Parameters { get; private set; }

    /// <summary>
    /// The return type of the delegate method, or null if void.
    /// </summary>
    public Type? ReturnType { get; private set; }

    public LuauDelegate(Luau owner, Delegate del)
    {
        Owner = owner;
        Delegate = del;
        Parameters = del.Method.GetParameters();
        ReturnType = del.Method.ReturnType == typeof(void) ? null : del.Method.ReturnType;

        IsVariadic = Parameters.Length == 1 && Parameters[0].ParameterType == typeof(object?[]);
    }


    public int Invoke(LuaState state)
    {
        int stackTop = state.GetTop();
        object?[] args = new object?[GetArgCount(stackTop)];

        if (IsVariadic)
        {
            for (int i = 0; i < stackTop; i++)
                args[i] = Owner.GetObject(i + 1, state);

            return PushResult(Delegate.DynamicInvoke([args]), state);
        }

        for (int i = 0; i < Parameters.Length; i++)
            args[i] = CoerceArg(
                Owner.GetObject(i + 1, state),
                Parameters[i].ParameterType
            );

        return PushResult(Delegate.DynamicInvoke(args), state);
    }

    private int GetArgCount(int stackTop)
    {
        return IsVariadic ? stackTop : Parameters.Length;
    }

    private int PushResult(object? result, LuaState state)
    {
        if (ReturnType is null)
            return 0;

        Owner.PushObject(result, state);
        return 1;
    }


    private static object? CoerceArg(object? value, Type target)
    {
        if (value is null) return null;
        if (target.IsAssignableFrom(value.GetType())) return value;


        if (target == typeof(int) && value is double d) return (int)d;
        if (target == typeof(float) && value is double d2) return (float)d2;
        if (target == typeof(long) && value is double d3) return (long)d3;
        if (target == typeof(bool) && value is double d4) return d4 != 0;
        if (target == typeof(string) && value is not string) return value.ToString();

        return Convert.ChangeType(value, target);
    }
}
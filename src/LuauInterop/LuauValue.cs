using LuauInterop.Objects;

namespace LuauInterop;

/// <summary>
/// Represents a raw Luau value.
/// </summary>
public readonly struct LuauValue(LuauType type, double number, long integer, object? reference)
{
    /// <summary>
    /// Gets the Luau type of this value.
    /// </summary>
    public LuauType Type { get; } = type;

    /// <summary>
    /// Gets the numeric value.
    /// </summary>
    /// <remarks>
    /// Valid only when <see cref="Type"/> is <see cref="LuauType.Number"/>
    /// or <see cref="LuauType.Boolean"/>.
    /// </remarks>
    public double Number { get; } = number;

    /// <summary>
    /// Gets the integer value.
    /// </summary>
    /// <remarks>
    /// Valid only when <see cref="Type"/> is <see cref="LuauType.Integer"/>.
    /// </remarks>
    public long Integer { get; } = integer;

    /// <summary>
    /// Gets the reference-backed value.
    /// </summary>
    /// <remarks>
    /// Used for strings and other managed reference-backed Luau values.
    /// </remarks>
    public object? Reference { get; } = reference;

    /// <summary>
    /// Converts this value into its managed CLR representation.
    /// </summary>
    /// <returns>
    /// The managed representation of the value,
    /// or <see langword="null"/> if the value is <c>nil</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The value does not contain the expected backing reference.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The value type is not supported for CLR conversion.
    /// </exception>
    public object? ToClr()
    {
        return Type switch
        {
            LuauType.Nil => null,
            LuauType.Boolean => Number != 0,
            LuauType.Integer => Integer,
            LuauType.Number => Number,
            LuauType.String => Reference is string s ? s : throw new InvalidOperationException("Luau string value has no backing string."),
            LuauType.Function => new LuauFunction(
                Reference as Luau
                    ?? throw new InvalidOperationException("Luau function value has no owning Luau instance."),
                (int)Number),
            _ => throw new NotSupportedException($"Unsupported Luau type: {Type}")
        };
    }

    /// <summary>
    /// Returns the string representation of this value.
    /// </summary>
    /// <returns>
    /// The CLR string representation of the value,
    /// or <c>"nil"</c> if the value is nil.
    /// </returns>
    public override string ToString()
    {
        return ToClr()?.ToString() ?? "nil";
    }

    /// <summary>
    /// Converts a double into a Luau number value.
    /// </summary>
    public static implicit operator LuauValue(double value) => new(LuauType.Number, value, 0, null);

    /// <summary>
    /// Converts a string into a Luau string value.
    /// </summary>
    public static implicit operator LuauValue(string value) => new(LuauType.String, 0, 0, value);

    /// <summary>
    /// Converts a boolean into a Luau boolean value.
    /// </summary>
    public static implicit operator LuauValue(bool value) => new(LuauType.Boolean, value ? 1 : 0, 0, null);

    /// <summary>
    /// Converts a 64-bit integer into a Luau integer value.
    /// </summary>
    public static implicit operator LuauValue(long value) => new(LuauType.Integer, 0, value, null);

    /// <summary>
    /// Converts a 32-bit integer into a Luau number value.
    /// </summary>
    public static implicit operator LuauValue(int value) => new(LuauType.Number, value, 0, null);

    /// <summary>
    /// Gets a Luau nil value.
    /// </summary>
    public static LuauValue Nil => new(LuauType.Nil, 0, 0, null);
}
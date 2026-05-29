namespace LuauInterop;

/// <summary>
/// Represents an exception that occurs during Luau compilation or execution.
/// </summary>
public class LuauException : Exception
{
    public LuauException(string message) : base(message)
    {
    }

    public LuauException(string message, Exception ex) : base(message, ex)
    {
    }
}

using LuauInterop.Ast.Nodes;

namespace LuauInterop.Ast;

public sealed record LuauParseError(Location Location, string Message)
{
    public override string ToString()
    {
        return $"({Location}): {Message}";
    }
};
namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeError(Location location, IReadOnlyList<AstType> types, double messageIndex) : AstType(location)
{
    public IReadOnlyList<AstType> Types { get; } = types;
    public double MessageIndex { get; } = messageIndex;
}
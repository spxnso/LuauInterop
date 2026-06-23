namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeIntersection(Location location, IReadOnlyList<AstType> types) : AstType(location)
{
    public IReadOnlyList<AstType> Types { get; } = types;
}
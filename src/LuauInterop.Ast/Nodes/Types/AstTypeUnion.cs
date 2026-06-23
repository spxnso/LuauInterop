namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeUnion(Location location, IReadOnlyList<AstType> types) : AstType(location)
{
    public IReadOnlyList<AstType> Types { get; } = types;
}
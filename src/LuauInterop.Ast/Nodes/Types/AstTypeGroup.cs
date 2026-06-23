namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeGroup(Location location, AstType innerType) : AstType(location)
{
    public AstType InnerType { get; } = innerType;
}

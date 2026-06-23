namespace LuauInterop.Ast.Nodes.Types;

public class AstTypePackVariadic(Location location, AstType variadicType) : AstTypePack(location)
{
    public AstType VariadicType { get; } = variadicType;
}

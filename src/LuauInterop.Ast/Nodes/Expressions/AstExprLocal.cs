namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprLocal(Location location, AstLocal local) : AstExpr(location)
{
    public AstLocal Local { get; } = local;
}

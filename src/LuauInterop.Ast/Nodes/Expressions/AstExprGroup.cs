namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprGroup(Location location, AstExpr expr) : AstExpr(location)
{
    public AstExpr Expression { get; } = expr;
}

namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprIndexExpr(Location location, AstExpr expr, AstExpr index) : AstExpr(location)
{
    public AstExpr Expression { get; } = expr;
    public AstExpr Index { get; } = index;
}

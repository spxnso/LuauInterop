namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprUnary(Location location, string op, AstExpr expr) : AstExpr(location)
{
    public string Op { get; } = op;
    public AstExpr Expression { get; } = expr;
}

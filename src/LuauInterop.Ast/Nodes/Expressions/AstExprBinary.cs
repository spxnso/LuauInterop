namespace LuauInterop.Ast.Nodes.Expressions;


public class AstExprBinary(Location location, string op, AstExpr left, AstExpr right) : AstExpr(location)
{
    public string Op { get; } = op;
    public AstExpr Left { get; } = left;
    public AstExpr Right { get; } = right;
}

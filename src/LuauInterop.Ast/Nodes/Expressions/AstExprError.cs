namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprError(Location location, IReadOnlyList<AstExpr> expressions, double messageIndex) : AstExpr(location)
{
    public IReadOnlyList<AstExpr> Expressions { get; } = expressions;
    public double MessageIndex { get; } = messageIndex;
}
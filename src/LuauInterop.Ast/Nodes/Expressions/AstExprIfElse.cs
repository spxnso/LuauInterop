namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprIfElse(Location location, AstExpr condition, bool hasThen, AstExpr trueExpr, bool hasElse, AstExpr falseExpr) : AstExpr(location)
{
    public AstExpr Condition { get; } = condition;
    public bool HasThen { get; } = hasThen;
    public AstExpr TrueExpr { get; } = trueExpr;
    public bool HasElse { get; } = hasElse;
    public AstExpr FalseExpr { get; } = falseExpr;
}

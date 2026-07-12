namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprConstantNumber(Location location, double value) : AstExpr(location)
{
    public double Value { get; } = value;
}

namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprConstantInteger(Location location, long value) : AstExpr(location)
{
    public long Value { get; } = value;
}

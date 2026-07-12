namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprConstantBool(Location location, bool value) : AstExpr(location)
{
    public bool Value { get; } = value;
}

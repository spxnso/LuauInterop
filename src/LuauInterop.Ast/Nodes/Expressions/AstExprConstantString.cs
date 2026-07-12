namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprConstantString(Location location, string value) : AstExpr(location)
{
    public string Value { get; } = value;
}

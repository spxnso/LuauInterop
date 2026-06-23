namespace LuauInterop.Ast.Nodes.Expressions;


public class AstExprGlobal(Location location, string name) : AstExpr(location)
{
    public string Name { get; } = name;
}

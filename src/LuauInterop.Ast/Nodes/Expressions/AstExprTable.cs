namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprTable(Location location, IReadOnlyList<AstExprTableItem> items) : AstExpr(location)
{
    public IReadOnlyList<AstExprTableItem> Items { get; } = items;
}

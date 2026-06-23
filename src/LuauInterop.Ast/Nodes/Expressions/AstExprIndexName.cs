namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprIndexName(Location location, AstExpr expr, string index, Location indexLocation, string? op = null) : AstExpr(location)
{
    public AstExpr Expression { get; } = expr;
    public string Index { get; } = index;
    public Location IndexLocation { get; } = indexLocation;
    public string? Op { get; } = op;
}

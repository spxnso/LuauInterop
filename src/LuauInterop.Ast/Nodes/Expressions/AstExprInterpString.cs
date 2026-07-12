namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprInterpString(Location location, List<string> strings, IReadOnlyList<AstExpr> expressions) : AstExpr(location)
{
    public List<string> Strings { get; } = strings;
    public IReadOnlyList<AstExpr> Expressions { get; } = expressions;
}

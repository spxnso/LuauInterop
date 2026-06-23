using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatReturn(Location location, IReadOnlyList<AstExpr> list) : AstStat(location)
{
    public IReadOnlyList<AstExpr> List { get; } = list;
}

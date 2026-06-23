using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatError(Location location, IReadOnlyList<AstExpr> expressions, IReadOnlyList<AstStat> statements) : AstStat(location)
{
    public IReadOnlyList<AstExpr> Expressions { get; } = expressions;
    public IReadOnlyList<AstStat> Statements { get; } = statements;
}
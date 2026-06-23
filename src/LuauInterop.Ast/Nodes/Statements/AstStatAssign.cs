using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatAssign(Location location, IReadOnlyList<AstExpr> vars, IReadOnlyList<AstExpr> values) : AstStat(location)
{
    public IReadOnlyList<AstExpr> Vars { get; } = vars;
    public IReadOnlyList<AstExpr> Values { get; } = values;
}

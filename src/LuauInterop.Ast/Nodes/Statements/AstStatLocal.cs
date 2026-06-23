using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatLocal(Location location, IReadOnlyList<AstLocal> vars, IReadOnlyList<AstExpr> values) : AstStat(location)
{
    public IReadOnlyList<AstLocal> Vars { get; } = vars;
    public IReadOnlyList<AstExpr> Values { get; } = values;
}

using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatFor(Location location, AstLocal var, AstExpr from, AstExpr to, AstExpr? step, AstStatBlock body, bool hasDo) : AstStat(location)
{
    public AstLocal Var { get; } = var;
    public AstExpr From { get; } = from;
    public AstExpr To { get; } = to;
    public AstExpr? Step { get; } = step;
    public AstStatBlock Body { get; } = body;
    public bool HasDo { get; } = hasDo;
}

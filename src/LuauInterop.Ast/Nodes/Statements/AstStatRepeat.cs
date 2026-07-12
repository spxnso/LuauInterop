using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatRepeat(Location location, AstExpr condition, AstStatBlock body) : AstStat(location)
{
    public AstExpr Condition { get; } = condition;
    public AstStatBlock Body { get; } = body;
}

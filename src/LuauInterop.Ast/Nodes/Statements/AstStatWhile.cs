using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatWhile(Location location, AstExpr condition, AstStatBlock body, bool hasDo) : AstStat(location)
{
    public AstExpr Condition { get; } = condition;
    public AstStatBlock Body { get; } = body;
    public bool HasDo { get; } = hasDo;
}

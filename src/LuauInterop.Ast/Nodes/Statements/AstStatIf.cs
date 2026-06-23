using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatIf(Location location, AstExpr condition, AstStatBlock thenBody, AstStat? elseBody, bool hasThen) : AstStat(location)
{
    public AstExpr Condition { get; } = condition;
    public AstStatBlock ThenBody { get; } = thenBody;
    public AstStat? ElseBody { get; } = elseBody;
    public bool HasThen { get; } = hasThen;
}

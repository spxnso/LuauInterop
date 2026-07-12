using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatExpr(Location location, AstExpr expr) : AstStat(location)
{
    public AstExpr Expression { get; } = expr;
}
using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatFunction(Location location, AstExpr name, AstExprFunction func) : AstStat(location)
{
    public AstExpr Name { get; } = name;
    public AstExprFunction Func { get; } = func;
}

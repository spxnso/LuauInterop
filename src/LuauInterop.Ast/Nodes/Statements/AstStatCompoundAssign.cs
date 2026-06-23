using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatCompoundAssign(Location location, string op, AstExpr var, AstExpr value) : AstStat(location)
{
    public string Op { get; } = op;
    public AstExpr Var { get; } = var;
    public AstExpr Value { get; } = value;
}
